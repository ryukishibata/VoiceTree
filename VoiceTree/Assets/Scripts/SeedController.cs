using UnityEngine;
using System.Collections;

public class SeedController : MonoBehaviour {
    /*-----------------------------------------------------------------------*
     * Seed STATUS
     * 0:デフォルト
     * 1:言霊の衝突+余韻エフェクト
     * 2:DNAが満タンの状態(デフォルト)
     * 3:DNAが満タンの状態(言霊の衝突+余韻エフェクト)
     *-----------------------------------------------------------------------*/
    //DNA
    public GameObject DNAPrefab;
    int DNACnt;
    const int DNAMAX = 10;
    Vector3 DNAPositon;
    Color DNAColor;

    //Seed
    public Material M_default;
    public Material M_bloom;
    int SeedState;
    float seedAlpha;
    float SeedAlphaMax = 0.75f;
    float SeedAlphaMin = 0.1f;
    float DeltaSpd = 2.0f; //Seedのalpha値が一秒間でどれだけ変化するか

    //Other
    GameObject MainCam;

    /*---------------------------------------------------------- generateDNA */
    /* DNAの生成
     */
    void generateDNA()
    {
        GameObject DNA = Instantiate(DNAPrefab) as GameObject;

        Vector3 moveTo = (this.transform.position - this.DNAPositon).normalized * 0.1f;

        //Apply Param
        DNA.GetComponent<DNAController>().setParam(
            this.DNAPositon + moveTo,
            this.DNAColor
                    );
    }
    /*-------------------------------------------------------- onHitKotodama */
    /* 衝突判定(言霊)
     */
    public void onHitKotodama(Vector3 pos, Color color)
    {
        if (this.DNACnt > DNAMAX){
            //---------------------------------------- Seed
            this.SeedState = 3;
            //Materialの変更
            if (this.DNACnt == DNAMAX + 1) {
                this.SeedAlphaMax = 0.5f;
                this.SeedAlphaMin = 0.01f;
                this.DeltaSpd = 2.0f;
                this.GetComponent<Renderer>().material = this.M_bloom;
            }
            //Audio
            GetComponent<AudioSource>().Play();
        }
        else{
            //---------------------------------------- Seed
            this.SeedState = 1;
            this.seedAlpha = SeedAlphaMin;
            //----------------------------------------- DNA
            this.DNAPositon = pos;
            this.DNAColor = color;
            this.generateDNA();
            this.DNACnt++;
        }
    }
    
    /*-----------------------------------------------------------------------*/
    /*
     */
    // Use this for initialization
    void Start () {
        //Seed
        this.SeedState = 0;
        this.seedAlpha = SeedAlphaMax;
        this.SeedAlphaMax = 0.5f;
        this.SeedAlphaMin = 0.1f;
        this.DeltaSpd = 2.0f;
        this.GetComponent<Renderer>().material = this.M_default;
        //DNA
        this.DNACnt = 0;
        //Other
        this.MainCam = GameObject.Find("MainCamera");
    }

    /*-----------------------------------------------------------------------*/
    /*
     */
    // Update is called once per frame
    void Update () {
        switch (this.SeedState)
        {
            case 0:
                break;
            case 1:
                //Calc Alpha
                this.seedAlpha += DeltaSpd * Time.deltaTime;
                if (this.seedAlpha >= SeedAlphaMax)
                {
                    this.seedAlpha = SeedAlphaMax;
                    this.SeedState = 0;
                }
                //Apply Color
                this.GetComponent<Renderer>().material.SetColor(
                    "_Color",
                    new Color(1.0f, 1.0f, 1.0f, this.seedAlpha)
                    );
                break;
            case 2:
                break;
            case 3:
                //Calc Alpha
                this.seedAlpha += DeltaSpd * Time.deltaTime;
                if (this.seedAlpha >= SeedAlphaMax)
                {
                    this.seedAlpha = SeedAlphaMax;
                    this.SeedState = 0;
                }
                //Apply Color
                this.GetComponent<Renderer>().material.SetColor(
                    "_Color",
                    new Color(1.0f, 1.0f, 1.0f, this.seedAlpha)
                    );
                break;
            default:
                break;
        }

    }
}
