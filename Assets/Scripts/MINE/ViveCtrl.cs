using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ViveCtrl : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }
    
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId padButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

    private HashSet<ItemCtrl> touchedItems = new HashSet<ItemCtrl>();
    private ItemCtrl closestItem;
    private ItemCtrl interactingItem;
	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
        //No controller
        if (controller == null) { return; }

        if (controller.GetPressDown(gripButton)) {
            float distance = 0.0f;
            float minDistance = float.MaxValue;


            foreach(ItemCtrl item in touchedItems) {
                distance = (item.transform.position - this.transform.position).sqrMagnitude;
                if(distance < minDistance) {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            interactingItem = closestItem;
            closestItem = null;

            if (interactingItem) {
                if (interactingItem.IsInteracting()) {
                    interactingItem.EndInteraction(this);
                }
                interactingItem.StartInteraction(this);
            }
        }

        if (controller.GetPressUp(gripButton) && interactingItem) {
            interactingItem.EndInteraction(this);
        }
	}

    void OnTriggerEnter(Collider coll) {
        ItemCtrl collidedItem = coll.GetComponent<ItemCtrl>();
        if (collidedItem) {
            touchedItems.Add(collidedItem);
        }
    }

    void OnTriggerExit(Collider coll) {
        ItemCtrl collidedItem = coll.GetComponent<ItemCtrl>();
        if (collidedItem) {
            touchedItems.Remove(collidedItem);
        }
    }
}
