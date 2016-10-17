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

    //
    public bool onGrowth;//成長中フラグ(UIEditor側で必ずチェックを入れておくこと)
    public float[] ChildrenDivRad;
    public Vector3 NPKEnegy;

    GameObject TreePrefab;
    GameObject ParentBranch;
    //float PrismHeight;
    //float PrismGrowthSpeed;
    int NumOfSibling; //自分自身が親ノードにおける何番目の子供なのか

    //Mesh
    int m_divition;
    float m_topRad;
    float m_bottomRad;
    float m_height;

    //Other
    float deltaTime;

    /*---------------------------------------------------- setChildrenDivRad */
    /* 子ノードの半径の割合を設定する
     */
    void setChildrenDivRad()
    {
        //★アルゴリズムの改良をすべし

        int num = Random.Range(1, TreePrefab.GetComponent<TreeController2>().branchDiv);
        num = 2;

        ChildrenDivRad = new float[num];
    
        for(int i = 0; i < num; i++)
        {
            if(i == 0)
            {
                //ChildrenDivRad[0] = Random.Range(6, 9) / 10.0f;
                ChildrenDivRad[0] = 0.6f;
            }
            else
            {
                //ChildrenDivRad[i] = 1.0f - ChildrenDivRad[0];
                ChildrenDivRad[0] = 0.4f;
            }
        }

    }

    /*------------------------------------------------------------- setPrism */
    /* プリズムに必要な値を取得する
     */
    public void getPrismData(string name, ref GameObject Parent, int sibling)
    {
        //set Prism Param
        TreePrefab = GameObject.Find(name);
        ParentBranch = Parent;
        NumOfSibling = sibling;
        
        //calcuration Parameter
        calcPrismParam();
        //draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        return;
    }
    /*---------------------------------------------------- calcPrismParam */
    /* プリズムに必要な値を計算する
     */
     void calcPrismParam()
    {

        if (NPKEnegy.x > TreePrefab.GetComponent<TreeController2>().treeBranchEnegy)
        {
            onGrowth = false;
        }
        else
        {
            /*----------------------------------------------------------- 分割数 */
            m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;

            m_height += TreePrefab.GetComponent<TreeController2>().treeGrowUp;

            if (ParentBranch == TreePrefab)
            {
                /*--------------------------------------------------- NPK Enegy */
                //[X] : 窒素
                NPKEnegy.x = ParentBranch.GetComponent<TreeController2>().NPKEnegy.x;
                Debug.Log("ENEGY : " + NPKEnegy.x);
                //[Y] : リン酸
                NPKEnegy.y = ParentBranch.GetComponent<TreeController2>().NPKEnegy.y;
                //[Z] : カリウム
                NPKEnegy.z = ParentBranch.GetComponent<TreeController2>().NPKEnegy.z;

                NPKEnegy.x -= m_height * TreePrefab.GetComponent<TreeController2>().treeDecrease;
                NPKEnegy.y += 0;
                NPKEnegy.z += 0;

                /*-------------------------------------------- Bottom Radius */
                m_bottomRad =
                    ParentBranch.GetComponent<TreeController2>().NPKEnegy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRad;
                
                /*----------------------------------------------- Top Radius */
                m_topRad =
                    NPKEnegy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRad;
            }
            else
            {
                /*--------------------------------------------------- NPK Enegy */
                //[X] : 窒素
                NPKEnegy.x = ParentBranch.GetComponent<PrismController>().NPKEnegy.x
                    * ParentBranch.GetComponent<PrismController>().ChildrenDivRad[NumOfSibling];
                //[Y] : リン酸
                NPKEnegy.y = ParentBranch.GetComponent<PrismController>().NPKEnegy.y;
                //[Z] : カリウム
                NPKEnegy.z = ParentBranch.GetComponent<PrismController>().NPKEnegy.z;

                NPKEnegy.x -= m_height * TreePrefab.GetComponent<TreeController2>().treeDecrease;
                NPKEnegy.y += 0;
                NPKEnegy.z += 0;

                /*-------------------------------------------- Bottom Radius */
                m_bottomRad =
                    ParentBranch.GetComponent<PrismController>().NPKEnegy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRad;
                
                /*----------------------------------------------- Top Radius */
                m_topRad =
                    NPKEnegy.x
                    * TreePrefab.GetComponent<TreeController2>().offsetRad;
            }
        }

        //ルートノード(0-0)の場合
        //if (ParentBranch == TreePrefab)
        //{
        //    /*--------------------------------- NPKEnegy(窒素, リン酸, カリウム) */
        //
        //    /*------------------------------------------------------- 下半径 */
        //    m_bottomRad += TreePrefab.GetComponent<TreeController2>().treeRadSpd;
        //
        //    /*------------------------------------------------------- 上半径 */
        //    m_topRad = m_bottomRad * TreePrefab.GetComponent<TreeController2>().treeDecrease;
        //
        //}
        ////それ以外のノード
        //else
        //{
        //    
        //    /*------------------------------------------------------- 下半径 */
        //    if (NumOfSibling == 0)
        //    {
        //        m_bottomRad = ParentBranch.GetComponent<CreatePrismMesh>().TopRadius;
        //    }
        //    else
        //    {
        //        m_bottomRad =
        //               ParentBranch.GetComponent<CreatePrismMesh>().TopRadius
        //             * ParentBranch.GetComponent<PrismController>().ChildrenDivRad[NumOfSibling];
        //
        //    }
        //
        //    /*------------------------------------------------------- 上半径 */
        //    m_topRad = m_bottomRad * TreePrefab.GetComponent<TreeController2>().treeDecrease;
        //}

        /*------------------------------------------------------------- 高さ */
        //if (m_height > TreePrefab.GetComponent<TreeController2>().branchHeight){
        //    //4.ノードが完全に成長しきったら
        //    m_height = TreePrefab.GetComponent<TreeController2>().branchHeight;
        //    onGrowth = false;
        //}
        //else{
        //    m_height += TreePrefab.GetComponent<TreeController2>().treeGrowUp;
        //}


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

        onGrowth = true;
        //PrismGrowthSpeed = 0;
        //PrismHeight = 0;

        //Mesh
        m_topRad = 0;
        m_bottomRad = 0;
        m_height = 0;

        setChildrenDivRad();

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
