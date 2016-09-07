using UnityEngine;
using System.Collections;

public class kotodamaController : MonoBehaviour {
    /*----------------------------------------------------- setKotodamaParam */
    /*◆パラメータセット
     */
    public void setKotodamaParam(
        string character,
        Vector3 position,
        Quaternion rotation,
        Vector3 localscale
        )
    {
        this.GetComponent<TextMesh>().text = character;
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = localscale;
    }

    /*--------------------------------------------------------- jumpKotodama */
    /*◆飛んでいく
     */
    public void jumpKotodama(
        float force,//飛ばす力
        Vector3 position,
        Quaternion rotation,
        Vector3 localscale
        )
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = localscale;
        //this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    // Use this for initialization
    void Start () {        
    }

    // Update is called once per frame
    void Update () {

        //消滅条件
        if(this.transform.position.y < -1)
        {
            Destroy(gameObject);
        }
	}
}
