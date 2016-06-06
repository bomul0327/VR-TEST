using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LaserPointerCtrl))]

public class TeleportCtrl : MonoBehaviour {
    private Vector3 destinationVector = Vector3.zero;

    public float blinkTransitionTime = 0.15f;


	// Use this for initialization
	void Start () {

	}
	
    void Blink () {
        SteamVR_Fade.Start(Color.black, 0);
    }
    
    void ExitBlink() {
        SteamVR_Fade.Start(Color.clear, 1);
    }
    public void Teleport () {
        Blink();
        Invoke("ExitBlink", blinkTransitionTime);            	
        destinationVector = new Vector3(destinationVector.x, transform.parent.position.y, destinationVector.z);
        transform.parent.position = destinationVector;
    }

    public void SetTeleportPosition (Vector3 position) {
        destinationVector = position;
    }

    public Vector3 GetTeleportPosition (Vector3 position) {
        return destinationVector;
    }
}
