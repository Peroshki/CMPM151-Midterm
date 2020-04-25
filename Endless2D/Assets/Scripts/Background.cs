using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject[] gos;
    private int fidelity = 75;
    private Transform parentTranform;
    private float smooth = 5.0f;
    private float seed;

    public int height;
    public GameObject go;

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
        parentTranform = GameObject.Find("BlockHead").transform;
        seed = UnityEngine.Random.value * 100;

        initializeBars(fidelity, parentTranform);
    }

    void initializeBars(int size, Transform head)
    {
        gos = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            gos[i] = Instantiate(go, new Vector3((i % 2 == 0 ? i : -1 * i) / 2.0f, 0, 0), Quaternion.identity);
            gos[i].name = "Block " + (i + 1);
            gos[i].transform.parent = head;
            gos[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    /* UPDATE FUNCTIONS */

    void Update()
    {
        rotateBarParent();

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

    void rotateBarParent()
    {
        float zAngle = Mathf.PerlinNoise(Time.time / 5.0f, seed) * 180.0f; // OR Mathf.Sin(Time.time)

        Quaternion spin = Quaternion.Euler(0, Time.time / 10.0f, zAngle);
        parentTranform.rotation = Quaternion.Slerp(parentTranform.rotation, spin, Time.deltaTime * smooth);
    }

    /* UTLITY FUNCTIONS */

    void GetCameraInfo()
    {
        Debug.Log("Camera Width : " + Camera.main.scaledPixelWidth + "\nCamera Height : " + Camera.main.scaledPixelHeight);
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

