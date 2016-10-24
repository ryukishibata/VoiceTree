using UnityEngine;
using System.Collections;


/*****************************************************************************
 * ◆ PrismController
 *    プリズムの動きの制御を行う
 *    
 *    1.プリズムの描画に必要な情報をGetしてくる
 *    2.成長、属性(葉っぱなど)の管理を行う
 *    3.CreatePrismMeshにSendする
 *    4.完全に成長しきった場合, <TreeController>に成長停止フラグを渡す
 *****************************************************************************/

public class PrismController : MonoBehaviour {

    /*--------------------------------------------------------------- public */
    public GameObject TreePrefab;  //どの樹木に属しているか
    public GameObject ParentBranch;
    public Vector3 NPKEnergy;       //エネルギー量
    public int NumOfHierarchy;     //樹木における階層構造
    public float[] DivRad; //枝分岐によるエネルギーの割合

    /*-------------------------------------------------------------- private */
    //Branch
    int BranchState;             //成長中フラグ(UIEditor側で必ずチェックを入れておくこと)
    bool onGenerateBranch;        //枝分岐用フラグ
    int NumOfSibling;             //親ノードに対して何番目の子供なのか
    float Egy_BreakPoint;

    //Mesh
    int m_divition;
    float m_topRad;
    float m_bottomRad;
    float m_height;

    //Other
    float deltaTime;

