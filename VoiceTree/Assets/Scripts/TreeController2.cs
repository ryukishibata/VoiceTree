using UnityEngine;
using System.Collections;

public class TreeController2 : MonoBehaviour
{

    //プリズムPrefab
    public GameObject PrismPrefab;
    public int treeDivition;    //分割数
    public int treeNumOfBranch; //節の本数
    public float treeEnegy;     //エネルギーの総量
    public float treeHeight;    //高さ
    public float treeGrowUp;    //高さ方向の成長係数
    public float treeRadSpd;    //幅方向の成長係数
    public float treeDecrease;  //1m成長するのに必要なエネルギー減少率
    public int branchDiv;       //1節における最大分割数
    public float branchHeight;  //1節の高さの最大値

    GameObject lastBranch;
    int treeState;           //状態遷移
    float treeBranchEnegy;   //節をつくるのに必要な最低エネルギー量
    int treeNumOfHierarchy;  //階層構造を示す
    int treeNumOfSibling;    //兄弟の数を示す


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
    /*----------------------------------------------------------- treeGrowing*/
    /* 樹木の成長中関数
     */
    void treeGrowing()
    {
        if (treeNumOfBranch == 0)
        {
            /*------------------------------------------- インスタンスの生成 */
            GameObject Branch = Instantiate(PrismPrefab) as GameObject;
            //名前
            Branch.name = treeNumOfHierarchy + "-" + treeNumOfSibling;
            // 親子構造
            Branch.transform.parent = this.gameObject.transform;
            //Transform
            Branch.transform.localPosition = lastBranch.transform.up * 0;
            // set Prism
            Branch.GetComponent<PrismController>().getPrismData(this.name, ref lastBranch, 0);
            //節のカウント更新
            treeNumOfBranch++;
            treeNumOfHierarchy++;
            treeNumOfSibling = 1;
            lastBranch = Branch;
        }
        else
        {
            /*-------------------------------------------- ノード(枝)の追加 */
            if (!lastBranch.GetComponent<PrismController>().onGrowth)
            {
                int sibling = 0;

                /*--------------------------------- 格節からノードを生成する */
                for (int i = 0; i < treeNumOfSibling; i++)
                {
                    lastBranch = FindDeep((treeNumOfHierarchy - 1) + "-" + i);

                    /*------------------------------------- 何本枝分岐するか */
                    int div = lastBranch.GetComponent<PrismController>().ChildrenDivRad.Length;
                    Debug.Log(lastBranch.name + " : " + div);

                    if ((lastBranch.GetComponent<CreatePrismMesh>().TopRadius *
                        lastBranch.GetComponent<PrismController>().ChildrenDivRad[0]) > 0.01f)
                    {
                        for (int k = 0; k < div; k++)
                        {
                            /*----------------------------------- インスタンスの生成 */
                            GameObject Branch = Instantiate(PrismPrefab) as GameObject;
                            //名前
                            Branch.name = treeNumOfHierarchy + "-" + sibling;
                            // 親子構造
                            Branch.transform.parent = lastBranch.transform;

                            //Transform
                            if ((k % 2) == 0)
                            {
                                Branch.transform.localPosition = Vector3.up * lastBranch.GetComponent<CreatePrismMesh>().Height;
                                Branch.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            }
                            else
                            {
                                Branch.transform.localPosition = Vector3.up * lastBranch.GetComponent<CreatePrismMesh>().Height;
                                Branch.transform.localRotation = Quaternion.Euler(
                                    Random.Range(30, 60),
                                    Random.Range(0, 360),
                                    0
                                    );
                            }
                            // set Prism
                            Branch.GetComponent<PrismController>().getPrismData(this.name, ref lastBranch, k);

                            //カウンター更新
                            sibling++;         //兄弟の数
                            treeNumOfBranch++; //樹木全体の節の数
                        }//[for]:k
                    }
                    else
                    {
                        Debug.Log("worning");
                    }

                }//[for]:i

                /*----------------------------------------- 階層単位での更新 */
                lastBranch = FindDeep(treeNumOfHierarchy + "-" + 0);
                treeNumOfSibling = sibling;
                treeNumOfHierarchy++;
            }//[if]
        }

        /*--------------------------------------------------- 成長係数の計算 */
        // 幅係数
        if (this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius < 1.0f)
        {
            treeRadSpd = 0.0005f;
        }
        else
        {
            treeRadSpd = 0;
            treeState = 1;
        }
        //高さ係数
        float height_k = this.transform.FindChild("0-0").GetComponent<CreatePrismMesh>().BottomRadius;
        if ((treeGrowUp -= (0.01f * height_k / 400)) < 0)
        {
            treeGrowUp = 0;
        }

        return;
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        //Tree
        treeDivition = 6;
        treeEnegy = 100.0f;
        treeHeight = 0;
        treeGrowUp = 0.01f;
        treeRadSpd = 0.0005f;
        treeDecrease = 0.95f;
        treeBranchEnegy = 0.1f;

        //Branch
        branchDiv = 2;
        branchHeight = 1.0f;

        treeState = 0;
        treeNumOfBranch = 0;
        treeNumOfHierarchy = 0;
        treeNumOfSibling = 0;
        lastBranch = this.gameObject;



    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        //treeEnegy += 0.2f;

        switch (treeState)
        {
            case 0://成長状態
                treeGrowing();
                break;
            case 1:
                break;
            case 3:
                break;
            default:
                break;

        }


    }
}
