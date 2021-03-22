using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public LayerMask groundMask, playerMask;
    public Animator enemyAnimator;

    public bool isDead;

    public float health = 100f;
    public float enemyDamage;
    
    //Patrolling Area
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    
    
    //Chasing player
    public bool isChasingPlayer;
    
    //Attacking player
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public GameObject projectile;

    public float sightRadius;
    public float attackRadius;
    public bool playerIsInSightrange;
    public bool playerIsInAttackrange;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("First Person Player").transform;
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is in sight and attack range
        playerIsInSightrange = Physics.CheckSphere(transform.position, sightRadius, playerMask);
        playerIsInAttackrange = Physics.CheckSphere(transform.position, attackRadius, playerMask);

        if (!playerIsInSightrange && !playerIsInAttackrange)
        {
            isChasingPlayer = false;
            Patrol();
            if (!isChasingPlayer)
            {
                agent.SetDestination(walkPoint);
            }
            
        }
        
        if (playerIsInSightrange && !playerIsInAttackrange)
        {
            isChasingPlayer = true;
            ChasePlayer();
        }
        
        if (playerIsInSightrange && playerIsInAttackrange)
        {
            AttackPlayer();
        }
        
    }

    void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
            enemyAnimator.SetBool("Walking", true);
            Debug.Log("Searching");
            
        }
/*
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Debug.Log("moving");
        }*/

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //when walk point is reached
         if (distanceToWalkPoint.magnitude < 1f)
         {
             walkPointSet = false;
             Debug.Log("Point Reached");
         }
    }

    void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        //walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        walkPoint = new Vector3(randomX, transform.position.y, randomZ);
        
        //Debug.Log(walkPoint);

        //checks if random walk point is inside of the map
        if (Physics.Raycast(walkPoint, -transform.up, 3f, groundMask))
        {
            walkPointSet = true;
            Debug.Log("Patrolling");
        }
        
        agent.SetDestination(walkPoint);
    }
    
    void ChasePlayer()
    {
        enemyAnimator.SetBool("Attacking", false);
        agent.SetDestination(player.position);
        enemyAnimator.SetBool("Walking", true);
    }
    
    void AttackPlayer()
    {

        //Enemy won't move when attacking
        enemyAnimator.SetBool("Walking", false);
        agent.SetDestination(transform.position);
        
        //enemy will look at the player
        RotateTowards(player);
        
        enemyAnimator.SetBool("Attacking", true);
        
        if (!alreadyAttacked)
        {
            var startPostition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
            
            Rigidbody rb = Instantiate(projectile, startPostition, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //Enemy Health
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(walkPoint, 1);
    }
}
