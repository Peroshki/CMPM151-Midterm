using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject[] gos;
    private int fidelity = 75;
    private float smooth = 5.0f;
    private float seed;

    public int height;
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

        // Get head object for cloned objects
        seed = UnityEngine.Random.value * 100;

        initializeBars(fidelity);
    }

    void initializeBars(int size)
    {
        gos = new GameObject[size];
        for (int i = 0; i < size; i++) gos[i] = MakeBar(i, "Block", blockhead.transform);
    }


    /* UPDATE FUNCTIONS */

    void Update()
    {
        rotateBarParent(0, blockhead.transform);

        updateBarTransforms();
    }

    void updateBarTransforms()
    {
        for (int i = 0; i < fidelity; i++)
        {
            // Control bar height by phased Sines
            float yOffset = (Mathf.Sin((Time.time + i) * 2) + 1.0f);
            gos[i].transform.localScale = new Vector3(100.0f, yOffset * height, 0);

            // Modify hue over time
            float timeStep = Time.time / 20.0f;
            float hueOffset = timeStep - Mathf.Floor(timeStep);
            gos[i].GetComponent<SpriteRenderer>().color = Color.HSVToRGB(hueOffset, 0.5f, 0.0f);
        }
    }

    void rotateBarParent(float y, Transform parent)
    {
        float zAngle = Mathf.PerlinNoise(Time.time / 5.0f, seed) * 180.0f; // OR Mathf.Sin(Time.time)

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
        GameObject bar = Instantiate(go, new Vector3( (num % 2 == 0 ? num : -1 * num) / 2.0f, 0, 25) , Quaternion.identity);
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

