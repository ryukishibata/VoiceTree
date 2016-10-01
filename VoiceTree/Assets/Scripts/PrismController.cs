using UnityEngine;
using System.Collections;

public class PrismController : MonoBehaviour {

    //UIEditor側で必ずチェックを入れておくこと
    public bool onGrowth;//成長中フラグ

    /*------------------------------------------------------------- setPrism */
    /* プリズムの設定
     */
    public void setPrism(
        float height, float topRadius, float bottomRadius, int divition )
    {
        GetComponent<CreatePrismMesh>().Height = height;
        GetComponent<CreatePrismMesh>().TopRadius = topRadius;
        GetComponent<CreatePrismMesh>().BottomRadius = bottomRadius;
        GetComponent<CreatePrismMesh>().Divition = divition;

    }
    /*=======================================================================*/
	// Use this for initialization
	void Start () {
        onGrowth = true;
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
        //ノードの更新
        if (onGrowth)
        {
            GetComponent<CreatePrismMesh>().Height += 0.01f;
            GetComponent<CreatePrismMesh>().drawPrism();

            if(GetComponent<CreatePrismMesh>().Height > 0.5f)
            {
                GetComponent<CreatePrismMesh>().Height = 0.5f;
                onGrowth = false;
            }
        }
    }
}
