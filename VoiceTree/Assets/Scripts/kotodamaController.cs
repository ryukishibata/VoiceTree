using UnityEngine;
using System.Collections;

public class kotodamaController : MonoBehaviour {
    public GameObject kotodamaParticlePrefab;
    GameObject kotodamaGenerator;


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
        this.kotodamaGenerator = GameObject.Find("kotodamaGenerator");
    }

    // Update is called once per frame
    void Update () {

        //消滅条件
        if (this.transform.position.y <= 0)
        {
            //Particle
            GameObject kotodamaParticle = Instantiate(kotodamaParticlePrefab) as GameObject;
            kotodamaParticle.GetComponent<kotodamaParticlePrefabController>().playParticle(
                this.transform.position);

            //SE
            this.kotodamaGenerator.GetComponent<kotodamaGenerator>().playKotodamaSE(
                this.transform.position);
            
            Destroy(gameObject);


        }
	}
}
