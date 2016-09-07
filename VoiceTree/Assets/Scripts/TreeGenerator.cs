using UnityEngine;
using System.Collections;

public class TreeGenerator : MonoBehaviour {
    public GameObject TreePrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject Tree = Instantiate(TreePrefab) as GameObject;
        }
	}
}
