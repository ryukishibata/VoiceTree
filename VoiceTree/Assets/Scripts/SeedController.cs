using UnityEngine;
using System.Collections;

public class SeedController : MonoBehaviour {
    int DNACnt;
    bool onHitDNAFlag;
    float seedCol;

    public void onHitDNA()
    {
        DNACnt++;
        onHitDNAFlag = true;
    }

    // Use this for initialization
    void Start () {
        this.DNACnt = 0;
        this.onHitDNAFlag = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.onHitDNAFlag)
        {
            
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color( 1.0f, 1.0f, 1.0f, 0.2f ));
            this.onHitDNAFlag = false;
        }
        else
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", new Color( 1.0f, 1.0f, 1.0f, 0.7f ));
        }

    }
}
