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
    /*******************************
     * ◆ ノードの成長における状態遷移
     * 0:成長中
     * 1:分岐ノード
     * 2:先端ノード
     * 3:先端ノード成長停止
     * 4:全ノード成長停止
     *******************************/
    public int GrowUpState;//
    /*******************************
     * ◆ ノードの枝の属性
     * 0:幹
     * 1:主枝
     * 2:その他の枝
     *******************************/
    public int PartsType;
    public bool GrowingPoint;       //先端ノードか否か
    public GameObject TreePrefab;   //どの樹木に属しているか
    public GameObject ParentBranch; //親要素を格納する
    public Vector3 NPKEnergy;       //エネルギー量
    public Vector3 TmpNPKEnergy;    //花実エネルギーの貯蓄
    public int NumOfHierarchy;      //樹木における階層構造
    public int SiblingOfHierarchy;  //階層構造に対して何番目の子供なのか
    public int SiblingOfParent;     //親ノードに対して何番目の子供なのか

    /*-------------------------------------------------------------- private */
    //Branch
    bool onGenerateBranch;        //枝分岐用フラグ
    float sproutHeight = 0.1f;    //発芽から分岐するまでの長さ

    //Mesh
    int m_divition;
    float m_topRad;
    float m_bottomRad;
    float m_height;

    //Other
    float deltaTime;

    /*------------------------------------------------------------- FindDeep */
    /* 1. string name : 検索対象となるGameObject名を指定
     *-----------------------------------------------------------------------*/
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

    /*=======================================================================*
     * ◆Init関数群
     * 　・ノードの本数とエネルギー量の割合
     * 　
     * 　
     *=======================================================================*/

 

    /*=======================================================================*
     * ◆set Transform 関数群
     * 　ノードの位置, 姿勢を計算する
     * 　
     * 　
     *=======================================================================*/

    /*----------------------------------------------- setMainBranchTransform */
    /* 幹と直結している枝の角度
     *-----------------------------------------------------------------------*/
    void setTrunkBranchTransform(GameObject Branch, int NumOfBranches, float pitch, float randPitch)
    {
        Vector3 Direct = new Vector3(0, 0, 0);
        GameObject RefBranch;

        int randYaw = 10;

        /*------------------------------------------------------ 枝の角度調節 */
        if (NumOfBranches > 1)
        {
            /*------------------------------------- ひとつ前の枝の位置を求める */
            int i = Branch.GetComponent<PrismController>().NumOfHierarchy - 1;
            do
            {
                if (FindDeep(TreePrefab, i + "-" + 1)
                    .GetComponent<PrismController>().PartsType == 1)
                {
                    break;
                }
                else
                {
                    i--;
                }
            } while (i > 0);

            RefBranch = FindDeep(TreePrefab, i + "-" + 1).gameObject;

            /*---------------------------------------------------- 角度の設定 */
            switch (NumOfBranches % 4)
            {
                case 1:
                    int num = Mathf.FloorToInt(NumOfBranches / 4.0f);

                    Direct.x = Random.Range(pitch, pitch+randPitch);
                    Direct.y = RefBranch.transform.eulerAngles.y + 180.0f * (1 / (num * 4.0f))
                             + Random.Range(-randYaw / 2.0f, randYaw / 2.0f);
                    break;
                case 3:
                    Direct.x = Random.Range(pitch, pitch + randPitch);
                    Direct.y = RefBranch.transform.eulerAngles.y + 90.0f
                             + Random.Range(-randYaw / 2.0f, randYaw / 2.0f);
                    break;
                default:
                    Direct.x = Random.Range(pitch, pitch + randPitch);
                    Direct.y = RefBranch.transform.eulerAngles.y + 180.0f
                             + Random.Range(-randYaw / 2.0f, randYaw / 2.0f);
                    break;
            }
        }
        /*---------------------------------------------------- 最初の一回のみ */
        else
        {
            Direct.x = Random.Range(pitch, pitch + randPitch);
            Direct.y = Random.Range(0, 360);
        }

        /*---------------------------------------------------------- 適用部分 */
        //set Position(Local)
        Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
        //set Rotation(World)
        Branch.transform.rotation = Quaternion.Euler(
            Direct.x,
            Direct.y,
            Direct.z
            );

        return;
    }
    /*----------------------------------------------- setMainBranchTransform */
    /* 末節枝の角度
     *-----------------------------------------------------------------------*/
    void setSubBranchTransform(GameObject Branch, float pitch, float randPitch)
    {
        float RotPitch = Random.Range(pitch, pitch + randPitch);
        if (Random.Range(0.0f, 2.0f) < 1.0f) RotPitch *= -1;


        /*---------------------------------------------------------- Position */
        //set Position(Local)
        Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
 

        /*---------------------------------------------------------- Rotation */
        //set Rotation(World)
        /*-------------------------------------------- 親ノードと方向を揃える */
        GameObject SibBranch = FindDeep(
            TreePrefab,
            Branch.GetComponent<PrismController>().NumOfHierarchy
            + "-"
            + (int)(Branch.GetComponent<PrismController>().SiblingOfHierarchy - 1)
            );
        Branch.transform.localRotation = Quaternion.Euler(SibBranch.transform.localEulerAngles);

        /*------------------------------- ワールド座標系のY軸方向に回転させる */
        Branch.transform.Rotate(
            0,
            RotPitch,
            0,
            Space.World
            );

        return;
    }

    /*--------------------------------------------------- setBranchTransform */
    /* 枝全体の角度を決める
     *-----------------------------------------------------------------------*/
    void setBranchTransform(ref GameObject Branch, int SiblingOfHierarchy, int SiblingOfParent)
    {
        switch (SiblingOfHierarchy)
        {
            /*=========================================================== 幹 */
            case 0:
                Branch.GetComponent<PrismController>().PartsType = 0;
                Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
                Branch.transform.localRotation = Quaternion.Euler(
                    Random.Range(
                        TreePrefab.GetComponent<TreeController2>().BranchMainPit[0],
                        TreePrefab.GetComponent<TreeController2>().BranchMainPit[1]
                        ),
                    Random.Range(0, 360),
                    0
                    );
                break;
            /*======================================================= 幹以外 */
            default:
                /*--------------------------------------------------- Main枝 */
                if ((Branch.GetComponent<PrismController>().SiblingOfParent % 2) == 0)
                {
                    Branch.GetComponent<PrismController>().PartsType = 2;
                    Branch.transform.localPosition = Vector3.up * this.GetComponent<CreatePrismMesh>().Height;
                    Branch.transform.localRotation = Quaternion.Euler(
                        Random.Range(
                            TreePrefab.GetComponent<TreeController2>().BranchMainPit[0],
                            TreePrefab.GetComponent<TreeController2>().BranchMainPit[1]
                            ),
                        Random.Range(0, 360),
                        0
                        );
                }
                /*---------------------------------------------------- Sub枝 */
                else
                {
                    /*----------------------------------- 幹から生えている枝 */
                    if (Branch.transform.parent.GetComponent<PrismController>().SiblingOfHierarchy == 0)
                    {
                        //幹から生えている枝の数のカウント更新
                        TreePrefab.GetComponent<TreeController2>().NumOfBranches++;
                        Branch.GetComponent<PrismController>().PartsType = 1;
                        setTrunkBranchTransform(
                            Branch, 
                            TreePrefab.GetComponent<TreeController2>().NumOfBranches,
                            TreePrefab.GetComponent<TreeController2>().BranchTrunkPit[0],
                            TreePrefab.GetComponent<TreeController2>().BranchTrunkPit[1]
                            );
                    }
                    /*------------------------------------------ その他の枝 */
                    else
                    {
                        Branch.GetComponent<PrismController>().PartsType = 2;
                        setSubBranchTransform(
                            Branch, 
                            TreePrefab.GetComponent<TreeController2>().BranchSubPit[0],
                            TreePrefab.GetComponent<TreeController2>().BranchSubPit[1]
                            );
                    }
                }
                break;
        }
        return;
    }

    /*=======================================================================*
     * ◆Generate関数群
     * 　分岐してノード(枝)を生み出す処理を行う
     * 　・ノードの生成
     * 　・本数, 角度の調整
     *=======================================================================*/

    /*-------------------------------------------------------- setRootBranch */
    /* 0-0ノードの作成
     *-----------------------------------------------------------------------*/
    public void generateRootBranch(string name)
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
        this.NumOfHierarchy = 0;
        //階層構造による兄弟番号
        this.SiblingOfHierarchy = 0;
        //親要素による兄弟番号
        this.SiblingOfParent = 0;

        //Transform
        this.transform.localPosition = TreePrefab.transform.up * 0;

        //花実エネルギー
        this.TmpNPKEnergy.x = 0;
        this.TmpNPKEnergy.y = 0;
        this.TmpNPKEnergy.z = 0;

        //calcuration Parameter
        calcPrismParam();
        //draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        //節のカウント更新
        TreePrefab.GetComponent<TreeController2>().NumOfNodes++;

        return;
    }
    /*-------------------------------------------------- generateBranchPoints*/
    /* 樹木の分岐関数
     *-----------------------------------------------------------------------*/
    void generateBranchPoints()
    {
        /*--------------------------------------- 先端ノードフラグをオフにする */
        this.GrowingPoint = false;
       
        /*--------------------------------- 自分が何番目の子供なのかを検索する */
        int _SiblingOfHierarchy;                     //階層構造による兄弟番号
        int _SiblingOfParent;                        //同親における兄弟番号
        int nextHierarchy = this.NumOfHierarchy + 1; //階層構造における兄弟番号
        GameObject tmp = this.gameObject;            //階層兄弟発見用tmpObj
        int NumOfNodes = 2; 

        /*------------------------------------ 階層構造による兄弟番号の取得 */
        for(_SiblingOfHierarchy = 0; tmp != null; _SiblingOfHierarchy++)
        {
            tmp = FindDeep(TreePrefab, nextHierarchy + "-" + _SiblingOfHierarchy);
        }
        _SiblingOfHierarchy--;
        /*------------------------------------------------------ 枝の本数 */
        if(TreePrefab.GetComponent<TreeController2>().BranchDivition[0] == 2)
        {
            if (Random.Range(0.0f, 1.0f) < 
                (TreePrefab.GetComponent<TreeController2>().BranchDivition[1] * 0.1f))
            {
                NumOfNodes = 2;
            }
            else
            {
                NumOfNodes = 1;
            }
            if(NumOfHierarchy < 1) NumOfNodes = 1;
        }
        /*------------------------------------------------- 何本枝分岐するか */
        for (_SiblingOfParent = 0; _SiblingOfParent < NumOfNodes; _SiblingOfParent++)
        {
            /*--------------------------------------- インスタンスの生成 */
            GameObject Branch = Instantiate(
                TreePrefab.GetComponent<TreeController2>().PrismPrefab
                ) as GameObject;

            //樹木ルートの設定
            Branch.GetComponent<PrismController>().TreePrefab = this.TreePrefab;
            //親オブジェクト
            Branch.GetComponent<PrismController>().ParentBranch = this.gameObject;
            //名前
            Branch.name = nextHierarchy + "-" + _SiblingOfHierarchy;
            //親子構造
            Branch.transform.parent = this.transform;
            //階層構造
            Branch.GetComponent<PrismController>().NumOfHierarchy = nextHierarchy;
            //階層構造による兄弟番号
            Branch.GetComponent<PrismController>().SiblingOfHierarchy = _SiblingOfHierarchy;
            //親要素による兄弟番号
            Branch.GetComponent<PrismController>().SiblingOfParent = _SiblingOfParent;

            //Transform
            setBranchTransform(ref Branch, _SiblingOfHierarchy, _SiblingOfParent);

            //葉っぱの生成
            TreePrefab.GetComponent<LeafGenerator>().generateLeaf(Branch);
            //葉っぱの親子構造変更
            if (this.SiblingOfHierarchy == 0)
            {
                foreach (Transform child in this.transform)
                {
                    if (child.tag == "leaf")
                    {
                        child.GetComponent<LeafController>().setLeafParent(Branch.transform);
                    }
                }                    
            }

            //カウンター更新
            _SiblingOfHierarchy++;
            TreePrefab.GetComponent<TreeController2>().NumOfNodes++; //樹木全体の節の数

        }//[for]:SiblingOfParent

        return;
    }
    /*=======================================================================*
     * ◆Status関数群
     * 　・Statusの更新
     * 　
     * 　
     *=======================================================================*/

    /*------------------------------------------------------- checkAllState3 */
    /* 全てのGrowUpStateが3になったかどうか
     *-----------------------------------------------------------------------*/
    bool checkAllState3()
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
                    if(tmp.GetComponent<PrismController>().GrowUpState != 3)
                    {
                        return false;
                    }
                    j++;
                }
            } while (tmp != null);
            j = 0;
            i++;
            tmp = FindDeep(TreePrefab, i + "-" + j);
        }
        while (tmp != null);

        return true;
    }
    /*--------------------------------------------------------- setAllState4 */
    /* 全てのGrowUpStateを４にする
     *-----------------------------------------------------------------------*/
    void setAllState(int state)
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
                    tmp.GetComponent<PrismController>().GrowUpState = state;
                    j++;
                }
            } while (tmp != null);
            j = 0;
            i++;
            tmp = FindDeep(TreePrefab, i + "-" + j);
        }
        while (tmp != null);

        return;
    }
    /*--------------------------------------------------------- updateStatus */
    /* Statusの更新
     *-----------------------------------------------------------------------*/
    void updateStatus()
    {
        /*----------------------------------------------------- Statusの更新 */
        switch (GrowUpState)
        {
            case 0:
                /*-------------------------------------------- UpdateStatus */
                if (NPKEnergy.x >
                     TreePrefab.GetComponent<TreeController2>().BreakEnergy[0])
                {
                    //樹木が養分吸収中だったら
                    if (TreePrefab.GetComponent<TreeController2>().treeState == 0)
                    {
                        GrowUpState = 1;//★枝分岐
                        /*------------ 一定の高さまで成長しないと大きくならない */
                        if (ParentBranch == TreePrefab
                            && deltaTime < TreePrefab.GetComponent<TreeController2>().treeLifeTime[1])
                        {
                            GrowUpState = 0;
                        }
                    }
                    //養分の吸収が終わっていたら
                    else
                    {
                        if ((m_topRad * TreePrefab.GetComponent<TreeController2>().DivEnergyRatio)
                            > TreePrefab.GetComponent<TreeController2>().BranchRadEnd)
                        {
                            GrowUpState = 1;//★枝分岐
                        }
                        else
                        {
                            GrowUpState = 2;//★枝分岐
                        }
                    }
                }
                else
                {
                    if(deltaTime > TreePrefab.GetComponent<TreeController2>().treeLifeTime[0])
                    {
                        GrowUpState = 3;
                    }
                }
                break;
            case 1:
                GrowUpState = 2;
                break;
            case 2:
                if (m_topRad < TreePrefab.GetComponent<TreeController2>().BranchRadEnd)
                {
                    GrowUpState = 3;
                }
                break;
            case 3:
                if (checkAllState3() == true)
                {
                    setAllState(4);
                    TreePrefab.GetComponent<TreeController2>().treeState = 2;//★成長停止
                }
                break;
            case 4:
                break;
            default:
                break;
        }
        return;
    }

    /*=======================================================================*
     * ◆calc(update)関数群
     * 　・値の更新
     * 　
     * 　
     *=======================================================================*/

    /*------------------------------------------------------ calcDeltaHeight */
    /* 1フレームあたりの高さ方向の計算を行う
     *-----------------------------------------------------------------------*/
    float calcDeltaHeight()
    {
        float h;
        h = TreePrefab.GetComponent<TreeController2>().BranchGrowUp
            * (1.0f - (Mathf.Sin(
                            deltaTime 
                            * (Mathf.PI / (
                                (TreePrefab.GetComponent<TreeController2>().treeLifeTime[0]
                                - TreePrefab.GetComponent<TreeController2>().treeLifeTime[1])
                                * 2.0f
                            )
                            ))
                      )
            );
    
        h *= 0.1f;
        h = h * h;
        /*---------------------------------------------------- 高さを０にする */
        if (deltaTime > TreePrefab.GetComponent<TreeController2>().treeLifeTime[0])
        {
           h = 0;
        }
    
        //Debug.Log("Height : " + h);
    
        return h;
    }

    /*------------------------------------------------------ updateTransform */
    /* transformの更新(毎フレーム)
     *-----------------------------------------------------------------------*/
    void updateTransform()
    {
        if (ParentBranch == TreePrefab)
        {

        }
        else
        {
            this.transform.localPosition = Vector3.up * ParentBranch.GetComponent<CreatePrismMesh>().Height;
        }
        return;
    }

    /*--------------------------------------------------- updateTmpNPKEnergy */
    /* 花実ホルモンの更新(上分にも呼出部分あり)
     *-----------------------------------------------------------------------*/
    void updateTmpNPKEnergy()
    {
        GameObject tmp = ParentBranch;

        /*-------------------------------------------------- 兄弟の数を求める */
        int CntOfChildren = 0;
        foreach (Transform child in tmp.transform)
        {
            CntOfChildren++;
        }

        if (CntOfChildren < 2)
        {
            this.TmpNPKEnergy.y = tmp.GetComponent<PrismController>().NPKEnergy.y
                + tmp.GetComponent<PrismController>().TmpNPKEnergy.y;
        }
        else
        {
            /*------------------------- 親ノードがTmpNPKEnergy.yを持っていた場合 */
            if (tmp.GetComponent<PrismController>().TmpNPKEnergy.y > 0)
            {
                switch (this.SiblingOfParent)
                {
                    case 0:
                        this.TmpNPKEnergy.y =
                            tmp.GetComponent<PrismController>().TmpNPKEnergy.y
                            * (1.0f - TreePrefab.GetComponent<TreeController2>().DivEnergyRatio);
                        break;
                    case 1:
                        this.TmpNPKEnergy.y =
                            tmp.GetComponent<PrismController>().TmpNPKEnergy.y
                            * TreePrefab.GetComponent<TreeController2>().DivEnergyRatio;
                        break;
                    default:
                        break;
                }
            }
        }
        

        return;
    }

    /*------------------------------------------------------ updateNPKEnergy */
    /* 成長ホルモンの更新
     *-----------------------------------------------------------------------*/
    void updateNPKEnergy()
    {

        if (ParentBranch == TreePrefab)
        {
            /*--------------------------------------------------- NPK Enegy */
            //[X] : 窒素
            NPKEnergy.x = ParentBranch.GetComponent<TreeController2>().NPKEnergy.x
                        * (1.0f - TreePrefab.GetComponent<TreeController2>().DivEnergyRatio);
            //[Y] : リン酸
            NPKEnergy.y = ParentBranch.GetComponent<TreeController2>().NPKEnergy.x
                        * TreePrefab.GetComponent<TreeController2>().DivEnergyRatio;
            //[Z] : カリウム
            NPKEnergy.z = ParentBranch.GetComponent<TreeController2>().NPKEnergy.z;

            //伸長ホルモンの減算
            NPKEnergy.x -= m_height * TreePrefab.GetComponent<TreeController2>().HgtEnergyRatio;
            NPKEnergy.y += 0;
            NPKEnergy.z += 0;
        }
        else
        {
            updateTmpNPKEnergy();

            /*-------------------------------------------------- 主枝だったら */
            if (this.SiblingOfParent == 0)
            {
                /*------------------------------------------------ NPK Enegy */
                //[X] : 窒素
                NPKEnergy.x =
                    ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                    * (1.0f - TreePrefab.GetComponent<TreeController2>().DivEnergyRatio);
                //[Y] : リン酸
                NPKEnergy.y = 
                    ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                    * TreePrefab.GetComponent<TreeController2>().DivEnergyRatio;
                //[Z] : カリウム
                NPKEnergy.z = ParentBranch.GetComponent<PrismController>().NPKEnergy.z;
            }
            /*-------------------------------------------------- 副枝だったら */
            else
            {
                /*------------------------------------------------ NPK Enegy */
                //[X] : 窒素
                NPKEnergy.x =
                    ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                    * TreePrefab.GetComponent<TreeController2>().DivEnergyRatio;
                //[Y] : リン酸
                NPKEnergy.y =
                    ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                    * (1.0f - TreePrefab.GetComponent<TreeController2>().DivEnergyRatio);
                //[Z] : カリウム
                NPKEnergy.z = ParentBranch.GetComponent<PrismController>().NPKEnergy.z;
            }
            //伸長ホルモンの減算
            NPKEnergy.x -= m_height * TreePrefab.GetComponent<TreeController2>().HgtEnergyRatio;
            NPKEnergy.y += 0;
            NPKEnergy.z += 0;

        }
        return;
    }
    /*------------------------------------------------------ updatePrismMesh */
    /* Meshパラメータの更新
     *-----------------------------------------------------------------------*/
    void updatePrismMesh()
    {
        /*----------------------------------------------------------- 分割数 */
        m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;

        /*------------------------------------------------------------- 高さ */
        //成長中の場合のみ高さを更新する
        m_height += calcDeltaHeight();

        if (ParentBranch == TreePrefab)
        {
            /*---------------------------------------------------- Bottom Radius */
            m_bottomRad =
                TreePrefab.GetComponent<TreeController2>().NPKEnergy.x
                * TreePrefab.GetComponent<TreeController2>().BranchRadRatio;

            /*------------------------------------------------------- Top Radius */
            m_topRad =
                NPKEnergy.x
                * TreePrefab.GetComponent<TreeController2>().BranchRadRatio;
        }
        else
        {
            /*---------------------------------------------------- Bottom Radius */
            m_bottomRad =
                ParentBranch.GetComponent<PrismController>().NPKEnergy.x
                * TreePrefab.GetComponent<TreeController2>().BranchRadRatio;

            /*------------------------------------------------------- Top Radius */
            m_topRad =
                NPKEnergy.x
                * TreePrefab.GetComponent<TreeController2>().BranchRadRatio;
        }
       

        return;
    }
    /*------------------------------------------------------- calcPrismParam */
    /* プリズムに必要な値を計算する
     *-----------------------------------------------------------------------*/
    void calcPrismParam()
    {
        
        updateNPKEnergy();
        updateTransform();
        updatePrismMesh();

        return;
    }
    /*=======================================================================*
     * ◆Mesh関数群
     * 　・Meshの生成
     * 
     * 　
     *=======================================================================*/

    /*---------------------------------------------------- generatePrismMesh */
    /* プリズムにパラメータを設定し、Meshを作成する
     *-----------------------------------------------------------------------*/
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
    /*=======================================================================*/
    void Start ()
    {
        deltaTime = 0;

        GrowUpState = 0;
        GrowingPoint = true;
        onGenerateBranch = false;
        NPKEnergy.x = 0;
        NPKEnergy.y = 0;
        NPKEnergy.z = 0;

        //Mesh
        m_topRad = 0;
        m_bottomRad = 0;
        m_height = 0;

        return;
    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update ()
    {
        deltaTime += Time.deltaTime;

        updateStatus();

        //ノードの更新
        //------------------------------------------- calcuration Parameter
        calcPrismParam();
        //------------------------------------------------------- draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        switch (GrowUpState)
        {
            case 0:
                break;
            case 1:
                generateBranchPoints();//分節点関数の呼び出し
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }


        return;
    }
}
