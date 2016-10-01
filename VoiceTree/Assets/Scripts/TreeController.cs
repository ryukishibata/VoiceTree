using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour
{

    public GameObject PrismPrefab;

    GameObject lastBranch;
    int treeState;
    int NumOfTrunk;

    float BranchHeight;
    float delta;
    float deltaGrowthSpeed;


    public void setParam()
    {

    }

    /*------------------------------------------------------------ AddBranch */
    /* 枝を追加する
     * 1. string branchName  : ノード名
     * 2. GameObject parent  : 親となるゲームオブジェクト
     */
    void AddBranch(string branchName, GameObject parent)
    {
        /*------------------------------------------------ インスタンスの生成 */
        GameObject Branch = Instantiate(PrismPrefab) as GameObject;

        /*------------------------------------------------------------ Aplly */
        Branch.name = branchName;
        Branch.transform.parent = parent.transform;

        //枝の追加位置
        if (NumOfTrunk == 0)//最初の幹
        {
            Branch.transform.localPosition = parent.transform.up * 0;
        }
        else//それ以外の幹
        {
            Branch.transform.localPosition = Vector3.up * parent.GetComponent<CreatePrismMesh>().Height;
            Branch.transform.localRotation = Quaternion.Euler(0, 0, 10);
        }
        Branch.GetComponent<CreatePrismMesh>().drawPrism();

        //本数の更新
        NumOfTrunk++;
        //現在成長中のノードを取得
        lastBranch = lastBranch.transform.FindChild("Trunk" + (NumOfTrunk - 1)).gameObject;

    }
    /*--------------------------------------------------------- UpdateBranch */
    /* ノードの更新
     * 1. GameObject Branch : ゲームオブジェクト
     */
    void UpdateBranch(GameObject Branch)
    {

    }

    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        delta = 0;

        treeState = 0;
        NumOfTrunk = 0;
        BranchHeight = 1.0f;

        lastBranch = this.gameObject;

        //樹木に必要なパラメータの計算


        //プリズムの設定
        PrismPrefab.GetComponent<PrismController>().setPrism(
            0.0f,
            0.1f,
            0.1f,
            4
            );
        //枝の追加
        AddBranch("Trunk" + NumOfTrunk, lastBranch);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        //ノード分岐条件
        if (!lastBranch.GetComponent<PrismController>().onGrowth)
        {
            //プリズムの設定
            PrismPrefab.GetComponent<PrismController>().setPrism(
                0.0f,
                0.1f,
                0.1f,
                4
                );
            //枝の追加
            AddBranch("Trunk" + NumOfTrunk, lastBranch);

        }


    }
}
