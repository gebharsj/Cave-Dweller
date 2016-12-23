using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    public GameObject[] enemySpawnPoints;

    public int roamRadius;
    public int attackRadius;
    public int deAggro;
    public float patrolSpeed;
    public float chaseSpeed;

    public AudioSource source;
    public AudioClip patrolClip;
    public AudioClip chaseClip;
    public AudioClip attackClip;
    public AudioClip runAwayClip;

    Animator anim;
    GameObject player;
    GameObject flashlight;
    int enemySpawnRandom;
    bool runningCoroutine;
    bool playingClip;
    string gameOverScene, winScreen;
    Transform patrolTarget;
    ScreenTransition fadePanel;

    public enum enemyBehavior
    {
        patrol,
        chase,
        attack,
        runAway
    }

    Collider enemyLightSee;
    Collider enemyHearPlayer;

    public enemyBehavior currentBehavior;

    NavMeshAgent myNavMeshAgent;

    // Use this for initialization
    void Start ()
    {
        enemySpawnRandom = Random.Range(0, enemySpawnPoints.Length);
        gameObject.transform.position = enemySpawnPoints[enemySpawnRandom].transform.position;
        currentBehavior = enemyBehavior.patrol;
        myNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
        fadePanel = GameObject.Find("FadePanel").GetComponent<ScreenTransition>();
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {        
        switch(currentBehavior)
        {
            case enemyBehavior.patrol:
                CallCoroutine("Patrol");
                break;
            case enemyBehavior.chase:
                CallCoroutine("Chase");
                break;
            case enemyBehavior.attack:
                CallCoroutine("Attack");
                break;
            case enemyBehavior.runAway:
                CallCoroutine("RunAway");
                break;
        }

        if (!source.isPlaying)
            source.Play();
    }

    public void CallCoroutine(string name)
    {
        if (!runningCoroutine)
        {
            source.Stop();
            runningCoroutine = true;
            StopAllCoroutines();
            StartCoroutine(name);
        }
    }

    IEnumerator RunAway()
    {
        source.clip = runAwayClip;
        enemySpawnRandom = Random.Range(0, enemySpawnPoints.Length);
        gameObject.transform.position = enemySpawnPoints[enemySpawnRandom].transform.position;
        yield return new WaitForFixedUpdate();
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        currentBehavior = enemyBehavior.patrol;
        runningCoroutine = false;
    }    

    IEnumerator Attack()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);
        source.clip = attackClip;
        currentBehavior = enemyBehavior.chase;
        yield return new WaitForFixedUpdate();
        fadePanel.CallCoroutine("MainMenu");
        runningCoroutine = false;        
    }

    IEnumerator Chase()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", true);
        anim.SetBool("isAttacking", false);
        source.clip = chaseClip;
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        myNavMeshAgent.speed = chaseSpeed;

        myNavMeshAgent.SetDestination(player.transform.position);

        if (distanceFromPlayer <= attackRadius)
        {
            currentBehavior = enemyBehavior.attack;
        }
        if (distanceFromPlayer >= deAggro)
        {
            currentBehavior = enemyBehavior.patrol;
        }

        yield return new WaitForFixedUpdate();
        runningCoroutine = false;
    }

    IEnumerator Patrol()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        //print(anim.GetBool("isWalking"));
        source.clip = patrolClip;
        myNavMeshAgent.speed = patrolSpeed;
        yield return new WaitUntil(MovingToPoint);
        runningCoroutine = false;
    }

    bool MovingToPoint()
    {
        patrolTarget = player.transform;
        myNavMeshAgent.SetDestination(patrolTarget.position);

        if (Vector3.Distance(transform.position, patrolTarget.position) <= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            runningCoroutine = false;
            currentBehavior = enemyBehavior.chase;
        }
    }
}
