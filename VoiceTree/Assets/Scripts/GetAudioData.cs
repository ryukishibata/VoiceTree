using UnityEngine;
using System.Collections;
// 空の Audio Source を作って置く
[RequireComponent(typeof(AudioSource))]

public class GetAudioData : MonoBehaviour {
	public GameObject R_Controller;
	public GameObject megaphone;

	AudioSource microphone;

	public string sentence;
	public float volume;     //0.1:やや大きめの声ではきはきと喋った時の大きさ
	bool onAudioMode = false;//AudioDataを取得する
    

    /*---------------------------------------------------- GetAveragedVolume */
    //マイクのボリューム
    float GetAveragedVolume()
    {
        float[] waveData = new float[256];
        float sum = 0;
        microphone.GetOutputData(waveData, 0);
        foreach (float s in waveData)
        {
            sum += Mathf.Abs(s);
        }
        return sum / waveData.Length;
    }

    /*---------------------------------------------------- setMicrophoneData */
    //マイク音源との接続
    void setMicrophoneData()
    {
		microphone = this.megaphone.GetComponent<AudioSource>();
        // 引数：(デバイス名（null ならデフォルト）, ループ, 何秒取るか, サンプリング周波数)
        microphone.clip = Microphone.Start(null, true, 999, 44100);
        microphone.loop = true;// ループ再生にしておく

        // マイクが Ready になるまで待機（一瞬）
        while (!(Microphone.GetPosition("") > 0)) { }

        // 再生開始（録った先から再生、スピーカーから出力するとハウリングします）
        microphone.Play();

    }


    /*------------------------------------------------------- DummyAudioData */
    //ダミーの音関連データ
    void DummyAudioData()
    {
        this.sentence = "あああああ";
    }
    /*--------------------------------------------------------- getAudioData */
    //AudioDataを取得する
    public void getAudioData()
    {
        if (onAudioMode) {
            this.volume = GetAveragedVolume();
        }
        else {
            this.volume = GetAveragedVolume();//ボリュームの取得
            DummyAudioData();
        }
    }

    /*---------------------------------------------------------------- Start */
    // Use this for initialization
    void Start () {

		//この方法ではうまくいかない......
		//this.megaphone = GameObject.FindGameObjectWithTag("megaphone");

        this.volume = 0;
        this.sentence = "あいうえお";

        setMicrophoneData();
    }

    /*--------------------------------------------------------------- Update */
    // Update is called once per frame
    void Update () {

        //デバイスのトリガがONだったら
        if (this.R_Controller.GetComponent<R_ControllerExample>().R_onTrigger)
        {
            getAudioData();
        }

	}
}
