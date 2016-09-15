using UnityEngine;
using System.Collections;

public class SeedController : MonoBehaviour {
    /*-----------------------------------------------------------------------*
     * Seed STATUS
     * 0:デフォルト
     * 1:言霊の衝突+余韻エフェクト
     * 2:DNAが満タンの状態(デフォルト)
     * 3:DNAが満タンの状態(言霊の衝突+余韻エフェクト)
     * 4:
     * 5:
     * 6:
     *-----------------------------------------------------------------------*/
    //DNA
    public GameObject DNAPrefab;
    int DNACnt;
    const int DNAMAX = 100;
    Vector3 DNAPositon;
    Color DNAColor;

    //Seed
    public AudioClip SeedSE;
    public Material M_default;
    public Material M_bloom;
    int SeedState;
    float seedAlpha;
    float SeedAlphaMax = 0.75f;
    float SeedAlphaMin = 0.1f;
    float DeltaSpd = 2.0f; //Seedのalpha値が一秒間でどれだけ変化するか

    //Tree
    GameObject TreeGenerator;

    //Other
    GameObject MainCam;
    float deltaTime;


    /*---------------------------------------------------------- DestroySeed */
    /* 種の消去
     */
    void DestroySeed()
    {
        this.deltaTime += Time.deltaTime;
        if (this.deltaTime > 20.0f)
        {
            this.GetComponent<SphereCollider>().isTrigger = true;
        }

        //種に触れていなくて
        if(this.transform.position.y < -1.0f)
        {
            //Seed
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                GameObject.Destroy(this.transform.GetChild(i).gameObject);
            }
            //樹木の生成
            this.TreeGenerator.GetComponent<TreeGenerator>().GenerateTree(this.transform.position);
        
            Destroy(this.gameObject);
        }
    }


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

    /*---------------------------------------------------------- onHitGround */
    /* 衝突判定(地面)
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ground") {
            
        }

    }

    /*-------------------------------------------------------- onHitKotodama */
    /* 衝突判定(言霊)
     */
    public void onHitSeed(Vector3 pos, Color color)
    {
        if (this.DNACnt >= DNAMAX){
            //---------------------------------------- Seed
            this.SeedState = 3;


            //DNAが100ちょうどのとき
            if (this.DNACnt == DNAMAX) {
                //Audio
                GetComponent<AudioSource>().PlayOneShot(SeedSE, 0.7f);
                //Material
                this.SeedAlphaMax = 0.5f;
                this.SeedAlphaMin = 0.01f;
                this.DeltaSpd = 2.0f;
                this.GetComponent<Renderer>().material = this.M_bloom;
                this.DNACnt++;
                //Rigidbody
                this.GetComponent<Rigidbody>().isKinematic = false;
                //Other
                this.deltaTime = 0;
            }
        }
        else{
            //DNAが100になるまでカウント
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
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Renderer>().material = this.M_default;
        this.GetComponent<SphereCollider>().isTrigger = false;
        //DNA
        this.DNACnt = 0;
        //Tree
        this.TreeGenerator = GameObject.Find("TreeGenerator");
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
                //消えるまでの種の挙動
                DestroySeed();
                break;
            case 3:
                //消えるまでの種の挙動
                DestroySeed();
                //Calc Alpha
                this.seedAlpha += DeltaSpd * Time.deltaTime;
                if (this.seedAlpha >= SeedAlphaMax)
                {
                    this.seedAlpha = SeedAlphaMax;
                    this.SeedState = 2;
                }
                //Apply Color
                this.GetComponent<Renderer>().material.SetColor(
                    "_Color",
                    new Color(1.0f, 1.0f, 1.0f, this.seedAlpha)
                    );
                break;
            case 4:
                break;
            case 5: 
                break;
            default:
                break;
        }

    }
}
