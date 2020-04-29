using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC; // Include UnityOSC namespace
using UnityEngine.SceneManagement;

public class PdHandler : MonoBehaviour
{
    string pd_to_unity;
	public GameObject deathText, startImage;
    //************* Need to setup this server dictionary...
	Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
	//*************

    public static bool gameOnMenu = true;
	public static bool audioPlaying = true;
	public static bool newSpawn = false;
	public static bool dead = false;


    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true; //allows unity to update when not in focus

		//************* Instantiate the OSC Handler...
	    OSCHandler.Instance.Init ();
		OSCHandler.Instance.SendMessageToClient("pd", "/unity/init", 1);
        //*************

    }

	void Update() {
		if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space)) && !gameOnMenu) {
			Debug.Log("A pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/jump", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.N)) {
			Debug.Log("Option pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/statechange", 0);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.B)) {
			Debug.Log("Option pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/statechange", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.M)) {
			Debug.Log("Option pressed");
			audioPlaying = !audioPlaying;
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/toggleaudio", audioPlaying ? 1 : 0);
		}
		else if ((Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Return))) {
			Debug.Log("Start pressed");

			// Restart game
			if (dead) {
				gameOnMenu = true;
				startImage.SetActive(gameOnMenu);
				deathText.SetActive(!gameOnMenu);
				dead = false;
				Application.LoadLevel(0);
			}
			// Toggle pause
			else {
				gameOnMenu = !gameOnMenu;
				startImage.SetActive(gameOnMenu);
				OSCHandler.Instance.SendMessageToClient("pd", "/unity/statechange", gameOnMenu ? 0 : 1);
			}
		}

	}

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");


        //************* Routine for receiving the OSC...
		OSCHandler.Instance.UpdateLogs();
	    Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
		servers = OSCHandler.Instance.Servers;

		foreach (KeyValuePair<string, ServerLog> item in servers) {
			// If we have received at least one packet,
			// show the last received from the log in the Debug console
			if (item.Value.log.Count > 0) {
				int lastPacketIndex = item.Value.packets.Count - 1;

				//get address and data packet
				pd_to_unity = item.Value.packets [lastPacketIndex].Address.ToString ();
				pd_to_unity += item.Value.packets [lastPacketIndex].Data [0].ToString ();

                Debug.Log(pd_to_unity);
			}
		}
		//*************
    }

    void onTriggerEnter(Collider other)
    {
        Debug.Log("-------- COLLISION!!! ----------");

		// Use other.gameObject.CompareTag(str) to differentiate btwn types

		// Send message to Pd by OSCHandler.Instance.SendMessageToClient("pd", "/unity/<object name>", count * 20 + 50);
    }

	void onApplicationQuit() {
		OSCHandler.Instance.SendMessageToClient("pd", "/unity/death", 1);
	}
}
