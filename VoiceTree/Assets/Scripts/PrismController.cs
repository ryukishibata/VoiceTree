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
    //public float BranchEnegy;

    //float Enegy;

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
                ChildrenDivRad[0] = Random.Range(6, 9) / 10.0f;
            }
            else
            {
                ChildrenDivRad[i] = 1.0f - ChildrenDivRad[0];
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
        //generatePrismMesh(m_divition, m_topRad, m_bottomRad, PrismHeight);
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);

        return;
    }
    /*---------------------------------------------------- calcPrismParam */
    /* プリズムに必要な値を計算する
     */
     void calcPrismParam()
    {
        //if (onGrowth){
        //    PrismHeight = deltaTime * 0.3f;//一秒につき1mの速度で成長する
        //}
        //else{
        //    PrismHeight = TreePrefab.GetComponent<TreeController2>().branchHeight;
        //}
        //
        //if (ParentBranch == TreePrefab) {
        //    Enegy = TreePrefab.GetComponent<TreeController2>().treeEnegy;
        //}
        //else{
        //    Enegy = ParentBranch.GetComponent<PrismController>().BranchEnegy;
        //}
        //
        //float mapping = PrismHeight / TreePrefab.GetComponent<TreeController2>().branchHeight;
        //float decrease = TreePrefab.GetComponent<TreeController2>().treeDecrease * mapping;
        //
        //BranchEnegy = Enegy - decrease;
        //
        //m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;
        //m_bottomRad = Enegy * 0.002f;
        //m_topRad = BranchEnegy * 0.002f;

        /*--------------------------------------------------------------------*/
        m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;

        /*----------------------------------------------------------- 下半径 */
        if (ParentBranch == TreePrefab)
        {
            m_bottomRad += TreePrefab.GetComponent<TreeController2>().treeRadSpd;
        }
        else
        {
            m_bottomRad = 
                  ParentBranch.GetComponent<CreatePrismMesh>().TopRadius;
        }

        /*------------------------------------------------------------- 高さ */
        if (m_height > TreePrefab.GetComponent<TreeController2>().branchHeight){
            //4.ノードが完全に成長しきったら
            m_height = TreePrefab.GetComponent<TreeController2>().branchHeight;
            onGrowth = false;
        }
        else{
            m_height += TreePrefab.GetComponent<TreeController2>().treeGrowUp;
        }

        /*----------------------------------------------------------- 上半径 */
        if (ParentBranch == TreePrefab)
        {
            m_topRad = m_bottomRad * TreePrefab.GetComponent<TreeController2>().treeDecrease;
        }
        else
        {
            m_topRad =
                  ParentBranch.GetComponent<CreatePrismMesh>().TopRadius
                * ParentBranch.GetComponent<PrismController>().ChildrenDivRad[NumOfSibling]
                * TreePrefab.GetComponent<TreeController2>().treeDecrease;
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
	void Start () {
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
    void Update () {
        deltaTime += Time.deltaTime;

        //ノードの更新
        //------------------------------------------- calcuration Parameter
        calcPrismParam();
        //------------------------------------------------------- draw Mesh
        //generatePrismMesh(m_divition, m_topRad, m_bottomRad, PrismHeight);
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, m_height);


        //4.ノードが完全に成長しきったら
        //if (m_height > TreePrefab.GetComponent<TreeController2>().branchHeight)
        //{
        //    //------------------------------------------------------ update
        //    GetComponent<CreatePrismMesh>().Height = 
        //        TreePrefab.GetComponent<TreeController2>().branchHeight;
        //
        //    onGrowth = false;
        //}
        return;
    }
}
