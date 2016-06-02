using UnityEngine;
using System.Collections;

public class ItemCtrl : MonoBehaviour {
    private Material[] mat;
    private Rigidbody rb;

    private bool currentlyInteracting;

    private ViveCtrl attachedHand;
    private Transform interactionPoint;

    public float endInteractionDistance = 0.4f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().materials;
        mat[1].SetFloat("_Outline", 0);

        interactionPoint = new GameObject().transform;
        interactionPoint.name = "InteractionPoint";
        interactionPoint.SetParent(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
	    if(attachedHand && currentlyInteracting) {
            float distance = (interactionPoint.transform.position - attachedHand.transform.position).sqrMagnitude;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            if(distance > endInteractionDistance) {
                EndInteraction(attachedHand);
            }
        }
	}
    public void StartInteraction (ViveCtrl hand) {
        attachedHand = hand;
        rb.useGravity = false;
        OutlineOff();

        interactionPoint.position = hand.transform.position;
        interactionPoint.rotation = hand.transform.rotation;

        this.transform.SetParent(hand.transform);
        currentlyInteracting = true;
    }

    public void EndInteraction (ViveCtrl hand) {
        if (hand == attachedHand) {
            attachedHand = null;
            rb.useGravity = true;
            rb.velocity = hand.controller.velocity;
            
            this.transform.SetParent(null);
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting () {
        return currentlyInteracting;
    }

    public void OutlineOn () {
        mat[1].SetFloat("_Outline", 0.003f);
    }

    public void OutlineOff () {
        mat[1].SetFloat("_Outline", 0f);
    }
}
