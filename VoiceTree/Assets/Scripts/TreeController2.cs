using UnityEngine;
using System.Collections;

public class TreeController2 : MonoBehaviour {

    //プリズムPrefab
    public GameObject PrismPrefab;
    public int treeDivition;  //分割数
    public float treeEnegy;   //エネルギーの総量
    public float treeDecrease;//1m成長するのに必要なのエネルギー量
    public float treeMinEnegy;//節をつくるのに必要な最低エネルギー量
    public float branchHeight;//節までの高さ
    public int treeNumOfBranch;     //節の本数

    GameObject lastBranch;
    bool treeGrowth;         //成長中パラメータ
    int treeState;           //状態遷移
    int treeNumOfHierarchy;  //階層構造を示す
    int treeNumOfSibling;    //兄弟の数を示す

    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        //Tree
        treeDivition = 6;
        treeEnegy = 10.0f;
        treeDecrease = 10.0f;
        treeMinEnegy = 10.0f;
        //Branch
        branchHeight = 0.5f;

        treeGrowth = true;
        treeState = 0;
        treeNumOfBranch = 0;
        treeNumOfHierarchy = 0;
        treeNumOfSibling = 0;
        lastBranch = this.gameObject;

        

    }
	
    /*=======================================================================*/
	// Update is called once per frame
	void Update () {
        treeEnegy += 0.2f;

        if (treeNumOfBranch == 0)
        {
            /*------------------------------------------------ インスタンスの生成 */
            GameObject Branch = Instantiate(PrismPrefab) as GameObject;
            //名前
            Branch.name = treeNumOfHierarchy + "-" + treeNumOfSibling;
            // 親子構造
            Branch.transform.parent = this.gameObject.transform;
            //Transform
            Branch.transform.localPosition = lastBranch.transform.up * 0;
            // set Prism
            Branch.GetComponent<PrismController>().getPrismData(this.name, ref lastBranch);
            //節のカウント更新
            treeNumOfBranch++;
            treeNumOfHierarchy++;
            treeNumOfSibling = 0;
            lastBranch = Branch;
        }
        else
        {
            if (!lastBranch.GetComponent<PrismController>().onGrowth)
            {
                /*------------------------------------------------ インスタンスの生成 */
                GameObject Branch = Instantiate(PrismPrefab) as GameObject;
                //名前
                Branch.name = treeNumOfHierarchy + "-" + treeNumOfSibling;
                // 親子構造
                Branch.transform.parent = lastBranch.transform;
                //Transform
                Branch.transform.localPosition = Vector3.up * lastBranch.GetComponent<CreatePrismMesh>().Height;
                Branch.transform.localRotation = Quaternion.Euler(0, 0, 0);
                // set Prism
                Branch.GetComponent<PrismController>().getPrismData(this.name, ref lastBranch);
                //節のカウント更新
                treeNumOfBranch++;
                treeNumOfHierarchy++;
                treeNumOfSibling = 0;

                lastBranch = Branch;
            }
        }

    }
}
