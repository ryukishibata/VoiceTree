using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour {
	public bool VRMode;
	public int GameState;

    GameObject MainCamera;
    GameObject megaphone;

    // Use this for initialization
    void Start () {
        /*--------------------------------------------------------- Win Mode */
        if (!this.VRMode)
        {
            this.MainCamera = GameObject.Find("Main Camera");
            this.megaphone = GameObject.Find("Megaphone");

            this.MainCamera.transform.position = new Vector3(0.0f, 1.6f, -1.0f);
            this.megaphone.transform.position = new Vector3(0.0f, 1.3f, 0.0f);

        }
        /*---------------------------------------------------------- VR Mode */
    }
	
	// Update is called once per frame
	void Update () {

        /*--------------------------------------------------------- Win Mode */
        if (!this.VRMode)
        {
        }
    }
}
