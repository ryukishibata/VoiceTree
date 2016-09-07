using UnityEngine;
using System.Collections;

public class GetAudioData : MonoBehaviour {
    bool onAudioMode = false;//AudioDataを取得する
    public int volume;     //100:やや大きめの声ではきはきと喋った時の大きさ
    public string sentence;

    GameObject megaphone;


    //DummyAUdioData
    void DummyAudioData()
    {
        this.volume = 700;
        this.sentence = "樹の葉嚙む雌鹿のごとく背を伸ばしあなたの耳にことば吹きたり";
    }
    //AudioDataを取得する
    void getAudioData()
    {
        if (onAudioMode) {

        }
        else { 
            DummyAudioData();
        }
    }

	// Use this for initialization
	void Start () {
        this.megaphone = GameObject.Find("Megaphone");

        this.volume = 100;
        this.sentence = "樹の葉嚙む雌鹿のごとく背を伸ばしあなたの耳にことば吹きたり";
    }
	
	// Update is called once per frame
	void Update () {
        //デバイスのトリガがONだったら
        if (this.megaphone.GetComponent<LoudspeakerController>().onTrigger)
        {
            getAudioData();
        }

	}
}
