using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeafMeshController : MonoBehaviour {

    public Camera targetCamera;
    public GameObject treePrefab;

    //State
    bool meshFlag;

    //Color
    float[] leafCol = new float[3];
    float[] colTable0 = new float[3];
    float[] colTable1 = new float[3];
    float[] colTable2 = new float[3];
    float[] colTable3 = new float[3];

    //Time
    float deltaTime;

    /*=======================================================================*
     * ◆set 関数群
     * 　パラメータを設定する　
     *=======================================================================*/

    /*---------------------------------------------------------- setTextText *
     * 文字の設定
     *-----------------------------------------------------------------------*/
    void setTextText(int num)
    {
        string[] sample;

        sample = new string[] {
            "ハ","ロ","ー ", "夜。",
            "静", "か", "な", "霜", "柱。",
            "カ","ッ","プ", "ヌ", "ド", "ル","の", "海", "老", "た", "ち。"};

        this.GetComponent<Text>().text = sample[num];

        return;
    }
    /*--------------------------------------------------------- setTextColor *
     * 色の設定
     *-----------------------------------------------------------------------*/
    void setTextColor()
    {
        colTable0[0] = 0.0f + Random.Range(0, 0.1f);
        colTable0[1] = 0.6f + Random.Range(-0.1f, 0.2f);
        colTable0[2] = 0.2f + Random.Range(-0.1f, 0.1f);

        colTable1[0] = 0.0f + Random.Range(0, 0.1f);
        colTable1[1] = 0.2f + Random.Range(-0.1f, 0.1f);
        colTable1[2] = 0.0f + Random.Range(0, 0.1f);

        colTable2[0] = 0.6f + Random.Range(-0.1f, 0.1f);
        colTable2[1] = 0.1f + Random.Range(-0.1f, 0.1f);
        colTable2[2] = 0.1f + Random.Range(-0.1f, 0.1f);

        colTable3[0] = 0.4f + Random.Range(-0.1f, 0.1f);
        colTable3[1] = 0.1f + Random.Range(-0.1f, 0.1f);
        colTable3[2] = 0.1f + Random.Range(-0.1f, 0.1f);

        return;
    }


    /*=======================================================================*
     * ◆Text 関数群
     * 　葉っぱの文字・色・大きさなどの更新を行う
     *=======================================================================*/

    /*------------------------------------------------------ updateTextColor *
     * 色の更新
     *-----------------------------------------------------------------------*/
    void updateTextColor()
    {
        switch (GameObject.Find("GameDirector").GetComponent<TimeController>().seasonState)
        {
            case 0:
                leafCol = colTable0;
                break;
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    if (colTable0[i] < colTable1[i])
                    {
                        if (leafCol[i] > colTable1[i]) leafCol[i] = colTable1[i];
                        else leafCol[i] += (colTable1[i] - colTable0[i]) * Time.deltaTime;
                    }
                    else if (colTable0[i] > colTable1[i])
                    {
                        if (leafCol[i] < colTable1[i]) leafCol[i] = colTable1[i];
                        else leafCol[i] -= (colTable0[i] - colTable1[i]) * Time.deltaTime;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < 3; i++)
                {
                    if (colTable1[i] < colTable2[i])
                    {
                        if (leafCol[i] > colTable2[i]) leafCol[i] = colTable2[i];
                        else leafCol[i] += (colTable2[i] - colTable1[i]) * Time.deltaTime;
                    }
                    else if (colTable1[i] > colTable2[i])
                    {
                        if (leafCol[i] < colTable2[i]) leafCol[i] = colTable2[i];
                        else leafCol[i] -= (colTable1[i] - colTable2[i]) * Time.deltaTime;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    if (colTable2[i] < colTable3[i])
                    {
                        if (leafCol[i] > colTable3[i])
                        {
                            leafCol[i] = colTable3[i];
                            this.meshFlag = false;
                        }
                        else leafCol[i] += (colTable3[i] - colTable2[i]) * Time.deltaTime;
                    }
                    else if (colTable2[i] > colTable3[i])
                    {
                        if (leafCol[i] < colTable3[i])
                        {
                            leafCol[i] = colTable3[i];
                            this.meshFlag = false;
                        }
                        else leafCol[i] -= (colTable2[i] - colTable3[i]) * Time.deltaTime;
                    }
                }
                break;
        }

        this.GetComponent<Text>().color = new Color(leafCol[0], leafCol[1], leafCol[2], 1.0f);

        return;
    }
    /*------------------------------------------------------- updateTextMesh *
     * TextMeshの更新
     *-----------------------------------------------------------------------*/
    void updateText()
    {
        //Color
        updateTextColor();

        return;
    }
    /*=======================================================================*/
    // Use this for initialization
    /*=======================================================================*/
    void Start()
    {
        //Time
        this.deltaTime = 0;

        //Camera
        if (this.targetCamera == null) targetCamera = Camera.main;
        //Prefab
        this.treePrefab = this.transform.root.gameObject;

        //State
        this.meshFlag = true;

        //textMesh
        setTextText(Random.Range(0, 20));
        setTextColor();
        updateText();

    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update()
    {
        this.deltaTime += Time.deltaTime;

        //TextMesh
        if(this.meshFlag) updateText();


        /*------------------------------------------- 木の成長における状態遷移 */
        switch (this.treePrefab.GetComponent<TreeController2>().treeState)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }


        return;
    }
}