    /*------------------------------------------------------------- FindDeep */
    /* 1. string name : 検索対象となるGameObject名を指定
     */
    GameObject FindDeep(GameObject Parent, string name, bool includeInactive = false)
    {
        var children = Parent.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

    /*---------------------------------------------------- setChildrenDivRad */
    /* 子ノードの半径の割合を設定する
     */
    void setRadiusDivition()
    {
        //★アルゴリズムの改良をすべし

        int num;// = Random.Range(1, TreePrefab.GetComponent<TreeController2>().branchDiv);
        num = 2;

        DivRad = new float[num];
    
        for(int i = 0; i < num; i++)
        {
            if(i == 0)
            {
                //ChildrenDivRad[0] = Random.Range(6, 9) / 10.0f;
                DivRad[0] = 0.7f;
            }
            else
            {
                //ChildrenDivRad[i] = 1.0f - ChildrenDivRad[0];
                DivRad[i] = 0.3f;
            }
        }

    }
    /*-------------------------------------------------------- setRootBranch */
    /* 0-0ノードの作成
     */
    public void setRootBranch(string name)
    {
        //樹木ルートの設定
        TreePrefab = GameObject.Find(name);
        //名前
        this.name = "0-0";
        //親オブジェクト
        this.ParentBranch = GameObject.Find(name);
        //親子構造
        this.transform.parent = this.TreePrefab.transform;
        //階層構造
        NumOfHierarchy = 0;
        //兄弟番号
        NumOfSibling = 0;

        //Transform
        this.transform.localPosition = TreePrefab.transform.up * 0;

        //calcuration Parameter
        calcPrismParam();
        //draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        //節のカウント更新
        TreePrefab.GetComponent<TreeController2>().treeNumOfBranch++;

        return;
    }
    /*-------------------------------------------------- generateBranchPoints*/
    /* 樹木の分岐関数
     */
    void generateBranchPoints()
    {
        setRadiusDivition();

        /*------------------------------- 自分が何番目の子供なのかを検索する */
        GameObject tmp = this.gameObject;
        int nextHierarchy = NumOfHierarchy + 1;
        int sibling;

        for(sibling = 0; tmp != null; sibling++)
        {
            tmp = FindDeep(TreePrefab, nextHierarchy + "-" + sibling);
        }
       sibling--;

        /*------------------------------------- 何本枝分岐するか */
        for (int i = 0; i < DivRad.Length; i++)
        {
            /*----------------------------------- インスタンスの生成 */
            GameObject Branch = Instantiate(
                TreePrefab.GetComponent<TreeController2>().PrismPrefab
                ) as GameObject;

            //樹木ルートの設定
            Branch.GetComponent<PrismController>().TreePrefab = this.TreePrefab;
            //親オブジェクト
            Branch.GetComponent<PrismController>().ParentBranch = this.gameObject;
            //名前
            Branch.name = nextHierarchy + "-" + sibling;
            //親子構造
            Branch.transform.parent = this.transform;
            //階層構造
            Branch.GetComponent<PrismController>().NumOfHierarchy = nextHierarchy;
            //兄弟番号
            NumOfSibling = i;

            //Transform
            if ((i % 2) == 0)

            {
                Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
                Branch.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
                Branch.transform.localRotation = Quaternion.Euler(
                    Random.Range(40, 80),
                    Random.Range(0, 360),
                    0
                    );
            }

            //カウンター更新
            sibling++;
            TreePrefab.GetComponent<TreeController2>().treeNumOfBranch++; //樹木全体の節の数
            
        }//[for]:i

        return;
    }

    /**/
    /*
     */
    float calcDeltaHeight(float baseHeight, float baseRad)
    {
        return 0.001f;// ( baseHeight / (baseRad * 10.0f)) * deltaTime;
    }
    /*---------------------------------------------------- calcPrismParam */
    /* プリズムに必要な値を計算する
     */
    void calcPrismParam()
    {
        /*----------------------------------------------------------- 分割数 */
        m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;

        if (BranchState != 2)
        {
            //成長中の場合のみ高さを更新する
            if (BranchState == 0)
            {
                m_height += calcDeltaHeight(
                    TreePrefab.GetComponent<TreeController2>().DT_BranchGrowUp,
                    TreePrefab.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius
                    );
            }

            if (ParentBranch == TreePrefab)
            {
                /*---------------------------------------------- 枝分岐ホルモン */
                Egy_BreakPoint = ParentBranch.GetComponent<TreeController2>().NPKEnergy.x * 0.3f;


                /*--------------------------------------------------- NPK Enegy */
                //[X] : 窒素
                NPKEnergy.x = ParentBranch.GetComponent<TreeController2>().NPKEnergy.x;
                //[Y] : リン酸
                NPKEnergy.y = ParentBranch.GetComponent<TreeController2>().NPKEnergy.y;
                //[Z] : カリウム
                NPKEnergy.z = ParentBranch.GetComponent<TreeController2>().NPKEnergy.z;

                //枝分岐ホルモンの減算
                NPKEnergy.x = ParentBranch.GetComponent<TreeController2>().NPKEnergy.x * 0.7f;

                //伸長ホルモンの減算
                NPKEnergy.x -= m_height * TreePrefab.GetComponent<TreeController2>().DM_Egy_Height;
                NPKEnergy.y += 0;
                NPKEnergy.z += 0;
                /*-------------------------------------------- Bottom Radius */
                m_bottomRad =
                    ParentBranch.GetComponent<TreeController2>().NPKEnergy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRadRatio;
                /*----------------------------------------------- Top Radius */
                m_topRad =
                    NPKEnergy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRadRatio;
            }
            else
            {
                /*---------------------------------------------- 枝分岐ホルモン */
                Egy_BreakPoint += TreePrefab.GetComponent<TreeController2>().DT_NPKEnergy.x * 0.3f * Time.deltaTime;

                /*--------------------------------------------------- NPK Enegy */
                //[X] : 窒素
                NPKEnergy.x = ParentBranch.GetComponent<PrismController>().NPKEnergy.x;
                //[Y] : リン酸
                NPKEnergy.y = ParentBranch.GetComponent<PrismController>().NPKEnergy.y;
                //[Z] : カリウム
                NPKEnergy.z = ParentBranch.GetComponent<PrismController>().NPKEnergy.z;

                //枝分岐ホルモンの減算
                NPKEnergy.x -= Egy_BreakPoint;

                //伸長ホルモンの減算
                NPKEnergy.x -= m_height * TreePrefab.GetComponent<TreeController2>().DM_Egy_Height;
                NPKEnergy.y += 0;
                NPKEnergy.z += 0;

                //エネルギー量が最小値を上回っている場合
                if (NPKEnergy.x > TreePrefab.GetComponent<TreeController2>().Egy_End)
                {
                    /*-------------------------------------------- Bottom Radius */
                    m_bottomRad =
                        ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                        * TreePrefab.GetComponent<TreeController2>().offsetRadRatio;

                    /*----------------------------------------------- Top Radius */
                    m_topRad =
                        NPKEnergy.x
                        * TreePrefab.GetComponent<TreeController2>().offsetRadRatio;
                }
                else
                {
                    //全ノードの成長を停止させる
                    GameObject tmp;
                    int i = 0;
                    do
                    {
                        int j = 0;
                        do
                        {
                            tmp = FindDeep(TreePrefab, i + "-" + j);
                            if (tmp != null)
                            {
                                tmp.GetComponent<PrismController>().BranchState = 2;//★
                                j++;
                            }
                        } while (tmp != null);
                        j = 0;
                        i++;
                        tmp = FindDeep(TreePrefab, i + "-" + j);
                    }
                    while (tmp != null);
                }

            }

            /*------------------------------------------------ Update Status */
            if (Egy_BreakPoint > TreePrefab.GetComponent<TreeController2>().Egy_BreakPointMax)
            {
                BranchState = 1;//★
                if (onGenerateBranch == false)
                {
                    generateBranchPoints();//分節点関数の呼び出し
                    onGenerateBranch = true;
                }
            }
        }


        return;
    }
    /*---------------------------------------------------- generatePrismMesh */
    /* プリズムにパラメータを設定し、Meshを作成する
     */
    void generatePrismMesh(
        int divition,
        float topRadius,
        float bottomRadius,
        float height)
    {
        GetComponent<CreatePrismMesh>().Divition = divition;
        GetComponent<CreatePrismMesh>().TopRadius = topRadius;
        GetComponent<CreatePrismMesh>().BottomRadius = bottomRadius;
        GetComponent<CreatePrismMesh>().Height = height;
        GetComponent<CreatePrismMesh>().drawPrism();

        return;
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start ()
    {
        deltaTime = 0;

        BranchState = 0;
        onGenerateBranch = false;
        NPKEnergy.x = 0;
        NPKEnergy.y = 0;
        NPKEnergy.z = 0;

        Egy_BreakPoint = 0;



        //Mesh
        m_topRad = 0;
        m_bottomRad = 0;
        m_height = 0;

        return;
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update ()
    {
        deltaTime += Time.deltaTime;

        //ノードの更新
        //------------------------------------------- calcuration Parameter
        calcPrismParam();
        //------------------------------------------------------- draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        return;
    }
}
