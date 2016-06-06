using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ViveCtrl : MonoBehaviour {
    public bool isLaserPointerOn = false;
    public bool isTeleportOn = false;

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

    private ItemCtrl interactingItem;
    private LaserPointerCtrl laserPointer;
    private TeleportCtrl teleport;

	// Use this for initialization
	void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        
        if (GetComponent<LaserPointerCtrl>()) {
            laserPointer = GetComponent<LaserPointerCtrl>();
        }
        if (GetComponent<TeleportCtrl>()) {
            teleport = GetComponent<TeleportCtrl>();
        }
	}

	void FixedUpdate () {
        if(controller == null) { return; }
        if (trackedObj.isValid && laserPointer && isLaserPointerOn) {
            teleport.SetTeleportPosition(laserPointer.LaserPointerOn());
        }
    }

	// Update is called once per frame
	void Update () {
        //No controller
        if (controller == null) { return; }

        //Grapping Item
        if (controller.GetPressDown(gripButton)) {
            interactingItem = FindClosestTouchedItem();

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
        //End

        //Teleport
        if (controller.GetPressDown(triggerButton) && isTeleportOn) {
            teleport.Teleport();
        }

        if (controller.GetPressUp(triggerButton)) {

        }
        //End
	}

    ItemCtrl FindClosestTouchedItem () {
        float distance = 0.0f;
        float minDistance = float.MaxValue;
        ItemCtrl closestItem = null;

        foreach (ItemCtrl item in touchedItems) {
            distance = (item.transform.position - this.transform.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                closestItem = item;
            }
        }
        return closestItem;
    }

    void OnTriggerEnter(Collider coll) {
        ItemCtrl collidedItem = coll.GetComponent<ItemCtrl>();
        if (collidedItem) {
            touchedItems.Add(collidedItem);
            
            FindClosestTouchedItem().OutlineOn();
        }
    }

    void OnTriggerExit(Collider coll) {
        ItemCtrl collidedItem = coll.GetComponent<ItemCtrl>();
        if (collidedItem) {
            touchedItems.Remove(collidedItem);
            collidedItem.OutlineOff();
        }
    }
}
