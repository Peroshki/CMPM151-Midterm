// Blackthornprod youtube channel was used as reference
// https://www.youtube.com/watch?v=FVCW5189evI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    // speed towards enemies (can be changed in the Unity editor)
    public float speed;

    private float despawn;

    void Start() 
    {
        despawn = GameObject.Find("DespawnPoint").transform.position.x;
    }

    void Update()
    {
        if (!PdHandler.gameOnMenu)
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (gameObject.transform.position.x < despawn)
        {
            Destroy(gameObject);
        }
    }
}
