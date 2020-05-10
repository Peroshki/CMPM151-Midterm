using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTLine : MonoBehaviour
{
    public int numPoints = 200;

    private Vector3[] positions;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        allocatePositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (line.positionCount != numPoints)  allocatePositions();

        for (int i = 0; i < numPoints; ++i) {
            positions[i] = new Vector3((i % 2 == 0 ? i : -1 * i) / 4.0f, 0, 0);
        }
        line.SetPositions(positions);
    }

    void allocatePositions() 
    {
        positions = new Vector3[numPoints];
        line.positionCount = numPoints;
    }
}
