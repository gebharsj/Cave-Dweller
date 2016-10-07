using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour
{
    public Light playerLight;
    public float angle;
    RaycastHit hit;

    public GameObject enemy;

	// Use this for initialization
	void Start ()
    {
        //playerLight = GetComponentInChildren<Light>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, playerLight.range))
        {
            if (hit.transform.tag == "Enemy")
            {
                if (Vector3.Angle(transform.position, hit.transform.position) < angle && Vector3.Distance(transform.position, hit.transform.position) < playerLight.range)
                {
                    print("hit");
                    enemy.GetComponent<MonsterAI>().currentBehavior = MonsterAI.enemyBehavior.runAway;
                }
            }
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * playerLight.range, Color.red);
	}
}
