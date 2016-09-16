using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    public Light playerLight;
    RaycastHit hit;

    public GameObject enemy;

	// Use this for initialization
	void Start ()
    {
        //playerLight = GetComponentInChildren<Light>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, playerLight.range))
        {
            if (hit.transform.tag == "Enemy")
            {
                print("Back the fuck up");
                enemy.GetComponent<MonsterAI>().CallCoroutine("RunAway");
            }
        }
	}
}
