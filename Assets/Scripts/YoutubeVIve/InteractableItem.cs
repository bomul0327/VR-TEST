using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour {
    private Rigidbody Rb;

    private bool currentlyInteracting;

    private float velocityFactor = 20000f;
    private Vector3 posDelta;

    private float rotationFactor = 400f;
    private Quaternion rotationDelta;
    private float angle;
    private Vector3 axis;

    private HandCtrl attachedHand;

    private Transform interactionPoint;

	// Use this for initialization
	void Start () {
        Rb = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= Rb.mass;
    }
	
	// Update is called once per frame
	void Update () {
	    if(attachedHand && currentlyInteracting)
        {
            posDelta = attachedHand.transform.position - interactionPoint.position;
            this.Rb.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

            rotationDelta = attachedHand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotationDelta.ToAngleAxis(out angle, out axis);

            if(angle > 180)
            {
                angle -= 360;
            }

            this.Rb.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void BeginInteraction(HandCtrl hand)
    {
        attachedHand = hand;
        interactionPoint.position = hand.transform.position;
        interactionPoint.rotation = hand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(HandCtrl hand)
    {
        if (hand == attachedHand)
        {
            attachedHand = null;
            currentlyInteracting = false;
        }
    }

    public bool IsInteracting()
    {
        return currentlyInteracting;
    }

}
