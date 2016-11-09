using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeafController : MonoBehaviour {
    public Camera targetCamera;
    public GameObject treePrefab;

    //Transform
    float extent;      //カメラベクトルに対して葉をばらけさせる
    float[] scale;     //[0:最低値][1:最大値]
    
    //Time
    float deltaTime;
    float seasonTime;
    float tmpTime;
    float dropTime;

    //Other
    float treeLine;   //樹木のエネルギー変化量を(0.0 - 1.0)までで正規化したもの


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
    /*=======================================================================*
     * ◆RectTransform 関数群
     * 　葉っぱの移動・回転の更新を行う　
     *=======================================================================*/

    /*-------------------------------------------------  updateTransform *
     * Transformの更新
     *-----------------------------------------------------------------------*/
    void updateTransform()
    {
        /*--------------------------------- 親ノード(枝)の成長における状態遷移 */
        if (this.transform.parent != null)
        {
            //ノードが生長していたら
            if (this.transform.parent.GetComponent<PrismController>().GrowUpState < 4)
            {

                //translate
                this.transform.position = this.transform.parent.transform.position
                      + this.transform.parent.transform.up
                      * this.transform.parent.GetComponent<CreatePrismMesh>().Height
                      + this.targetCamera.transform.up
                      * this.extent
                      * this.treeLine;
                //scale
                var size = this.transform.lossyScale;
                size.x = size.y = size.z =
                      this.scale[0]
                    + this.scale[1]
                    * this.treeLine;


                //billboard
                this.transform.LookAt(this.targetCamera.transform.position);


                this.transform.localScale = size;
            }
            //季節が冬になったら
            if (GameObject.Find("GameDirector").GetComponent<TimeController>().seasonState == 3)
            {
                this.tmpTime += Time.deltaTime;
                if (this.tmpTime >= this.dropTime)
                {
                    this.tmpTime = 0;
                    setLeafParent(null);
                    //this.gameObject.AddComponent<Rigidbody>();
                    this.GetComponent<Rigidbody>().isKinematic = false;
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
            if (Mathf.Sqrt(rot.x * rot.x + rot.y * rot.y + rot.z * rot.z) > 1.3f)
            {
                this.GetComponent<Rigidbody>().AddForce(
                    rot.x * 0.2f,
                    rot.y * 0.2f,
                    rot.z * 0.2f,
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
        this.seasonTime = GameObject.Find("GameDirector").GetComponent<TimeController>().seasonTime;
        this.dropTime = Random.Range(0, seasonTime);
        this.tmpTime = 0;

        //Camera
        if (this.targetCamera == null) targetCamera = Camera.main;
        //Prefab
        this.treePrefab = this.transform.root.gameObject;

        //Transform
        this.extent = Random.Range(-0.1f, 0.1f);
        //scale
        this.scale = new float[2];
        this.scale[0] = 0.5f;
        this.scale[1] = 0;// Random.Range(0.3f, 0.5f);
        this.transform.localScale = new Vector3(0, 0, 0);

        //Other
        this.treeLine = this.treePrefab.GetComponent<TreeController2>().NPKEnergy.x
                         / this.treePrefab.GetComponent<TreeController2>().NPKEnergyMax.x;

        return;
    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update ()
    {
        this.deltaTime += Time.deltaTime;
        this.treeLine = this.treePrefab.GetComponent<TreeController2>().NPKEnergy.x
                         / this.treePrefab.GetComponent<TreeController2>().NPKEnergyMax.x;

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
