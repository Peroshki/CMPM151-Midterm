using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC; // Include UnityOSC namespace

public class PdHandler : MonoBehaviour
{
    string pd_to_unity;
    //************* Need to setup this server dictionary...
	Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
	//*************

    public static bool gamePlaying = false;
	public static bool audioPlaying = true;

	private GameObject startText;

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true; //allows unity to update when not in focus

		//************* Instantiate the OSC Handler...
	    OSCHandler.Instance.Init ();
		OSCHandler.Instance.SendMessageToClient("pd", "/unity/init", 1);
        //*************

		startText = GameObject.Find("StartImage");
    }

	void Update() {
		if ((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)) && gamePlaying) {
			Debug.Log("A pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/A", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton1)) {
			Debug.Log("B pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/B", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton2)) {
			Debug.Log("X pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/X", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
			Debug.Log("Y pressed");
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/Y", 1);
		}
		else if (Input.GetKeyDown(KeyCode.JoystickButton6)) {
			Debug.Log("Option pressed");
			audioPlaying = !audioPlaying;
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/toggleaudio", audioPlaying ? 1 : 0);
		}
		else if ((Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Return)) && !gamePlaying) {
			Debug.Log("Start pressed");

			startText.SetActive(false);

			gamePlaying = !gamePlaying;
			OSCHandler.Instance.SendMessageToClient("pd", "/unity/statechange", gamePlaying ? 1 : 0);
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
