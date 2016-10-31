using UnityEngine;
using System.Collections;


public class TreeController2 : MonoBehaviour
{
    /*--------------------------------------------------------------- public */
    public GameObject PrismPrefab;       //枝のMeshデータ
    /**********************************
     * 0:成長中
     * 1:養分を吸い切った状態
     * 2:
     * 3:
     **********************************/
    public int treeState;                //状態遷移
    public int treeDivition;             //分割数
    public float treeHeight;             //高さ
    public float treeRadius;             //幅
    public Vector3 NPKEnergy;            //エネルギーの総量[x:窒素][y:燐酸][z:カリウム]
    public int NumOfBranches;               //主となる枝の本数
    public int NumOfNodes;               //オブジェクトの数

    public Vector3 NPKEnergyMax;         //エネルギー総量のMax値
    public float Egy_BreakPointMax;      //節をつくるのに必要な最低エネルギー量
    public float BranchRadRatio;         //幅方向の成長係数
    public float DivEnergyRatio;         //分岐時の副枝のエネルギー量
    public float BranchRadEnd;           //先端ノードの最小半径
    public Vector3 DT_NPKEnergy;         //１秒(DetlaTime)当たりの根からの吸収力
    public float DT_BranchGrowUp;        //高さ方向の成長係数
    public float DM_Egy_Height;          //１m(DeltaMerter)成長するのに必要なエネルギー減少率
   
    
    /*-------------------------------------------------------------- private */
    const int BRANCHMAX = 50;            //一本当たりの最大ノード数
 
    /*---------------------------------------------------------------- Other */
    float deltaTime;
    float treeLife;




    /*------------------------------------------------------ updateTreeParam */
    void updateTreeParam()
    {
        return;
    }

    /*-------------------------------------------------------- updateNPKEnegy*/
    /* 樹木のエネルギー吸収率
     */
    void updateNPKEnegy()
    {
        switch (treeState)
        {
            case 0:
                //窒素の吸収
                NPKEnergy.x = NPKEnergyMax.x * (Mathf.Sin(deltaTime * (Mathf.PI / (treeLife * 2.0f))));
                if (deltaTime > treeLife)
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
    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        //初期化
        deltaTime = 0;
        treeLife = 5.0f;
        NPKEnergy.x = 0;
        NPKEnergy.y = 0;
        NPKEnergy.z = 0;
        treeHeight = 0;
        treeDivition = 6;


        //パラメータの設定
        DT_NPKEnergy.x = 40.0f;      //毎秒吸い上げる窒素量
        DT_BranchGrowUp = 0.005f;    //毎秒単位の成長率
        DM_Egy_Height = 0.35f;       //成長に伴って消費される伸長ホルモン比
        NPKEnergyMax.x = 200.0f;     //吸い上げる窒素肥料の最大値
        NPKEnergyMax.y = 0.0f;       //吸い上げるリン酸の最大値
        NPKEnergyMax.z = 0.0f;       //吸い上げるカリウムの最大値
        Egy_BreakPointMax = 7.5f;   //枝分岐に必要なホルモン量(大きいほど大きな枝になる)
        BranchRadEnd = 0.5f;         //枝先端の最小栄養ホルモン量
        BranchRadRatio = 0.002f;     //エネルギー量に対する半径比
        DivEnergyRatio = 0.4f;       //分岐時の副枝のエネルギー比率

        
        treeState = 0;
        NumOfBranches = 0;
        NumOfNodes = 0;

        //最初のノードを描画する
        /*------------------------------------------- インスタンスの生成 */
        GameObject Branch = Instantiate(PrismPrefab) as GameObject;
        Branch.GetComponent<PrismController>().generateRootBranch(this.name);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        updateNPKEnegy();
        updateTreeParam();

        switch (treeState)
        {
            case 0://成長状態
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;

        }


    }
}
