// Blackthornprod youtube channel was used as reference
// https://www.youtube.com/watch?v=FVCW5189evI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Enemies;

    private float timeBtwSpawn;

    // set the time between spawns
    public float startTimeBtwSpawn;

    public float decreaseTime = 1;

    // min amount of time before game becomes to difficult
    public float minTime = 0.65f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (timeBtwSpawn <= 0)
        {
            // begin spawnning enmies
            Instantiate(Enemies, transform.position, Quaternion.identity);
            timeBtwSpawn = startTimeBtwSpawn;

            // check if decrease in spawn time is needed
            if (startTimeBtwSpawn > minTime)
            {
                startTimeBtwSpawn -= decreaseTime;
            }
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }

    }

}
