using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 2f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public Transform[] patrolPoints;

    private Animator animator;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isAttacking = false;

    public Transform attackPoint;

    public int attackDamage = 20;
    public LayerMask enemyLayers;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer < detectionRange)
        {
            StopAttacking();
            ChasePlayer();
        }
        else
        {
            StopAttacking();
            Patrol();
        }

        UpdateAnimation();
    }

    void Patrol()
    {
        isAttacking = false;
        agent.speed = patrolSpeed;

        if (patrolPoints.Length == 0) return;

        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void ChasePlayer()
    {
        isAttacking = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.ResetPath();
            transform.LookAt(player);
            animator.SetTrigger("Attack");
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = agent.velocity.magnitude > 0.1f;  // 当角色有明显速度时才视作移动
        animator.SetBool("IsMoving", isMoving);
    }

    void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            animator.ResetTrigger("Attack"); // ⬅️ 取消触发器
            animator.SetBool("IsAttacking", false); // ⬅️ 可选：如果你还有用Bool控制
        }
    }

    void PerformAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            Health targetHealth = enemy.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(attackDamage);
            }
        }
    }
}


