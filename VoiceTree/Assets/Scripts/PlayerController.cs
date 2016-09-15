using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject R_Controller;
    public GameObject L_Controller;
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
    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        this.Megaphone = GameObject.Find("Megaphone");
	}
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {

	}
}
