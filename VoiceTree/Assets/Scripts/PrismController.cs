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
    public float BranchEnegy;

    float Enegy;

    GameObject TreePrefab;
    float PrismHeight;
    float PrismGrowthSpeed;

    //Mesh
    int m_divition;
    float m_topRad;
    float m_bottomRad;
    float m_height;

    //Other
    float deltaTime;

    /*------------------------------------------------------------- setPrism */
    /* プリズムに必要な値を取得する
     */
    public void getPrismData(string name)
    {
        //set Prism Param
        TreePrefab = GameObject.Find(name);

        
        //calcuration Parameter
        calcPrismParam();
        //draw Mesh
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, PrismHeight);

        return;
    }
    /*---------------------------------------------------- calcPrismParam */
    /* プリズムに必要な値を計算する
     */
     void calcPrismParam()
    {
        if (onGrowth){
            PrismHeight = deltaTime * 0.2f;//一秒につき1mの速度で成長する
        }
        else{
            PrismHeight = TreePrefab.GetComponent<TreeController2>().branchHeight;
        }

        Enegy = TreePrefab.GetComponent<TreeController2>().treeEnegy;
        BranchEnegy = Enegy - (PrismHeight * TreePrefab.GetComponent<TreeController2>().treeDecrease);

        m_divition = TreePrefab.GetComponent<TreeController2>().treeDivition;
        m_bottomRad = Enegy * 0.002f;
        m_topRad = BranchEnegy * 0.002f;

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
        onGrowth = true;
        PrismGrowthSpeed = 0;
        PrismHeight = 0;

        deltaTime = 0;

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
        generatePrismMesh(m_divition, m_topRad, m_bottomRad, PrismHeight);


        //4.ノードが完全に成長しきったら
        if (PrismHeight > TreePrefab.GetComponent<TreeController2>().branchHeight)
        {
            //------------------------------------------------------ update
            GetComponent<CreatePrismMesh>().Height = 
                TreePrefab.GetComponent<TreeController2>().branchHeight;

            onGrowth = false;
        }
    }
}
