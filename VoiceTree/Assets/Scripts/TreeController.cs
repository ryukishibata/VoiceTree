using UnityEngine;
using System.Collections;

public class TreeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector3(Random.Range(-5, 6), 0.0f, Random.Range(-5, 6));

        this.GetComponent<Tree>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
