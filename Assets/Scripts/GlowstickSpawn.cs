using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlowstickSpawn : MonoBehaviour {

    public List<GameObject> glowsticks;

    public float tossForce;

    public int glowstickLimit;

    int glowstickCount;

	// Use this for initialization
	void Start ()
    {
        glowstickCount = glowstickLimit;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(glowstickCount > 0)
            {
                //glowsticks[0].transform.rotation = transform.rotation;
                glowsticks[0].transform.parent = null;
                glowsticks[0].SetActive(true);
                glowsticks[0].GetComponent<Rigidbody>().AddForce(transform.forward * tossForce);
                glowsticks.RemoveAt(0);
                glowstickCount--;
            }            
        }
    }
}