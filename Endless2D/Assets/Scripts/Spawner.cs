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
    public float maxBtwnSpawn;

    public float decreaseTime;

    // min amount of time before game becomes to difficult
    public float minTime;

    private int num_enemies;

    // Start is called before the first frame update
    void Start()
    {
        num_enemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PdHandler.gameOnMenu)
        {
            if (timeBtwSpawn <= 0)
            {
			    OSCHandler.Instance.SendMessageToClient("pd", "/unity/spawn", 1);
                // begin spawnning enmies
                Instantiate(Enemies, new Vector3(transform.position.x, Random.Range(-3.9f, 5f), 0f), Quaternion.identity);
                timeBtwSpawn = Random.Range(minTime, maxBtwnSpawn);

                ++num_enemies;
                // check if decrease in spawn time is needed
                if (maxBtwnSpawn > (minTime + decreaseTime) && ( num_enemies % 10 == 0 )) maxBtwnSpawn -= decreaseTime;
            }
            else
            { timeBtwSpawn -= Time.deltaTime; }
        }
    }

}
