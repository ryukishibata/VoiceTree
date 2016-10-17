using UnityEngine;
using System.Collections;


public class TreeController2 : MonoBehaviour
{
    /*--------------------------------------------------------------- public */
    public GameObject PrismPrefab;//枝のMeshデータ
    public int treeDivition;      //分割数
    public int treeNumOfBranch;   //節の本数
    public Vector3 NPKEnegy;      //エネルギーの総量[x:窒素][y:燐酸][z:カリウム]
    public float treeHeight;      //高さ
    public float treeGrowUp;      //高さ方向の成長係数
    public float treeRadSpd;      //幅方向の成長係数
    public float treeDecrease;    //１m成長するのに必要なエネルギー減少率
    public int branchDiv;         //１節における最大分割数
    public float branchHeight;    //１節の高さの最大値
    public float treeBranchEnegy; //節をつくるのに必要な最低エネルギー量
    public float offsetRad;       //エネルギー量に対する半径の比率

    /*-------------------------------------------------------------- private */
    int treeState;                //状態遷移
    GameObject parentBranch;      //親となるノードを示す
    int treeNumOfHierarchy;       //階層構造を示す
    int treeNumOfSibling;         //兄弟の数を示す

    const int BRANCHMAX = 50;     //一本当たりの最大ノード数

    Vector3 NPKEnegyMax;          //エネルギー総量のMax値
    float treeSuckFource;         //１秒当たりの根からの吸収力

    /*---------------------------------------------------------------- Other */
    float deltaTime;



    /*------------------------------------------------------------- FindDeep */
    /* 1. string name : 検索対象となるGameObject名を指定
     */
    GameObject FindDeep(string name, bool includeInactive = false)
    {
        var children = this.gameObject.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

    /*----------------------------------------------------------- updateNPKEnegy*/
    /* 樹木のエネルギー吸収率
     */
    void updateNPKEnegy()
    {
        switch (treeState)
        {
            case 0:
                //窒素の吸収
                NPKEnegy.x += treeSuckFource * Time.deltaTime;
                if (NPKEnegy.x > NPKEnegyMax.x)
                {
                    NPKEnegy.x = NPKEnegyMax.x;
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
    /*-------------------------------------------------- generateBranchPoints*/
    /* 樹木の分岐関数
     */
    void generateBranchPoints()
    {
        if (treeNumOfBranch == 0)
        {
            /*------------------------------------------- インスタンスの生成 */
            GameObject Branch = Instantiate(PrismPrefab) as GameObject;
            //名前
            Branch.name = treeNumOfHierarchy + "-" + treeNumOfSibling;
            // 親子構造
            Branch.transform.parent = this.gameObject.transform;
            //Transform
            Branch.transform.localPosition = parentBranch.transform.up * 0;
            // set Prism
            Branch.GetComponent<PrismController>().getPrismData(this.name, ref parentBranch, 0);
            //節のカウント更新
            treeNumOfBranch++;
            treeNumOfHierarchy++;
            treeNumOfSibling = 1;
            parentBranch = Branch;
        }
        else
        {
            /*-------------------------------------------- ノード(枝)の追加 */
            if (!parentBranch.GetComponent<PrismController>().onGrowth)
            {
                int sibling = 0;

                /*--------------------------------- 格節からノードを生成する */
                for (int i = 0; i < treeNumOfSibling; i++)
                {
                    parentBranch = FindDeep((treeNumOfHierarchy - 1) + "-" + i);

                    /*------------------------------------- 何本枝分岐するか */
                    int div = parentBranch.GetComponent<PrismController>().ChildrenDivRad.Length;

                    for (int k = 0; k < div; k++)
                    {
                        /*----------------------------------- インスタンスの生成 */
                        GameObject Branch = Instantiate(PrismPrefab) as GameObject;
                        //名前
                        Branch.name = treeNumOfHierarchy + "-" + sibling;
                        // 親子構造
                        Branch.transform.parent = parentBranch.transform;

                        //Transform
                        if ((k % 2) == 0)
                        {
                            Branch.transform.localPosition = Vector3.up * parentBranch.GetComponent<CreatePrismMesh>().Height;
                            Branch.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            Branch.transform.localPosition = Vector3.up * parentBranch.GetComponent<CreatePrismMesh>().Height;
                            Branch.transform.localRotation = Quaternion.Euler(
                                Random.Range(30, 60),
                                Random.Range(0, 360),
                                0
                                );
                        }
                        // set Prism
                        Branch.GetComponent<PrismController>().getPrismData(this.name, ref parentBranch, k);

                        //カウンター更新
                        sibling++;         //兄弟の数
                        treeNumOfBranch++; //樹木全体の節の数
                        if(treeNumOfBranch == BRANCHMAX)
                        {
                            treeState = 1;
                        }
                    }//[for]:k

                }//[for]:i

                /*----------------------------------------- 階層単位での更新 */
                parentBranch = FindDeep(treeNumOfHierarchy + "-" + 0);
                if(parentBranch == null)
                {
                    treeState = 1;
                }
                treeNumOfSibling = sibling;
                treeNumOfHierarchy++;
            }//[if]
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
        if (this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius < NPKEnegyMax.x * offsetRad)
        {

            treeRadSpd = (NPKEnegyMax.x * offsetRad) * (deltaTime / offsetTime);
        }
        else
        {
            treeRadSpd = 0;
            treeState = 1;//★
        }

        //-------- 高さ係数
        float height_k = this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius;
        if(treeState == 0)
        {
            treeGrowUp = 0.05f;
        }
        else
        {
            treeGrowUp = 0;
        }
        //if ((treeGrowUp -= (0.01f * height_k / 400)) < 0)
        //{
        //    treeGrowUp = 0;
        //}

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
        NPKEnegy.x = 0;
        NPKEnegy.y = 0;
        NPKEnegy.z = 0;
        treeRadSpd = 0;
        treeGrowUp = 0;

        //パラメータの設定
        treeDecrease = 0.85f;//伸長ホルモンの消費パラメータ
        treeBranchEnegy = 0.1f;//枝分岐に必要なホルモン量
        NPKEnegyMax.x = 100.0f;
        NPKEnegyMax.y = 0.0f;
        NPKEnegyMax.z = 0.0f;
        treeSuckFource = 5.0f;
        offsetRad = 0.001f;


        //Branch
        branchDiv = 2;
        branchHeight = 1.0f;

        treeState = 0;
        treeNumOfBranch = 0;
        treeNumOfHierarchy = 0;
        treeNumOfSibling = 0;
        parentBranch = this.gameObject;



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
                generateBranchPoints();
                updateGrowingParam();
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
