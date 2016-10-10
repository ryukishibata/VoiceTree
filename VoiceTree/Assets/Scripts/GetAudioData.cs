using UnityEngine;
using System.Collections;
// 空の Audio Source を作って置く
[RequireComponent(typeof(AudioSource))]

public class GetAudioData : MonoBehaviour {
	public GameObject R_Controller;
	public GameObject megaphone;

	AudioSource microphone;

	bool onAudioMode = true;//AudioDataを取得する

    public string sentence;   //文字列
    public float speakingTime;//時間
    public float volume;      //0.1:やや大きめの声ではきはきと喋った時の大きさ
    public float height;

    /*---------------------------------------------------- GetAveragedHeight */
    //高さ
    float GetAveragedHeight(int qSamples, float threshold)
    {

        //int qSamples = 256; //配列のサイズ
        //float threshold = 0.04f; //ピッチとして検出する最小の分布
        //float pitchValue; //ピッチの周波数

        //float[] spectrum = new float[qSamples]; //FFTされたデータ

        //microphone.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        //float max = 0;

        ///*------------------------------------------------------------------*/
        //for (int i = 1; i < spectrum.Length - 1; i++)
        //{
        //    max += spectrum[i];
        //}



        ///*------------------------------------------------------------------*/
        //float maxV = 0;
        //int maxN = 0;
        ////最大値（ピッチ）を見つける。ただし、閾値は超えている必要がある
        //for (int i = 0; i < qSamples; i++)
        //{
        //    if (spectrum[i] > maxV && spectrum[i] > threshold)
        //    {
        //        maxV = spectrum[i];
        //        maxN = i;
        //    }
        //}
        //float freqN = maxN;
        //if (maxN > 0 && maxN < qSamples - 1)
        //{
        //    //隣のスペクトルも考慮する
        //    float dL = spectrum[maxN - 1] / spectrum[maxN];
        //    float dR = spectrum[maxN + 1] / spectrum[maxN];
        //    freqN += 0.5f * (dR * dR - dL * dL);
        //}

        //pitchValue = freqN * (AudioSettings.outputSampleRate / 2) / qSamples;
        //return max;

        float[] spectrum = new float[qSamples];
        microphone.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        //最大値（ピッチ）を見つける。ただし、閾値は超えている必要がある
        for (int i = 0; i < qSamples; i++)
        {
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i;
            }
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < qSamples - 1)
        {
            //隣のスペクトルも考慮する
            float dL = spectrum[maxN - 1] / spectrum[maxN];
            float dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        float pitchValue = freqN * (AudioSettings.outputSampleRate / 2) / qSamples;
        return pitchValue;
    }

    /*---------------------------------------------------- GetAveragedVolume */
    //ボリューム
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
            this.height = GetAveragedHeight(1024, 0.04f);
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
        this.speakingTime = 0;

        this.sentence = "あいうえお";

        setMicrophoneData();
    }

    /*--------------------------------------------------------------- Update */
    // Update is called once per frame
    void Update () {

        //一回だけ入る
        if(this.R_Controller.GetComponent<R_ControllerExample>().R_triggerState == 1)
        {
            //Debug.Log("hit");
            this.speakingTime = 0;
        }
        //デバイスのトリガがONだったら
        if (this.R_Controller.GetComponent<R_ControllerExample>().R_onTrigger)
        {
            this.speakingTime += Time.deltaTime;
            getAudioData();
        }
	}
}
