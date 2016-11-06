using UnityEngine;
using System.Collections;

public class LeafController : MonoBehaviour {
    public Camera targetCamera;
    public GameObject treePrefab;

    //Color
    float[] leafCol = new float[3];
    float[] colTable0 = new float[3];
    float[] colTable1 = new float[3];
    float[] colTable2 = new float[3];
    float[] colTable3 = new float[3];
    
    //Time
    float deltaTime;
    float seasonTime;
    float dropTime;   //冬になったどのタイミングで落ちるか
    float tmpTime;

    /*=======================================================================*
     * ◆set 関数群
     * 　パラメータを設定する　
     *=======================================================================*/

    /*-------------------------------------------------------- setLeafParent *
     * 親子構造を設定する
     *-----------------------------------------------------------------------*/
    public void setLeafParent(Transform Parent)
    {
        this.transform.parent = Parent;
        return;
    }
    /*------------------------------------------------------------ setTMText *
     * 文字の設定
     *-----------------------------------------------------------------------*/
     void setTMTextMesh(int num)
    {
        string[] sample;
        
        sample = new string[] {
            "ハ","ロ","ー ", "夜。",
            "静", "か", "な", "霜", "柱。",
            "カ","ッ","プ", "ヌ", "ド", "ル","の", "海", "老", "た", "ち。"};

        this.GetComponent<TextMesh>().text = sample[num];

        return;
    }
    /*----------------------------------------------------------- setTMColor *
     * 色の設定
     *-----------------------------------------------------------------------*/
    void setTMColor()
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
     * ◆TextMesh 関数群
     * 　葉っぱの文字・色・大きさなどの更新を行う
     *=======================================================================*/

    /*-------------------------------------------------------- updateTMColor *
     * 色の更新
     *-----------------------------------------------------------------------*/
    void updateTMColor()
    {
        switch (GameObject.Find("GodPrefab").GetComponent<TimeController>().seasonState)
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
                        else leafCol[i] += 0.01f;// (colTable2[i] - colTable1[i]) * Time.deltaTime;
                    }
                    else if (colTable1[i] > colTable2[i])
                    {
                        if (leafCol[i] < colTable2[i]) leafCol[i] = colTable2[i];
                        else leafCol[i] -= 0.01f;// (colTable1[i] - colTable2[i]) * Time.deltaTime;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    if (colTable2[i] < colTable3[i])
                    {
                        if (leafCol[i] > colTable3[i]) leafCol[i] = colTable3[i];
                        else leafCol[i] += (colTable3[i] - colTable2[i]) * Time.deltaTime;
                    }
                    else if (colTable2[i] > colTable3[i])
                    {
                        if (leafCol[i] < colTable3[i]) leafCol[i] = colTable3[i];
                        else leafCol[i] -= (colTable2[i] - colTable3[i]) * Time.deltaTime;
                    }
                }
                break;
        }

        this.GetComponent<TextMesh>().color =
                    new Color(leafCol[0], leafCol[1], leafCol[2], 1.0f);

        return;
    }
    /*--------------------------------------------------------- updateTMSize *
     * 大きさの更新
     *-----------------------------------------------------------------------*/
    void updateTMSize()
    {
        if (this.transform.parent != null)
        {
            if (this.transform.parent.GetComponent<PrismController>().SiblingOfParent != 0)
            {
                //ノードが生長していたら
                if (this.transform.parent.GetComponent<PrismController>().GrowUpState < 4)
                {
                    this.GetComponent<TextMesh>().characterSize
                        = (this.treePrefab.GetComponent<TreeController2>().NPKEnergy.x
                            / this.treePrefab.GetComponent<TreeController2>().NPKEnergyMax.x);
                }
            }
        }

        return;
    }
    /*------------------------------------------------------- updateTextMesh *
     * TextMeshの更新
     *-----------------------------------------------------------------------*/
    void updateTextMesh()
    {
        //Color
        updateTMColor();

        //Size
        updateTMSize();

        return;
    }

    /*=======================================================================*
     * ◆Transform 関数群
     * 　葉っぱの移動・回転の更新を行う　
     *=======================================================================*/

    /*------------------------------------------------------ updateTransform *
     * Transformの更新
     *-----------------------------------------------------------------------*/
    void updateTransform()
    {

        /*--------------------------------- 親ノード(枝)の成長における状態遷移 */
        if (this.transform.parent != null)
        {
            if (this.transform.parent.GetComponent<PrismController>().SiblingOfParent != 0)
            {
                //ノードが生長していたら
                if (this.transform.parent.GetComponent<PrismController>().GrowUpState < 4)
                {
                    //translate
                    this.transform.position = this.transform.parent.transform.position
                          + this.transform.parent.transform.up
                          * this.transform.parent.GetComponent<CreatePrismMesh>().Height;

                    //billboard
                    this.transform.LookAt(this.targetCamera.transform.position);

                }
            }
            //季節が冬になったら
            if (GameObject.Find("GodPrefab").GetComponent<TimeController>().seasonState == 3)
            {
                this.tmpTime += Time.deltaTime;
                if (this.tmpTime >= this.dropTime)
                {
                    this.tmpTime = 0;
                    setLeafParent(null);
                    this.gameObject.AddComponent<Rigidbody>();
                    this.GetComponent<Rigidbody>().useGravity = true;
                    this.GetComponent<Rigidbody>().drag = Random.Range(5.0f, 10.0f);
                    this.GetComponent<Rigidbody>().angularDrag = Random.Range(5.0f, 10.0f);
                }
            }
        }
        else
        {
            /*----------------------------------------- 葉っぱが落下中だったら */
            //Rotate
            var rot = this.transform.localRotation;
            rot.x += Random.Range(-0.3f, 0.3f);
            rot.y += Random.Range(-0.3f, 0.3f);
            rot.z += Random.Range(-0.3f, 0.3f);
            this.transform.localRotation = rot;
            //AddForce
            if (rot.x > 0.25f || rot.y > 0.25f || rot.z > 0.25f
                || rot.x < -0.25f || rot.y < -0.25f || rot.z < -0.25f)
            {
                this.GetComponent<Rigidbody>().AddForce(
                    rot.x * 1.2f,
                    rot.y * 1.2f,
                    rot.z * 1.2f,
                    ForceMode.Impulse);
            }

            //消滅処理
            if (this.transform.position.y < -1.0f)
            {
                Destroy(this.gameObject);
            }
        }

        return;
    }

    /*=======================================================================*/
    // Use this for initialization
    /*=======================================================================*/
    void Start () {
        //Time
        this.deltaTime = 0;
        this.seasonTime = GameObject.Find("GodPrefab").GetComponent<TimeController>().seasonTime;
        this.dropTime = Random.Range(0, seasonTime);
        this.tmpTime = 0;
        //Camera
        if (this.targetCamera == null) targetCamera = Camera.main;
        //Prefab
        this.treePrefab = this.transform.root.gameObject;

        //Transform
        var size = this.transform.lossyScale;
        size.x = size.y = size.z = Random.Range(0.02f, 0.045f);
        this.transform.localScale = size;

        //textMesh
        setTMTextMesh(Random.Range(0, 20));
        setTMColor();
        updateTextMesh();

    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update ()
    {
        this.deltaTime += Time.deltaTime;



        //TextMesh
        updateTextMesh();

        //Transform
        updateTransform();

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
