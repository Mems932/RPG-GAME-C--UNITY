using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float speed;
    private float playerDetectTime;
    public float playerDetectRate;
    public float chaseRange;

    [Header("Attack")]
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float attackRate;
    private float lastAttackTime;

    [Header("Component")]
    Rigidbody2D rb;  
    private PlayerController targetPlayer;


    [Header("Pathfinding")]
    public float nextWaypointDistance = 2f;
    Path path;
    int currentWaypoint = 0;
    bool reachEndOfPath = false;
    Seeker seeker;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath",0f, .5f);
    }
    
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && targetPlayer != null)
        {
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if(targetPlayer != null)
        {
            float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time > lastAttackTime + attackRate)
            {
                rb.velocity = Vector2.zero;
            }
            else if (dist > attackRange)
            {
                if (path == null)
                    return;

                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachEndOfPath = true;
                    return;
                }
                else
                {
                    reachEndOfPath = false;
                }
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.deltaTime;

                rb.velocity = force;
            
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }


        

        DetectPlayer();
    }



    void DetectPlayer()
    {
        if(Time.time - playerDetectTime > playerDetectRate)
        {
            playerDetectTime = Time.time;

            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if(player != null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);

                    if (player == targetPlayer)
                    {
                        if (dist < chaseRange)
                        {
                            targetPlayer = null;
                        }
                    } else if(dist < chaseRange)
                    {
                        if (targetPlayer == null) 
                        targetPlayer = player;
                    }
                    
                }
            }
        }
    }
}
