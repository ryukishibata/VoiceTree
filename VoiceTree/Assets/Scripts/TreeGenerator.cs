using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour {


    public GameObject TreePrefab;


    public void GenerateTree(Vector3 pos) {
        GameObject Tree = Instantiate(TreePrefab) as GameObject;
        //Aplly
        Tree.transform.position = pos;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	}
}
