using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create a layer called 'Platforms' and set is as the Layer Mask in the Unity Editor.
    // Make sure all platforms are on the 'Platforms' layer.
    [SerializeField] private LayerMask layerMask;

    // Set the jump velocity in the Unity Editor.
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float rotationAmount;

    // Variables to store the BoxCollider and Rigidbody.
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    private GameObject deathText;

    // Start is called before the first frame update.
    void Start()
    {
        // Get the BoxCollider and Rigidbody of the object this script is attached to.
        rigidBody = transform.GetComponent<Rigidbody2D>();
        boxCollider = transform.GetComponent<BoxCollider2D>();

        deathText = GameObject.Find("DeathText");
        deathText.SetActive(false);
    }

    // Update is called once per frame.
    void Update()
    {
        // If the player is on the ground and the space key is pressed, jump at the set velocity.
        if (IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && PdHandler.gamePlaying)
        {
            rigidBody.velocity = Vector2.up * jumpVelocity;

            // Find a way to do smooth rotation
            // rigidBody.rotation = rotationAmount;
        }
    }

    private bool IsGrounded()
    {
        // Casts a very short ray down at the ground:
        //    If the player is in the air, ray.collider will return null (false).
        //    Else, ray.collider will return a value (true).
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, layerMask);
        return ray.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
            deathText.SetActive(true);
        }
    }
}
