using UnityEngine;
using System.Collections;

public class TreeController2 : MonoBehaviour {

    //プリズムPrefab
    public GameObject PrismPrefab;
    public int treeDivition;
    public float treeDecrease;//1m時のエネルギー消費率
    public float treeMinEnegy;//節をつくるのに必要な最低エネルギー量
    public float branchHeight;

    bool treeGrowth;//成長中パラメータ
    int treeState;
    float treeEnegy;   //エネルギーの総量

    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        //Tree
        treeDivition = 6;
        treeEnegy = 100.0f;
        treeDecrease = 2.0f;
        treeMinEnegy = 10.0f;
        //Branch
        branchHeight = 0.5f;

        treeGrowth = true;
        treeState = 0;


        /*------------------------------------------------ インスタンスの生成 */
        GameObject Branch = Instantiate(PrismPrefab) as GameObject;
        // set Prism
        Branch.GetComponent<PrismController>().getPrismData( this.name, treeEnegy );
        // 親子構造
        Branch.transform.parent = this.gameObject.transform;



    }
	
    /*=======================================================================*/
	// Update is called once per frame
	void Update () {
	
	}
}
