using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 5f;

    NavMeshAgent navMeshAgent; // simple easy to recall names
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false; //  start the game idling until player comes near or provokes.
    EnemyHealth health; 

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>(); 

        if (navMeshAgent == null) // the debugs have been comented out so that I dont get contant updates on navmeshagents location.
                                  // I am however going to keep these in the code incase something breaks in the future and I have to find it.
        {
            //Debug.LogError("NavMeshAgent component not found on this GameObject.");
        }

        if (health == null)
        {
          //  Debug.LogError("EnemyHealth component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead()) // what to do if enemyHealth script is saying zombie is dead
        {
            enabled = false; // turn off movement and navemeshagent and return
            navMeshAgent.enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked) // if the player is out of sensing range but shot zombie then chase player
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange) // otherwise if the player is withen chase range chase player.
        {
            isProvoked = true;
        }
    }

    public void OnDamageTaken() // if shot then zombie is provoked and chases player.
    {
        isProvoked = true;
    }

    private void EngageTarget()
    {
        FaceTarget(); // turn body to always face the player when chasing/attacking.
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        GetComponent<Animator>().SetBool("attack", false); // pretty simple - if chasing the target turn off attack anim and play move anim.
        GetComponent<Animator>().SetTrigger("move");

        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(target.position);
           // Debug.Log("Setting destination to target position: " + target.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not on a NavMesh."); // this debug is not commented out because it comes in handy, searching for bugs.
        }
    }

    private void AttackTarget()
    {
        GetComponent<Animator>().SetBool("attack", true); // set the boolean for the animation "attack to true if the zombie is attacking"
    }

    private void FaceTarget() // function to always face ther playere when provoked.
    {
        Vector3 direction = (target.position - transform.position).normalized; // getting direction of player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed); // transform.rotate only changes
                                                                                                             // the rotation of the zombie and nothing else.
    }

    void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);// this will project a wireframe sphere around the enemy to inidicate the chase range.
                                                              // making it easier to visualise when making changes or balancing the game difficulty.
                                                              // this has no impact on the actual game. its purely for making changes in the editor.
    }
}
