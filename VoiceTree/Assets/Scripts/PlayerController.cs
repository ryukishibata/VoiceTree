using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject R_Controller;
    public GameObject L_Controller;
    GameObject GameDirector;
    GameObject Megaphone;

    /*--------------------------------------------------------------- pickUp */
    /* 手でものを掴む
     */
    public void pickUp(GameObject target, GameObject hand)
    {
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.parent = hand.transform;
    }
    /*--------------------------------------------------------------- pickUp */
    /* ものを手放す
     */
    public void release(GameObject target, GameObject hand)
    {
        target.GetComponent<Rigidbody>().isKinematic = false;
        target.transform.parent = null;
    }

    /*--------------------------------------------------------- keyboardMove */
    /* キーボードで移動
     */
    void keyboardMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += this.transform.forward * 0.01f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += this.transform.forward * -0.01f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0,-5.0f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0, 5.0f, 0);
        }
    }

    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        this.Megaphone = GameObject.Find("Megaphone");
        this.GameDirector = GameObject.Find("GameDirector");
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
        if (!this.GameDirector.GetComponent<GameDirector>().VRMode)
        {
            keyboardMove();
        }
	}
}
