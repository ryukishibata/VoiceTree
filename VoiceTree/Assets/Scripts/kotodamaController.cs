using UnityEngine;
using System.Collections;

public class kotodamaController : MonoBehaviour {

    public GameObject kotodamaParticlePrefab;
    GameObject kotodamaGenerator;
    GameObject seedPrefab;
    //Transform
    Vector3 speedInSeed;
    //TextMesh
    float tm_charaSize;
    const float tm_charSizeMin = 0.2f;
    //Other
    bool inSeed;


    /*----------------------------------------------------- setKotodamaParam */
    /*◆パラメータセット
     */
    public void setKotodamaParam( string character, Color color, float charaSize,
        Vector3 position, Quaternion rotation )
    {

        
        /*-------------------------------------------------- メンバ変数に格納 */
        //TextMesh
        tm_charaSize = charaSize;

        /*------------------------------------------------------------- 適用 */
        //Transform
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //TextMesh
        this.GetComponent<TextMesh>().text = character;
        this.GetComponent<TextMesh>().color = color;
        this.GetComponent<TextMesh>().fontSize = 64;
        this.GetComponent<TextMesh>().characterSize = tm_charaSize;
    }

    /*--------------------------------------------------------- jumpKotodama */
    /*◆飛んでいく
     */
    public void jumpKotodama( float force, float charaSize,
        Vector3 position, Quaternion rotation )
    {
        /*-------------------------------------------------- メンバ変数に格納 */
        //TextMesh
        tm_charaSize = charaSize;

        /*------------------------------------------------------------- 適用 */
        //Transform
        this.transform.position = position;
        this.transform.rotation = rotation;
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //TextMesh
        //this.GetComponent<TextMesh>().color = color;
        this.GetComponent<TextMesh>().characterSize = charaSize;
        this.GetComponent<TextMesh>().fontSize = 64;
        //RigidBody
        //this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start () {
        this.kotodamaGenerator = GameObject.Find("kotodamaGenerator");
        this.seedPrefab = GameObject.Find("SeedPrefab");

        this.inSeed = false;
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update () {
        //樹の種の影響範囲
        float seedRange = 2.0f;
        //樹の種の半径
        float seedRad = this.seedPrefab.GetComponent<SphereCollider>().radius;
        //樹の種までの距離
        float dist = (this.seedPrefab.transform.position - this.transform.position).magnitude;

        //樹の種に当たったかどうか
        if(dist < seedRad)
        {
            this.inSeed = true;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.speedInSeed = this.transform.forward * 0.01f;
        }


        if (this.inSeed)//種の中にいる
        {
            //消滅
            Destroy(gameObject);
        }
        else//種の外にいる
        {
            //if (dist > seedRad && dist < seedRange)
            //{
            //    float sizeOffset;
            //    this.GetComponent<TextMesh>().color = new Color(1.0f, 1.0f, 1.0f);
            //
            //    sizeOffset = dist * (1.0f - tm_charSizeMin) / (seedRange - seedRad);
            //    this.GetComponent<TextMesh>().characterSize = tm_charaSize * sizeOffset;
            //}
            /*------------------------------------------------------ 消滅条件 */
            if (this.transform.position.y <= 0)
            {
                //Particle
                GameObject kotodamaParticle = Instantiate(kotodamaParticlePrefab) as GameObject;
                kotodamaParticle.GetComponent<kotodamaParticlePrefabController>().playParticle(
                    this.transform.position);

                //SE
                this.kotodamaGenerator.GetComponent<kotodamaGenerator>().playKotodamaSE(
                    this.transform.position);
                
                //消滅
                Destroy(gameObject);
            }
        }//[else]:inSeed

	}//[update]
}
