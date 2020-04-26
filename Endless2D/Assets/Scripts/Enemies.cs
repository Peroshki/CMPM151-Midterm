// Blackthornprod youtube channel was used as reference
// https://www.youtube.com/watch?v=FVCW5189evI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    // speed towards enemies (can be changed in the Unity editor)
    public float speed;

    // set the object that you want to this effect on (player)
    public GameObject effect;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // block would be the name of your player object
        if (other.CompareTag("Block"))
        {
            other.GetComponent<Block>(); // -- health or 
            //any other variable that we want to change if there is a collision
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
