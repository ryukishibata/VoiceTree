using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*****************************************************************************
 * ◆葉っぱ全体の設定を行う
 *
 *
 *
 *****************************************************************************/

public class LeafGenerator : MonoBehaviour {
    public GameObject LeafPrefab;

    public int NumOfLeaf;

    /*--------------------------------------------------------- generateLeaf *
     * 葉っぱを生成する
     *-----------------------------------------------------------------------*/
    public void generateLeaf( GameObject ParentBranch )
    {
        /*--------------------------------------- インスタンスの生成 */
        GameObject Leaf = Instantiate(LeafPrefab) as GameObject;
        //名前
        Leaf.name = "Leaf" + this.NumOfLeaf;
        //親子構造
        Leaf.transform.parent = ParentBranch.transform;

        //Transform
        Leaf.transform.position = ParentBranch.transform.position;

        this.NumOfLeaf++;
        return;
    }

    /*=======================================================================*/
    // Use this for initialization
    /*=======================================================================*/
    void Start () {
        this.NumOfLeaf = 0;
    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update () {
	
	}
}
