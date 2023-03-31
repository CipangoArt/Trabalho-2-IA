using Panda;
using UnityEngine;
using UnityEngine.AI;



public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] AIBehaviour player;
    Vector3 playerDir;
    [SerializeField] LayerMask layerMask;
    public Vector3 lastPlayerPos;
    [SerializeField] NavMeshAgent enemyAgent;
    bool awareOfPlayer;
    public bool alerted;
    [SerializeField] EnemyBehaviour patroller;
    [SerializeField] bool isStatic=false;
    [SerializeField] Transform staticPos;


    [Task]
    bool IsMoving()
    {
        //bool moving = enemyAgent.remainingDistance <= enemyAgent.stoppingDistance;
        bool moving = enemyAgent.velocity.magnitude <= 0.1f ? false : true;
        return moving;
    }

    [Task]
    bool isAware()
    {
        return awareOfPlayer;
    }

    [Task]
    bool BecomeUnaware()
    {
        awareOfPlayer = false;
        return true;
    }

    [Task]
    bool Idle()
    {
        enemyAgent.SetDestination(staticPos.position);
        return true;
    }


    [Task]
    bool PatrolArea()
    {
        Vector3 RandomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        enemyAgent.SetDestination(RandomDir.normalized * 10 + transform.position );
        return true;
    }

    [Task]
    bool MustHaveBeenTheWind()
    {
        awareOfPlayer = false;
        Vector3 RandomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        enemyAgent.SetDestination(RandomDir.normalized * 1 + transform.position );
        return false;
    }

    [Task]
    bool ChasePlayer()
    {
        enemyAgent.SetDestination(lastPlayerPos);
        return true;
    }






    [Task]
    bool CanSeePlayer()
    {
        if (alerted) return true;
        return PlayerOnVision();
    }

    [Task]

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AIBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = player.transform.position - transform.position;
    }


    bool PlayerOnVision()
    {
        RaycastHit playerHit;
        if (Physics.Raycast(transform.position, playerDir.normalized, out playerHit))
        {
            if (playerHit.collider.CompareTag("Player"))
            {
                awareOfPlayer = true;
                lastPlayerPos = player.transform.position;
                if (isStatic)
                {
                    patroller.alerted = true;
                    patroller.lastPlayerPos = player.transform.position;
                }
                
                return true;
            }
            else
            {
                patroller.alerted = false;
            }

        }
        ;
        return false;

    }
}
