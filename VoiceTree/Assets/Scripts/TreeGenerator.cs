using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour {


    public GameObject TreePrefab;
    public int NumOfTree;

    public float ave_volume; //平均的な声の大きさ
    public float talkingTime;//喋っていた時間
    //public int num_

    /*--------------------------------------------- public void GenerateTree */
    /* 樹木の生成点
     */
    public void GenerateTree(Vector3 pos, float aveVol)
    {

        GameObject Tree = Instantiate(TreePrefab) as GameObject;
        //Aplly
        Tree.name = "Tree" + NumOfTree;
        Tree.transform.position = new Vector3(pos.x, 0.0f, pos.z); 

        setTreeDNA(ref Tree, aveVol);

        NumOfTree++;//木の本数取得
    }
    /*----------------------------------------------------------- setTreeDNA */
    /*樹木に必要なパラメータを計算して渡す
     */
    void setTreeDNA(ref GameObject Tree, float aveVol)
    {
        float radius = aveVol;//樹木の半径


        Tree.GetComponent<TreeController>().setParam(
            6,
            radius,//声の大きさベース
            0.90f,//声の高さベース
            0.05f,//声の高さベース
            2//声の高さベース
            );
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        NumOfTree = 0;

        GenerateTree(new Vector3(0, 0, 0), 0.2f);
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
	}
}
