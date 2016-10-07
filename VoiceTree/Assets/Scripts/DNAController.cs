using UnityEngine;
using System.Collections;

public class DNAController : MonoBehaviour {

    public float vol;
    public float height;
    public string character;

    GameObject seedPrefab;
    Vector3 axis;
    float spd;

    /*------------------------------------------------------------- setParam */
    /* DNAに必要なパラメータを取得
     */
    public void setParam(Vector3 pos, Color color, 
        float _vol, float _height, string _character)
    {
        //setParam
        vol = _vol;
        height = _height;
        character = _character;

        this.transform.position = pos;
        this.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    /*=======================================================================*/
	// Use this for initialization
	void Start () {
        this.seedPrefab = GameObject.Find("SeedPrefab");
        this.transform.parent = this.seedPrefab.transform;
        this.axis = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
        this.spd = Random.Range(45.0f, 720.0f);

    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
        this.transform.RotateAround(this.seedPrefab.transform.position, this.axis, this.spd * Time.deltaTime);
    }
}
