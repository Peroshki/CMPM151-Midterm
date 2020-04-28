using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    [SerializeField] private GameObject platformObject;

    private List<GameObject> platforms;

    private Vector3 spawnPoint;
    private Vector3 despawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint").transform.position;
        despawnPoint = GameObject.Find("DespawnPoint").transform.position;
        platforms = new List<GameObject>();

        float platformWidth = platformObject.GetComponent<BoxCollider2D>().size.x;

        Vector3 xoffset = new Vector3(platformWidth * 2, 0f, 0f);
        Vector3 yzoffset = new Vector3(0f, -7.5f, -1.5f);

        spawnPoint = gameObject.transform.position + xoffset + yzoffset;
        despawnPoint = gameObject.transform.position - xoffset + yzoffset;

        GameObject platform1 = Instantiate(platformObject, spawnPoint, Quaternion.identity);
        GameObject platform2 = Instantiate(platformObject, spawnPoint - xoffset, Quaternion.identity);
        platform1.layer = LayerMask.NameToLayer("Platform");
        platform2.layer = LayerMask.NameToLayer("Platform");

        platforms.Add(platform1);
        platforms.Add(platform2);

        Debug.Log(spawnPoint.ToString());
        Debug.Log(despawnPoint.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject platform in platforms)
        {
            if (platform.transform.position.x < despawnPoint.x)
            {
                platform.transform.position = spawnPoint;
            }

            platform.transform.Translate(Vector2.left * platformSpeed * Time.deltaTime);
        }
    }
}
