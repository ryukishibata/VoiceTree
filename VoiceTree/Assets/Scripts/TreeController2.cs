using UnityEngine;
using System.Collections;


public class TreeController2 : MonoBehaviour
{
    /*--------------------------------------------------------------- public */
    public GameObject PrismPrefab;       //枝のMeshデータ
    public int treeDivition;             //分割数
    public int treeNumOfBranch;          //節の本数
    public float treeHeight;             //高さ
    public float treeRadius;             //幅
    public Vector3 NPKEnergy;            //エネルギーの総量[x:窒素][y:燐酸][z:カリウム]

    public Vector3 DT_NPKEnergy;         //１秒(DetlaTime)当たりの根からの吸収力
    public float DT_BranchRad;           //幅方向の成長係数
    public float DT_BranchGrowUp;        //高さ方向の成長係数
    public float DT_Egy_BreakPoint;      //１秒(DetlaTime)当たりの枝分岐エネルギーの備蓄率
    public float DM_Egy_Height;          //１m(DeltaMerter)成長するのに必要なエネルギー減少率
    public float Egy_BreakPointMax;      //節をつくるのに必要な最低エネルギー量
    public float offsetRadRatio;         //エネルギー量に対する半径の比率
    
    /*-------------------------------------------------------------- private */
    const int BRANCHMAX = 50;            //一本当たりの最大ノード数
    Vector3 NPKEnergyMax;                //エネルギー総量のMax値

    int treeState;                       //状態遷移

    /*---------------------------------------------------------------- Other */
    float deltaTime;

    /*----------------------------------------------------------- updateNPKEnegy*/
    /* 樹木のエネルギー吸収率
     */
    void updateNPKEnegy()
    {
        switch (treeState)
        {
            case 0:
                //窒素の吸収
                NPKEnergy.x += DT_NPKEnergy.x * Time.deltaTime;

                if (NPKEnergy.x > NPKEnergyMax.x)
                {
                    NPKEnergy.x = NPKEnergyMax.x;
                    treeState = 1;
                }
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;

        }

        return;
    }
    /*--------------------------------------------------- updateGrowingParam */
    /* 樹木の成長パラメータ計算関数
     */
    void updateGrowingParam()
    {
        float offsetTime = 10.0f;//何秒で最大半径になるか

        //-------- 幅係数
        if (this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius < NPKEnergyMax.x * offsetRadRatio)
        {

            DT_BranchRad = (NPKEnergyMax.x * offsetRadRatio) * (deltaTime / offsetTime);
        }
        else
        {
            Debug.Log("State1:Branch Radius is Maxed");
            DT_BranchRad = 0;
            treeState = 1;//★
        }

        //-------- 高さ係数
        float height_k = this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius;
        if(treeState == 0)
        {
            DT_BranchGrowUp = 0.001f;
        }
        else
        {
            DT_BranchGrowUp = 0;
        }

        return;
    }

    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        //初期化
        deltaTime = 0;
        treeDivition = 6;
        treeHeight = 0;
        NPKEnergy.x = 0;
        NPKEnergy.y = 0;
        NPKEnergy.z = 0;
        DT_BranchRad = 0;
        DT_BranchGrowUp = 0;

        //パラメータの設定
        DM_Egy_Height = 0.85f;//伸長ホルモンの消費パラメータ
        Egy_BreakPointMax = 20.0f;//枝分岐に必要なホルモン量
        NPKEnergyMax.x = 100.0f;
        NPKEnergyMax.y = 0.0f;
        NPKEnergyMax.z = 0.0f;
        DT_NPKEnergy.x = 10.0f;
        offsetRadRatio = 0.001f;

        treeState = 0;
        treeNumOfBranch = 0;

        //最初のノードを描画する
        /*------------------------------------------- インスタンスの生成 */
        GameObject Branch = Instantiate(PrismPrefab) as GameObject;
        Branch.GetComponent<PrismController>().setRootBranch(this.name);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        updateNPKEnegy();

        switch (treeState)
        {
            case 0://成長状態
                updateGrowingParam();
                if (treeNumOfBranch == BRANCHMAX)
                {
                    Debug.Log("State1:Branch Maxed");
                    treeState = 1;
                }
                break;
            case 1:
                break;
            case 3:
                break;
            default:
                break;

        }


    }
}
