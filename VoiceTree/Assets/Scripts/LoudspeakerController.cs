using UnityEngine;
using System.Collections;

public class LoudspeakerController : MonoBehaviour {

    bool onWandDevice = false;
    GameObject GetAudioData;

    public bool onTrigger;
    public int triggerState;//(0: DEFAULT, 1:DOWN, -1:UP)



    //マウスで操作する
    void mouseController()
    {
        //メガホンのスイッチON/OFF
        if (Input.GetMouseButtonDown(0)){
            this.onTrigger = true;
            this.triggerState = 1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(0)){
            this.onTrigger = false;
            this.triggerState = -1;

            //メガホンの演出
            //GetComponent<ParticleSystem>().Stop();
        }
        else{
            this.triggerState = 0;
        }
    }

	// Use this for initialization
	void Start () {
        this.onTrigger = false;
        this.triggerState = 0;
        this.GetAudioData = GameObject.Find("GetAudioData");
    }
	
	// Update is called once per frame
	void Update () {
        if (this.onWandDevice)
        {
            //コントローラの値を取得
        }
        else{
            mouseController();
        }
    }
}
