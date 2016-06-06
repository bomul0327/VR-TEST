using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LaserPointerCtrl))]

public class TeleportCtrl : MonoBehaviour {
    private Vector3 destinationVector = Vector3.zero;

    public float blinkTransitionTime = 0.3f;
    public float exitBlinkTransitionTime = 0.5f;


	// Use this for initialization
	void Start () {

	}
	
    public void Blink () {
        SteamVR_Fade.Start(Color.black, exitBlinkTransitionTime);
    }
    
    public void ExitBlink() {
        SteamVR_Fade.Start(Color.clear, exitBlinkTransitionTime);
    }
    public void Teleport () {
        destinationVector = new Vector3(destinationVector.x, transform.parent.position.y, destinationVector.z);
        transform.parent.position = destinationVector;
        Blink();
        Invoke("ExitBlink", blinkTransitionTime);
    }

    public void SetTeleportPosition (Vector3 position) {
        destinationVector = position;
    }

    public Vector3 GetTeleportPosition (Vector3 position) {
        return destinationVector;
    }
}
