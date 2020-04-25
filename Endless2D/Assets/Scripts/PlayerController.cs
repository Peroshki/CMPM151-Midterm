using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 playerPos;

    public float jumpPower;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Use Vector2.MoveTowards to smoothly transition to the player's new position and multiply 
        // moveSpeed by Time.deltaTime to ensure that the movement is consistent accross all machines.
        //transform.position = Vector2.MoveTowards(transform.position, playerPos, moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(Vector2.up * jumpPower);
        }
    }
}
