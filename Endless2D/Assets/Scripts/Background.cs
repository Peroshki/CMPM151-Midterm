using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public int height;
    public int width;
    public int fidelity;

    public GameObject go;

    private GameObject[] gos;

    void Start()
    {
        gos = new GameObject[fidelity + 1];

        for (int i = 0; i <= fidelity; i++) {
            gos[i] = Instantiate(go, new Vector3(-10 + i * ((float)fidelity) / 5.0f, -5, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        for (int i = 0; i <= fidelity; i++) {
            float yOffset = (Mathf.Sin(Time.time / 1.0f + i * Mathf.PI / 5.0f) + 1.0f);
            gos[i].transform.localScale = new Vector3(1000.0f / ((float)fidelity), yOffset * height, 0);
        }
    }

}

