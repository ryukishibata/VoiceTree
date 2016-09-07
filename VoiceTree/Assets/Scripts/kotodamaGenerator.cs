using UnityEngine;
using System.Collections;

public class kotodamaGenerator : MonoBehaviour {

    public GameObject kotodamaPrefab;
    GameObject megaphone; 
    GameObject AudioData;

    /*---------------------------------------------------------------- 音声系 */
    int volume;           //音量

    /*---------------------------------------------------------------- 文字系 */
    string sentence;      //文章データ
    string preSentence;   //ひとつ前の変換結果のデータ
    int s_pointa;         //何文字目のデータかを示す

    int translationPhase ;//何回目の翻訳データか？
    bool onEnd;           //文末までたどり着いたか
    float confidence;     //信頼度(0 - 1.0f)

    /*---------------------------------------------------------------- 言霊系 */
    string character;     //一文字データ
    float force;     //飛ばすときのエネルギー量
    Vector3 localScale;   //大きさ


    /*---------------------------------------------------------------- 時間系 */
    float span;
    float delta;


    /*------------------------------------------ Use this for initialization */
    void Start () {
        this.megaphone = GameObject.Find("Megaphone");
        this.AudioData = GameObject.Find("GetAudioData");

        /*---  パラメータ設定 ---*/
        //音声系
        this.volume = this.AudioData.GetComponent<GetAudioData>().volume;
        
        //文字系
        this.sentence = this.AudioData.GetComponent<GetAudioData>().sentence;
        this.preSentence = this.sentence;
        this.s_pointa = 0;
        this.translationPhase = 0;
        this.onEnd = false;
        this.confidence = 0.0f;

        //言霊系
        this.character = this.sentence[this.s_pointa].ToString();
        this.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        this.force = this.volume;
        this.localScale = new Vector3(
            this.volume / 500.0f,
            this.volume / 500.0f,
            0.1f
            );


        //時間系
        this.span = 0.1f;
        this.delta = 0;
    }
	
	/*-------------------------------------- Update is called once per frame */
	void Update () {

        //デバイスのトリガがUPのとき
        if (this.megaphone.GetComponent<LoudspeakerController>().triggerState == 1)
        {
            this.s_pointa = 0;
            this.span = this.delta;//トリガをひいた瞬間出るようにする

        }
        

        //デバイスのトリガがON
        if (this.megaphone.GetComponent<LoudspeakerController>().onTrigger)
        {

            /*---------------------------------------------- パラメータの更新 */
            //音量系
            this.volume = this.AudioData.GetComponent<GetAudioData>().volume;
            //文字系
            if (this.sentence != this.preSentence) {
                this.sentence = this.AudioData.GetComponent<GetAudioData>().sentence;
            }
            //言霊系
            this.character = this.sentence[this.s_pointa].ToString();
            this.force = this.volume;
            this.localScale = new Vector3(
                this.volume / 500.0f,
                this.volume / 500.0f,
                0.1f
                );
            /*------------------------------------------------- その他の設定 */
            this.delta += Time.deltaTime;

            /*------------------------------------------ インスタンンスの生成 */
            if (this.delta > this.span)
            {
                if (this.s_pointa < this.sentence.Length)
                {
                    GameObject kotodama = Instantiate(kotodamaPrefab) as GameObject;

                    //言霊パラメータの初期化
                    kotodama.GetComponent<kotodamaController>().setKotodamaParam(
                        this.character,
                        megaphone.transform.position,
                        megaphone.transform.rotation,
                        this.localScale
                        );
                    //メガホンから飛び出させる
                    kotodama.GetComponent<kotodamaController>().jumpKotodama(
                        this.force,
                        megaphone.transform.position,
                        megaphone.transform.rotation,
                        this.localScale
                        );

                    /*---------------------------------------- ポインタの更新 */
                    this.s_pointa++;
                    if (this.s_pointa >= this.sentence.Length) {//
                        this.s_pointa = this.sentence.Length - 1;
                    }
                    else if (this.confidence == 1.0f){//信頼度が1.0
                        this.s_pointa = 0;
                    }
                }//[if]:文字配列以内のとき

                this.delta = 0;
            }//[if]:秒間何個言霊が飛ぶか
        }//[if]:デバイストリガON


        //言語変換が更新されたら
        if (this.sentence != this.preSentence){
            this.preSentence = this.sentence;
        }
	}//[update]
}
