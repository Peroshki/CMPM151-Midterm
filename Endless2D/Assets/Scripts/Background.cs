using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject[] gos;

    private int numBars;
    private float cachedLeftOffset;

    public GameObject go, blockhead;
    public float leftOffset = -30;



    /* INIT FUNCTIONS */

    void Start()
    {
        // Print camera dimensions
        GetCameraInfo();

        initializeBars(AudioPeer.cleansedData.Length);

    }

    void initializeBars(int size)
    {
        cachedLeftOffset = leftOffset;
        numBars = size;
        foreach (Transform child in GameObject.Find("BlockHead").transform) {
            Destroy(child.gameObject);
        }
        gos = new GameObject[size];
        for (int i = 0; i < size; i++) {
            float xPos = leftOffset + i / 2.0f;
            gos[i] = MakeBar(i, xPos, "Block", blockhead.transform);
            gos[i].GetComponent<SpriteRenderer>().color = Color.black;

        }
    }

    /* UPDATE FUNCTIONS */

    void Update()
    {
        if (numBars != AudioPeer.cleansedData.Length || cachedLeftOffset != leftOffset) initializeBars(AudioPeer.cleansedData.Length);

        for (int i = 0; i < AudioPeer.cleansedData.Length; i++)
        {
            Vector3 newPos = new Vector3(20.0f, AudioPeer.cleansedData[i] * 5000, 0);
            gos[i].transform.localScale = Vector3.Lerp(gos[i].transform.localScale, newPos, AudioPeer.interpolationPos);
        }
        Camera mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCam.backgroundColor = Color.Lerp(mainCam.backgroundColor, AudioPeer.colorOffset, AudioPeer.interpolationPos);
        foreach (Enemies enemy in GameObject.FindObjectsOfType<Enemies>()) {
            float H,S,V;
            Color.RGBToHSV(mainCam.backgroundColor, out H, out S, out V);
            enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H+0.25f, 1, 1);
        }
    }

    /* UTLITY FUNCTIONS */

    void GetCameraInfo()
    {
        Debug.Log("Camera Width : " + Camera.main.scaledPixelWidth + "\nCamera Height : " + Camera.main.scaledPixelHeight);
    }

    GameObject MakeBar(int num, float xPos, string name_type, Transform parent)
    {
        GameObject bar = Instantiate(go, new Vector3(xPos, 0, 0) , Quaternion.identity);
        bar.name = name_type + " " + (num + 1);
        bar.transform.parent = parent;
        bar.GetComponent<SpriteRenderer>().enabled = true;

        return bar;
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using System.Threading;
// using UnityEngine;

// public class Background : MonoBehaviour
// {
//     private GameObject[] gos;
//     private float smooth = 5.0f;
//     private float seed;
//     private float timeOffset = 0;
//     private float perlinOffset;
//     private Color colorOffset;
//     private float delTime;
//     private float[] barSizes;

//     [Range(0.01f, 1f)]
//     public float deltaUpdate;

//     public int numBars = 200;
//     public int height = 10000;
//     public float fftScale;
//     public GameObject go, blockhead;


//     /* INIT FUNCTIONS */

//     void Start()
//     {
//         // Print camera dimensions
//         GetCameraInfo();

//         seed = UnityEngine.Random.value * 100;
//         perlinOffset = (Mathf.PerlinNoise(0, seed)) * 180.0f;

//         initializeBars(numBars);

//         delTime = 0;
//     }

//     void updateBarLerp() {
//         delTime += Time.deltaTime;
//         if (delTime >= deltaUpdate) {
//             delTime = 0;
//             calculateFFTSizes(numBars);
//         }
//     }

//     void initializeBars(int size)
//     {
//         gos = new GameObject[size];
//         for (int i = 0; i < size; i++) {
//             gos[i] = MakeBar(i, "Block", blockhead.transform);
//             gos[i].GetComponent<SpriteRenderer>().color = Color.black;

//         }
//     }

//     /* UPDATE FUNCTIONS */

//     void Update()
//     {
//         updateBarTransforms();
//     }

//     void updateBarTransforms()
//     {
//         updateBarLerp();
//         for (int i = 0; i < numBars; i++)
//         {
//             Vector3 newPos = new Vector3(120.0f, barSizes[i], 0);
//             gos[i].transform.localScale = Vector3.Lerp(gos[i].transform.localScale, newPos, delTime / deltaUpdate);
//         }
//         Camera mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
//         mainCam.backgroundColor = Color.Lerp(mainCam.backgroundColor, colorOffset, delTime / deltaUpdate);
//     }

//     /* UTLITY FUNCTIONS */

//     void GetCameraInfo()
//     {
//         Debug.Log("Camera Width : " + Camera.main.scaledPixelWidth + "\nCamera Height : " + Camera.main.scaledPixelHeight);
//     }

//     GameObject MakeBar(int num, string name_type, Transform parent)
//     {
//         GameObject bar = Instantiate(go, new Vector3( (num % 2 == 0 ? num : -1 * num) / 2.0f, 0, 0) , Quaternion.identity);
//         bar.name = name_type + " " + (num + 1);
//         bar.transform.parent = parent;
//         bar.GetComponent<SpriteRenderer>().enabled = true;

//         return bar;
//     }

//     void calculateFFTSizes(int numPartitions) {
//         // animate the cube size based on sample data
// 		float[] aveMag = new float[numPartitions];
// 		float partitionIndx = 0;
// 		int numDisplayedBins = 512 / 2; //NOTE: we only display half the spectral data because the max displayable frequency is Nyquist (at half the num of bins)

// 		for (int i = 0; i < numDisplayedBins; i++) 
// 		{
// 			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
// 				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
// 			}
// 			else{
// 				partitionIndx++;
// 				i--;
// 			}
// 		}

//         float limit = fftScale * height;
//         int maxind = 0;
// 		// scale and bound the average magnitude.
// 		for(int i = 0; i < numPartitions; i++)
// 		{
// 			aveMag[i] = (float) 0.5 + aveMag[i]*limit;

//             float abs = Mathf.Abs(aveMag[i]);
//             if (abs > aveMag[maxind]) maxind = i;

// 			if (aveMag[i] > limit) aveMag[i] = limit;
// 		}

//         // Assign GLOBAL VARS
//         barSizes = aveMag;
//         colorOffset = Color.HSVToRGB((float)maxind / (float)numPartitions, 1.0f, 0.5f);
//     }
// }

