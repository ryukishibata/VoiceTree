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
    public void GenerateTree(Vector3 pos)
    {

        GameObject Tree = Instantiate(TreePrefab) as GameObject;
        //Aplly
        Tree.name = "Tree" + NumOfTree;
        Tree.transform.position = pos;
        setTreeDNA(ref Tree);

        NumOfTree++;//木の本数取得
    }
    /*----------------------------------------------------------- setTreeDNA */
    /*樹木に必要なパラメータを計算して渡す
     */
    void setTreeDNA(ref GameObject Tree)
    {

        Tree.GetComponent<TreeController>().setParam(
            6,
            0.5f,//声の大きさベース
            0.8f,//声の高さベース
            0.2f,//声の高さベース
            2//声の高さベース
            );
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        NumOfTree = 0;

        GenerateTree(new Vector3(0, 0, 0));
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
	}
}
