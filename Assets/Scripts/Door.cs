using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public float openTime = 3;

    private bool open = false;
    public ParticleSystem m_particles;



	// Use this for initialization
	void Start ()
    {
    }

    public void Open()
    {
        Debug.Log("OPEN");
        open = true;
        ChangeDoorState(open);

        Invoke("Close", openTime);
    }

    public void Close()
    {
        open = false; ;
        ChangeDoorState(open);
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void ChangeDoorState(bool open)
    {
        gameObject.GetComponent<BoxCollider>().enabled = !open;
        m_particles.enableEmission = !open;
    }
}
