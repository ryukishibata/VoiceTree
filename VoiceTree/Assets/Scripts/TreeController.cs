using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour
{

    public GameObject PrismPrefab;

    bool treeGrowth = true;
    int treeState;
    int NumOfTrunk;

    GameObject lastBranch;
    float BranchHeight;
    float delta;
    float deltaGrowthSpeed;

    //樹木の生成に必要なパラメータ
    int divCircle;
    float radius;        //下円の半径
    float radiusDecRate; //下円に対する上円の減少率
    float radiusMinTrunk;//幹の最小半径
    int NumOfBranch;     //枝の本数

    /*------------------------------------------------------------- setParam */
    /* 樹木に必要なパラメータを取得する
     * 1. int _divCircle        : 円の分割数
     * 2. float _radius         : 下円の半径
     * 3. float _radiusDecRate  : 下円に対する上円の半径の比率
     * 4. float _radiusMinTrunk : 幹の最小半径
     * 5. int _NumOfBranch      : 一節における枝の本数
     * 6. 
     */
    public void setParam(
        int _divCircle,
        float _radius,
        float _radiusDecRate,
        float _radiusMinTrunk,
        int _NumOfBranch
        )
    {
        divCircle = _divCircle;
        radius = _radius;
        radiusDecRate = _radiusDecRate;
        radiusMinTrunk = _radiusMinTrunk;
        NumOfBranch = _NumOfBranch;
    }

    /*------------------------------------------------------------ AddBranch */
    /* 枝を追加する
     * 1. string branchName  : ノード名
     * 2. GameObject parent  : 親となるゲームオブジェクト
     */
    void AddBranch(string branchName, GameObject parent)
    {
        //分岐の本数を設定する
        int num = NumOfBranch;//ランダム

        for (int i = 0; i <= num; i++)
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
            //成長の限界点チェック
            if (lastBranch.GetComponent<CreatePrismMesh>().TopRadius <= radiusMinTrunk)
            {
                treeGrowth = false;
            }
        }

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
        treeGrowth = true;

        delta = 0;

        treeState = 0;
        NumOfTrunk = 0;
        BranchHeight = 1.0f;

        lastBranch = this.gameObject;

        //樹木に必要なパラメータの計算


        //プリズムの設定
        PrismPrefab.GetComponent<PrismController>().setPrism(
            0.0f,
            radius * radiusDecRate,
            radius,
            divCircle
            );
        //枝の追加
        AddBranch("Trunk" + NumOfTrunk, lastBranch);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (treeGrowth)
        {
            //ノード分岐条件
            if (!lastBranch.GetComponent<PrismController>().onGrowth)
            {
                //プリズムの設定
                PrismPrefab.GetComponent<PrismController>().setPrism(
                    0.0f,
                    lastBranch.GetComponent<CreatePrismMesh>().TopRadius * radiusDecRate,
                    lastBranch.GetComponent<CreatePrismMesh>().TopRadius,
                    divCircle
                    );
                //枝の追加
                AddBranch("Trunk" + NumOfTrunk, lastBranch);
            }
        }


    }
}
