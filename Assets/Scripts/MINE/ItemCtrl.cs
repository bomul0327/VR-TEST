using UnityEngine;
using System.Collections;

public class ItemCtrl : MonoBehaviour {
    private Rigidbody rb;

    private bool currentlyInteracting;

    private ViveCtrl attachedHand;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(attachedHand && currentlyInteracting) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
	}

    public void StartInteraction (ViveCtrl hand) {
        attachedHand = hand;
        this.transform.SetParent(hand.transform);
        currentlyInteracting = true;
    }

    public void EndInteraction (ViveCtrl hand) {
        if (hand == attachedHand) {
            attachedHand = null;
            this.transform.SetParent(null);
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting () {
        return currentlyInteracting;
    }
}
