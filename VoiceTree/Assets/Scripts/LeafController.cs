using UnityEngine;
using System.Collections;

public class LeafController : MonoBehaviour {
    public Camera targetCamera;
    public string treeName;

    /*--------------------------------------------------------- setLeafParam *
     * ◆葉っぱのパラメータを設定する
     *-----------------------------------------------------------------------*/
    public void updateLeafParam(float length)
    {
        return;
    }

    /*-------------------------------------------------------- setLeafParent *
     * ◆親子構造を設定する
     *-----------------------------------------------------------------------*/
    public void setLeafParent(Transform Parent)
    {
        this.transform.parent = Parent;
        return;
    }
    /*----------------------------------------------------------- updateLeaf *
     * ◆葉っぱの更新
     *-----------------------------------------------------------------------*/
    void updateLeaf()
    {
        if (this.transform.parent.GetComponent<PrismController>().GrowUpState < 3)
        {
            //translate
            this.transform.position = this.transform.parent.transform.position
                  + this.transform.parent.transform.up
                  * this.transform.parent.GetComponent<CreatePrismMesh>().Height;
        }
        return;
    }
    /*=======================================================================*/
    // Use this for initialization
    /*=======================================================================*/
    void Start () {
        //カメラ
        if (this.targetCamera == null) targetCamera = Camera.main;
    }
    /*=======================================================================*/
    // Update is called once per frame
    /*=======================================================================*/
    void Update () {

        switch (GameObject.Find(this.treeName).GetComponent<TreeController2>().treeState)
        {
            case 0:
                updateLeaf();
                break;
            case 1:
                updateLeaf();
                break;
            case 2:
                setLeafParent(null);
                break;
            default:
                break;
        }
        
            
        //ビルボード
        this.transform.LookAt(this.targetCamera.transform.position);
        return;
    }
}
