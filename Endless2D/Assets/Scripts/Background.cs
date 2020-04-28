using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject[] gos;
    private float smooth = 5.0f;
    private float seed;
    private float timeOffset = 0;
    private float perlinOffset;


    public int numBars = 200;
    public int height = 10000;
    public GameObject go, blockhead;


    /* Object Hierarchy 
       * Script (Empty) <- (Background.cs)
       * "BlockHead" (Empty) 
         * Block 0  (SpriteRenderer uses "FFFFFF-1" Asset) Attach to "Go" input of Script
    */

    /* INIT FUNCTIONS */

    void Start()
    {
        // Print camera dimensions
        GetCameraInfo();

        seed = UnityEngine.Random.value * 100;
        perlinOffset = (Mathf.PerlinNoise(0, seed)) * 180.0f;

        initializeBars(numBars);
    }

    void initializeBars(int size)
    {
        gos = new GameObject[size];
        for (int i = 0; i < size; i++) gos[i] = MakeBar(i, "Block", blockhead.transform);
    }


    /* UPDATE FUNCTIONS */

    void Update()
    {
        if (PdHandler.gamePlaying) {
            timeOffset += 0.001f;
            rotateBarParent(0, blockhead.transform);
        }

        updateBarTransforms();
    }

    void updateBarTransforms()
    {
        for (int i = 0; i < numBars; i++)
        {
            // Control bar height by phased Sines
            float yOffset = (Mathf.Sin((Time.time + i)) + 1.0f);
            gos[i].transform.localScale = new Vector3(120.0f, yOffset * height, 0);

            // Modify hue over time
            float timeStep = Time.time / 20.0f;
            float hueOffset = timeStep - Mathf.Floor(timeStep);
            gos[i].GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueOffset, 0.5f, 0.0f);
        }
    }

    void rotateBarParent(float y, Transform parent)
    {
        float zAngle = (Mathf.PerlinNoise(timeOffset, seed)) * 180.0f - perlinOffset;

        Quaternion spin = Quaternion.Euler(0, y, zAngle);
        parent.rotation = Quaternion.Slerp(parent.rotation, spin, Time.deltaTime * smooth);
    }

    /* UTLITY FUNCTIONS */

    void GetCameraInfo()
    {
        Debug.Log("Camera Width : " + Camera.main.scaledPixelWidth + "\nCamera Height : " + Camera.main.scaledPixelHeight);
    }

    GameObject MakeBar(int num, string name_type, Transform parent)
    {
        GameObject bar = Instantiate(go, new Vector3( (num % 2 == 0 ? num : -1 * num) / 2.0f, 0, 0) , Quaternion.identity);
        bar.name = name_type + " " + (num + 1);
        bar.transform.parent = parent;
        bar.GetComponent<SpriteRenderer>().enabled = true;

        return bar;
    }

    //void CheckResize()
    //{
    //    if (res[0] != Screen.width || res[1] != Screen.height)
    //    {
    //        Debug.Log("Screen Width : " + Screen.width + "\nScreen Height : " + Screen.height);
    //        res = new float[] { Screen.width, Screen.height };
    //    }
    //}
}

