﻿using UnityEngine;
using System.Collections;
// 空の Audio Source を作って置く
[RequireComponent(typeof(AudioSource))]

public class GetAudioData : MonoBehaviour {
    bool onAudioMode = false;//AudioDataを取得する
    public float volume;     //0.1:やや大きめの声ではきはきと喋った時の大きさ
    public string sentence;

    GameObject megaphone;
    AudioSource microphone;

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
        microphone = GameObject.Find("Megaphone").GetComponent<AudioSource>();
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
        this.sentence = "樹の葉嚙む雌鹿のごとく背を伸ばしあなたの耳にことば吹きたり";
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
        this.megaphone = GameObject.Find("Megaphone");
        
        this.volume = 0;
        this.sentence = "  ";

        setMicrophoneData();
    }

    /*--------------------------------------------------------------- Update */
    // Update is called once per frame
    void Update () {

        //デバイスのトリガがONだったら
        if (this.megaphone.GetComponent<LoudspeakerController>().onTrigger)
        {
            getAudioData();
        }

	}

    
}
