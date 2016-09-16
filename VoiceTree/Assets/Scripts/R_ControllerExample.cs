using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class R_ControllerExample : MonoBehaviour
{
	GameObject Megaphone;

    public bool R_onTrigger;
    public int R_triggerState;//0:default, 1,浅く引く, 2:深く引く, -1:離す
	public int R_gripState;

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
        var device = SteamVR_Controller.Input((int) trackedObject.index);

        /*--------------------------------------------------------- Trigger */
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

		/*--------------------------------------------------------- Trigger */
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			this.R_gripState = 1;
		}
		else {
			this.R_gripState = 0;
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
}
