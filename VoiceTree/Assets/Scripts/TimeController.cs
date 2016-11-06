using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {
    public int seasonState;
    public float seasonTime;

    float deltaTime;

	// Use this for initialization
	void Start () {
        deltaTime = 0;
        seasonState = 0;
        seasonTime = 2.0f;//一年は10秒
    }
	
	// Update is called once per frame
	void Update() {
        deltaTime += Time.deltaTime;
        seasonState = (int)(deltaTime / seasonTime) % 4;
	}
}
