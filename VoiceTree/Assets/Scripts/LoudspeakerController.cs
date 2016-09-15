using UnityEngine;
using System.Collections;

public class LoudspeakerController : MonoBehaviour {
    
	GameObject GameDirector;
	GameObject GetAudioData;
	public GameObject R_Controller;

    public bool onTrigger;
    public int triggerState;//(0: DEFAULT, 1:DOWN, -1:UP)
    float mouseX = 0;
    float mouseY = 0;

	/*------------------------------------------------------ mouseController */
	/* Steam VR コントローラで操作する
	 */
	void steamVRController()
	{
		//Transform
		this.transform.position = this.R_Controller.transform.position;
		this.transform.rotation = this.R_Controller.transform.rotation;


	}

	/*------------------------------------------------------ mouseController */
    /* マウスで操作する
	 */
    void mouseController()
    {
		//Translate
        mouseX = (Input.mousePosition.x - (Camera.main.pixelWidth / 2.0f)) * 0.2f;
        mouseY = (Input.mousePosition.y - (Camera.main.pixelHeight / 2.0f)) * 0.2f;
        this.transform.rotation = Quaternion.Euler( -mouseY, mouseX, 0.0f);

        //メガホンのスイッチON/OFF
        if (Input.GetMouseButtonDown(1)){
            this.onTrigger = true;
            this.triggerState = 1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(1)){
            this.onTrigger = false;
            this.triggerState = -1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Stop();
        }
        else{
            this.triggerState = 0;
        }
    }

	/*=======================================================================*/
	// Use this for initialization
	void Start () {
		this.GameDirector = GameObject.Find("GameDirector");
		this.GetAudioData = GameObject.Find("GetAudioData");

        this.onTrigger = false;
		this.triggerState = 0;
    }
	/*=======================================================================*/
	// Update is called once per frame
	void Update () {
		if (this.GameDirector.GetComponent<GameDirector>().VRMode){
			steamVRController();
        }
        else{
            mouseController();
        }
    }
}
