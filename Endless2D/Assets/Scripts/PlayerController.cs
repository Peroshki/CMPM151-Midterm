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

    public GameObject deathText, startImage;

    // Start is called before the first frame update.
    void Start()
    {
        // Get the BoxCollider and Rigidbody of the object this script is attached to.
        rigidBody = transform.GetComponent<Rigidbody2D>();
        boxCollider = transform.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame.
    void Update()
    {
        // If the player is on the ground and the space key is pressed, jump at the set velocity.
        // IsGrounded() && "removed temporarily"
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && !PdHandler.gameOnMenu)
        {
            rigidBody.AddForce(Vector2.up * (IsGrounded() ? 1250 : 750));
            rigidBody.AddTorque(-30);
        }
    }

    private bool IsGrounded()
    {
        // Casts a very short ray down at the ground:
        //    If the player is in the air, ray.collider will return null (false).
        //    Else, ray.collider will return a value (true).
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.01f, layerMask);
        return ray.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
            startImage.SetActive(false);
            deathText.SetActive(true);
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/death", 1);
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/statechange", 0);
            PdHandler.dead = true;
        }
    }
}
