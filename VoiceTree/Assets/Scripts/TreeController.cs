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

    //Tree
    float scaleSize;

    //樹木の生成に必要なパラメータ
    int divCircle;
    float radius;        //下円の半径
    float radiusDecRate; //下円に対する上円の減少率
    float radiusMinTrunk;//幹の最小半径
    int NumOfBranch;     //枝の本数
    int NumOfHierarchy;  //樹木における階層値
    int NumOfSibling;    //兄弟の数

    /*------------------------------------------------------------- FindDeep */
    /* 1. string name : 検索対象となるGameObject名を指定
     */
    GameObject FindDeep(string name, bool includeInactive = false)
    {
        var children = this.gameObject.GetComponentsInChildren<Transform>(includeInactive);
        foreach (var transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

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
        NumOfHierarchy = 0;
        NumOfSibling = 0;
        BranchHeight = 1.0f;

        lastBranch = this.gameObject;

        scaleSize = 1.0f;

        //樹木の計算
        this.transform.localScale = new Vector3(scaleSize, 1.0f, scaleSize);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        scaleSize += 0.01f;
        //樹木の計算
        this.transform.localScale = new Vector3(scaleSize, 1.0f, scaleSize);

        delta += Time.deltaTime;

        if (treeGrowth)
        {
            if(NumOfTrunk == 0)
            {
                /*------------------------------------------------ set Prism */
                //PrismPrefab.GetComponent<PrismController>().setPrism(
                //    0.0f,
                //    (radius * radiusDecRate),
                //    radius,
                //    divCircle
                //    );
                /*--------------------------------------- インスタンスの生成 */
                GameObject Branch = Instantiate(PrismPrefab) as GameObject;

                /*---------------------------------------------------- Aplly */
                //Name
                Branch.name = NumOfHierarchy + "-" + NumOfSibling;
                //Parent
                Branch.transform.parent = this.gameObject.transform;
                //Transform
                Branch.transform.localPosition = lastBranch.transform.up * 0;
                //drawPrism
                Branch.GetComponent<CreatePrismMesh>().drawPrism();

                /*---------------------------------------- Update Tree Param */
                NumOfTrunk++;
                NumOfHierarchy++;
                NumOfSibling = 1;
                lastBranch = Branch;
            }
            //ノード分岐条件
            else
            {
                //if (!lastBranch.GetComponent<PrismController>().onGrowth)
                //{
                    int sibling = 0;

                    for (int i = 0; i < NumOfSibling; i++)
                    {
                        lastBranch = FindDeep((NumOfHierarchy - 1) + "-" + i);

                        float[] topRad = new float[2];

                        topRad[0] = lastBranch.GetComponent<CreatePrismMesh>().TopRadius;
                        topRad[1] = lastBranch.GetComponent<CreatePrismMesh>().TopRadius * Random.Range(6, 9) * 0.1f;

                        for (int j = 0; j < 2; j++)
                        {
                            if (lastBranch.GetComponent<CreatePrismMesh>().TopRadius > radiusMinTrunk) {
                                /*------------------------------------------------ set Prism */
                                //PrismPrefab.GetComponent<PrismController>().setPrism(
                                //    0.0f,
                                //    (topRad[j] * radiusDecRate) * scaleSize,
                                //    topRad[j] * scaleSize,
                                //    divCircle
                                //    );
                                /*------------------------------------------------ インスタンスの生成 */
                                GameObject Branch = Instantiate(PrismPrefab) as GameObject;

                                /*------------------------------------------------------------ Aplly */
                                Branch.name = NumOfHierarchy + "-" + sibling;
                                Branch.transform.parent = lastBranch.transform;


                                if ((sibling % 2) == 0)
                                {
                                    Branch.transform.localPosition = Vector3.up * lastBranch.GetComponent<CreatePrismMesh>().Height;
                                    Branch.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                    Branch.GetComponent<CreatePrismMesh>().drawPrism();
                                }
                                else
                                {
                                    Branch.transform.localPosition = Vector3.up * lastBranch.GetComponent<CreatePrismMesh>().Height;
                                    Branch.transform.localRotation = Quaternion.Euler(
                                        Random.Range(30, 60),
                                        Random.Range(0, 360),
                                        0
                                        );
                                    Branch.GetComponent<CreatePrismMesh>().drawPrism();
                                }
                                sibling++;
                            }
                        }//[for] : ひとつのBranchに何本追加するか
                    }//[for] : 階層に該当する枝の更新

                    lastBranch = FindDeep(NumOfHierarchy + "-" + 0);
                    if(lastBranch.GetComponent<CreatePrismMesh>().TopRadius > radiusMinTrunk){
                        NumOfSibling = sibling;
                        NumOfHierarchy++;
                    //}
                    //else{
                    //    treeGrowth = false;
                    //}
                    
                }//[if] : Branchが分岐点に達したら
            }//[else] : 最初の一本目以外だったら
        }//[if] : 樹木が成長中だったら

    }
}
