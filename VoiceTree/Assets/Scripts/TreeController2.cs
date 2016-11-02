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
    public int NumOfNodes;               //オブジェクトの数
    public int NumOfBranches;            //主となる枝の本数
    public float treeHeight;             //高さ
    public float treeRadius;             //幅
    public Vector3 NPKEnergy;            //エネルギーの総量[x:窒素][y:燐酸][z:カリウム]

    public Vector3 NPKEnergyMax;         //エネルギー総量のMax値
    public float[] treeLifeTime;         //木の寿命[0:寿命][1:発芽期]
    public float[] BreakEnergy;          //分岐エネルギー量[0:総量][1:固定値][2:変化幅]
    public float DivEnergyRatio;         //分岐時の副枝のエネルギー量割合
    public float HgtEnergyRatio;         //１m(DeltaMerter)成長するのに必要なエネルギー減少率
    public float BranchRadRatio;         //幅方向の成長係数
    public float[] BranchTrunkPit;       //幹枝のpitch[0:min][2:max]
    public float[] BranchMainPit;        //主枝のpitch[0:min][2:max]
    public float[] BranchSubPit;         //副枝のpitch[0:min][2:max]
    public float BranchRadEnd;           //先端ノードの最小半径
    public float BranchGrowUp;           //高さ方向の成長係数
    public int[] BranchDivition;         //分岐[0:本数][1:割合(x / 10.0f)]        
   
    
    /*-------------------------------------------------------------- private */
    const int BRANCHMAX = 50;            //一本当たりの最大ノード数
 
    /*---------------------------------------------------------------- Other */
    float deltaTime;




    /*-------------------------------------------------------- updateNPKEnegy*/
    /* 樹木のエネルギー吸収率
     *-----------------------------------------------------------------------*/
    void updateNPKEnegy()
    {
        float offsetNEnergy = 0.5f;//芽の状態に使用するエネルギー量

        switch (treeState)
        {
            case 0:
                if (deltaTime < treeLifeTime[1])
                {
                    NPKEnergy.x = offsetNEnergy * (deltaTime / treeLifeTime[1]);
                }
                else
                {
                    //窒素の吸収
                    NPKEnergy.x = offsetNEnergy +
                        (
                            (NPKEnergyMax.x - offsetNEnergy)
                          * (Mathf.Sin((deltaTime - treeLifeTime[1]) * (Mathf.PI / ((treeLifeTime[0] - treeLifeTime[1]) * 2.0f))))
                        );
                    if (deltaTime > treeLifeTime[0])
                    {
                        NPKEnergy.x = NPKEnergyMax.x;
                        treeState = 1;
                    }
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
    /*------------------------------------------------- calcBrakePointEnergy */
    /* 枝分岐に必要な栄養量の更新
     *-----------------------------------------------------------------------*/
    void updateBrakePointEnergy()
    {
        float growTime = this.treeLifeTime[0] - this.treeLifeTime[1];

        // 変化幅を格納する
        this.BreakEnergy[0]
            = this.BreakEnergy[2]
            * ((Mathf.Sin(deltaTime * (Mathf.PI / (growTime * 2.0f)))));
        if (deltaTime > this.treeLifeTime[0])
        {
            this.BreakEnergy[0] = 0;
        }
        //固定値を入れる
        this.BreakEnergy[0]
            += this.BreakEnergy[1];

        return;
    }
    /*=======================================================================*/
    // Use this for initialization
    /*=======================================================================*/
    void Start()
    {
        //初期化
        deltaTime = 0;
        treeLifeTime = new float[2];
        treeLifeTime[0] = 6.0f;         //木の全体寿命
        treeLifeTime[1] = 1.0f;         //発芽期の時間
        NPKEnergy.x = 0;
        NPKEnergy.y = 0;
        NPKEnergy.z = 0;
        treeHeight = 0;
        treeDivition = 6;


        //パラメータの設定
        BreakEnergy = new float[3];  //枝分岐に必要なホルモン量(大きいほど大きな枝になる)
        BreakEnergy[1] = 10.0f;
        BreakEnergy[2] = 3.0f;
        BreakEnergy[0] = BreakEnergy[1] + BreakEnergy[2];
        BranchDivition = new int[] {2, 7 };
        BranchTrunkPit = new float[]{ 40.0f, 30.0f};
        BranchMainPit = new float[]{ 0.0f, 10.0f};
        BranchSubPit = new float[]{ 40.0f, 40.0f};

        NPKEnergyMax.x = 400.0f;     //吸い上げる窒素肥料の最大値
        NPKEnergyMax.y = 0.0f;       //吸い上げるリン酸の最大値
        NPKEnergyMax.z = 0.0f;       //吸い上げるカリウムの最大値
        BranchRadEnd = 0.5f;         //枝先端の最小栄養ホルモン量
        BranchRadRatio = 0.0002f;    //エネルギー量に対する半径比
        DivEnergyRatio = 0.4f;       //分岐時の副枝のエネルギー比率
        HgtEnergyRatio = 0.35f;      //成長に伴って消費される伸長ホルモン比
        BranchGrowUp = 0.7f;

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
    /*=======================================================================*/
    void Update()
    {
        deltaTime += Time.deltaTime;

        updateNPKEnegy();
        updateBrakePointEnergy();

        switch (treeState)
        {
            case 0://
                break;
            case 1://treeLifeSpanが経過したら
                break;
            case 2://
                break;
            default:
                break;
        }
    }
}
