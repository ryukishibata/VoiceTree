using UnityEngine;
using System.Collections;

public class Test_reflectVec : MonoBehaviour {
    GameObject seedPrefab;
    Vector3 force;

    // Use this for initialization
    void Start () {
        seedPrefab = GameObject.Find("Sphere");
        force = new Vector3(0.01f, 0.003f, 0.005f);
    }
	// Update is called once per frame
	void Update () {
        this.transform.position += force;

        float dist = (this.seedPrefab.transform.position - this.transform.position).magnitude;
        if (dist >= this.seedPrefab.GetComponent<SphereCollider>().radius)
        {
            Vector3 inNormal = (this.seedPrefab.transform.position - this.transform.position).normalized;
            force = Vector3.Reflect(this.transform.position, inNormal) * 0.01f;
            //force = -force;
            this.transform.position += force;
        }
    }
}
