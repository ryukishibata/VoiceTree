﻿using UnityEngine;

public class ControllerExample : MonoBehaviour
{
	GameObject Megaphone;

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


		if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
			this.Megaphone.GetComponent<LoudspeakerController> ().onTrigger = true;
			this.Megaphone.GetComponent<LoudspeakerController> ().triggerState = 1;
		}
		else if (device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
		}
		else if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			this.Megaphone.GetComponent<LoudspeakerController> ().onTrigger = false;
			this.Megaphone.GetComponent<LoudspeakerController> ().triggerState = -1;
		}
		else {
			this.Megaphone.GetComponent<LoudspeakerController> ().triggerState = 0;
		}

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
