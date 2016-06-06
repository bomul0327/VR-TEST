using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointerCtrl : MonoBehaviour {
    public float width = 0.003f;

    private int layerMask; 
    private LineRenderer lRenderer;

	// Use this for initialization
    void Awake () {
        layerMask = ~(1 << LayerMask.NameToLayer("Controller"));
        lRenderer = GetComponent<LineRenderer>();
    }

	void Start () {
        lRenderer.enabled = true;
        lRenderer.SetWidth(width, width);
	}
	
    public Vector3 LaserPointerOn () {
        if (!lRenderer.enabled) {
            lRenderer.enabled = true;
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        lRenderer.SetPosition(0, ray.origin);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            lRenderer.SetPosition(1, hit.point);
        }
        else {
            lRenderer.SetPosition(1, ray.GetPoint(Mathf.Infinity));
        }
        return hit.point;
    }
    public void LaserPointerOff () {
        lRenderer.enabled = false;
    }
}
