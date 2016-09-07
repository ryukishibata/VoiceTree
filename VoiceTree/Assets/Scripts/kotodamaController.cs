using UnityEngine;
using System.Collections;

public class kotodamaController : MonoBehaviour {
    /*----------------------------------------------------- setKotodamaParam */
    /*◆パラメータセット
     */
    public void setKotodamaParam(
        string character,
        float charaSize,
        Vector3 position,
        Quaternion rotation
        )
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        this.GetComponent<TextMesh>().text = character;
        this.GetComponent<TextMesh>().fontSize = 64;
        this.GetComponent<TextMesh>().characterSize = charaSize;
    }

    /*--------------------------------------------------------- jumpKotodama */
    /*◆飛んでいく
     */
    public void jumpKotodama(
        float force,//飛ばす力
        float charaSize,
        Vector3 position,
        Quaternion rotation
        )
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        this.GetComponent<TextMesh>().fontSize = 64;
        this.GetComponent<TextMesh>().characterSize = charaSize;

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
