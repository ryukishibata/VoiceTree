using UnityEngine;
using System.Collections;

public class kotodamaParticleController : MonoBehaviour {

    float delta;

    public void playParticle(Vector3 pos)
    {
        this.transform.position = pos;
        this.GetComponent<ParticleSystem>().Play();
    }
    // Use this for initialization
    void Start () {
        this.delta = 0;
	}
	
	// Update is called once per frame
	void Update () {
        this.delta += Time.deltaTime;

        if(this.delta > GetComponent<ParticleSystem>().startLifetime)
        {
            Destroy(gameObject);
        }

    }
}
