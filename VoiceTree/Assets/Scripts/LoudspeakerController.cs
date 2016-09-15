using UnityEngine;
using System.Collections;

public class LoudspeakerController : MonoBehaviour {

	GameObject SeedPrefab; 
    
	GameObject MainCamera;
	GameObject GameDirector;
	GameObject GetAudioData;
	public GameObject R_Controller;

    float mouseX = 0;
    float mouseY = 0;


    /*------------------------------------------------------ mouseController */
    /* 衝突判定(メガホン)
	 */
     void OnTriggerStay(Collider other)
    {
        /*------------------------------------------------------------- Seed */
        if(other.gameObject.tag == "seed")
        {
			
            switch(R_Controller.GetComponent<R_ControllerExample>().R_triggerState)
            {
                case 1:
                    this.R_Controller.GetComponent<R_ControllerExample>().pickUp(
                        other.gameObject, this.gameObject );
                    break;
                case 2:
                    break;
                case -1:
                    break;
                case 0:
                    break;
                default:
                    break;
            }
            
        }
    }

    /*------------------------------------------------------ mouseController */
    /* Steam VR コントローラで操作する
	 */
    void steamVRController()
	{
		//Position
		Vector3 offsetModelPos = new Vector3(0, 0, 0.001f);
		this.transform.position = 
			this.R_Controller.transform.position + offsetModelPos;
        //Rotation
        Vector3 offsetModelRot = new Vector3(0, 270.0f, 270.0f);
		this.transform.rotation = this.R_Controller.transform.rotation;


	}

	/*------------------------------------------------------ mouseController */
    /* マウスで操作する
	 */
    void mouseController()
    {
        //Position
        this.transform.position = new Vector3(0.2f, 1.4f, 0.3f);
        //Rotate
        mouseX = (Input.mousePosition.x - (Camera.main.pixelWidth / 2.0f)) * 0.2f;
        mouseY = (Input.mousePosition.y - (Camera.main.pixelHeight / 2.0f)) * 0.2f;
        this.transform.rotation = Quaternion.Euler( -mouseY, mouseX, 0.0f);

        //メガホンのスイッチON/OFF
        if (Input.GetMouseButtonDown(1)){
            this.R_Controller.GetComponent<R_ControllerExample>().R_onTrigger = true;
            this.R_Controller.GetComponent<R_ControllerExample>().R_triggerState = 1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(1)){
            this.R_Controller.GetComponent<R_ControllerExample>().R_onTrigger = false;
            this.R_Controller.GetComponent<R_ControllerExample>().R_triggerState = -1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Stop();
        }
        else{
            this.R_Controller.GetComponent<R_ControllerExample>().R_triggerState = 0;
        }
    }

	/*=======================================================================*/
	// Use this for initialization
	void Start () {
        this.MainCamera = GameObject.Find("MainCamera");
        this.GameDirector = GameObject.Find("GameDirector");
		this.GetAudioData = GameObject.Find("GetAudioData");

		this.GetAudioData = GameObject.Find("SeedPrefab");

        this.R_Controller.GetComponent<R_ControllerExample>().R_onTrigger = false;
        this.R_Controller.GetComponent<R_ControllerExample>().R_triggerState = 0;
    }
	/*=======================================================================*/
	// Update is called once per frame
	void Update () {
		if (this.GameDirector.GetComponent<GameDirector>().VRMode){
			steamVRController();

			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("hit");
			}
        }
    }
}
