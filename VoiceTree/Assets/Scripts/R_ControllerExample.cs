using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class R_ControllerExample : MonoBehaviour
{
    public GameObject prefab;
    public Rigidbody attachPoint;
    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;


    GameObject Megaphone;
	SteamVR_Controller.Device device;

    public bool R_onTrigger;
    public int R_triggerState;//0:default, 1,浅く引く, 2:深く引く, -1:離す



    /*=======================================================================*/
    // Use this for initialization
    void Start()
	{
		this.Megaphone = GameObject.Find ("Megaphone");
	}
	/*=======================================================================*/
	// Update is called once per frame
    void Update()
    {
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int) trackedObject.index);

        /*------------------------------------------------------ 右トリガー */
		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			this.R_onTrigger = true;
			this.R_triggerState = 1;
		}
		else if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
            this.R_triggerState = 2;
        }
		else if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			this.R_onTrigger = false;
            this.R_triggerState = -1;
		}
		else {
            this.R_triggerState = 0;
		}

        //other
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("トリガーを浅く引いた");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("トリガーを深く引いた");
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) {
            Debug.Log("トリガーを離した");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("タッチパッドをクリックした");
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("タッチパッドをクリックしている");
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("タッチパッドをクリックして離した");
        }
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("タッチパッドに触った");
        }
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            Debug.Log("タッチパッドを離した");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
            Debug.Log("メニューボタンをクリックした");
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            Debug.Log("グリップボタンをクリックした");
        }

        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            //Debug.Log("トリガーを浅く引いている");
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
            //Debug.Log("トリガーを深く引いている");
        }
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
            //Debug.Log("タッチパッドに触っている");
        }
    }

    /*-----------------------------------------------------------------------*/
    /* PickUp
     */
     public void pickUp(GameObject target, GameObject hand)
    {
        var go = target;
        go.transform.position = attachPoint.transform.position;

        joint = go.AddComponent<FixedJoint>();
        joint.connectedBody = hand.GetComponent<Rigidbody>();
    }

    /*=======================================================================*/
    // Update is called once per frame
    void FixedUpdate()
    {
        //device = SteamVR_Controller.Input((int)trackedObj.index);

        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //var go = GameObject.Instantiate(prefab);
            //go.transform.position = attachPoint.transform.position;
            //
            //joint = go.AddComponent<FixedJoint>();
            //joint.connectedBody = attachPoint;
            
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            var go = joint.gameObject;
            var rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 15.0f);

            // We should probably apply the offset between trackedObj.transform.position
            // and device.transform.pos to insert into the physics sim at the correct
            // location, however, we would then want to predict ahead the visual representation
            // by the same amount we are predicting our render poses.

            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(device.velocity);
                rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
            }
            else
            {
                rigidbody.velocity = device.velocity;
                rigidbody.angularVelocity = device.angularVelocity;
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
    }
}
