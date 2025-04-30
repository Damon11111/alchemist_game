using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float lastAttackTime;
    private float comboTimeout = 1.0f;

    private AnimatorStateInfo currentState;

    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 20;
    public LayerMask enemyLayers;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 1 → Attack（起手）
        if (Input.GetKeyDown(KeyCode.Alpha1) && comboStep == 0)
        {
            animator.SetTrigger("Attack");
            comboStep = 1;
            lastAttackTime = Time.time;
        }

        // 2 → Attack1（必须在 Attack 中触发）
        if (Input.GetKeyDown(KeyCode.Alpha2) &&
            comboStep == 1 &&
            currentState.IsName("Attack") &&
            currentState.normalizedTime > 0.4f)
        {
            animator.SetTrigger("Attack1");
            comboStep = 2;
            lastAttackTime = Time.time;
        }

        // 3 → Hurricane Kick（任何时候都能触发）
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("Hurricane");
            lastAttackTime = Time.time;
        }

        // Hurricane Kick 动作播放期间，让角色旋转（模拟旋风腿）
        if (currentState.IsName("Hurricane Kick") && currentState.normalizedTime < 1.0f)
        {
            transform.Rotate(Vector3.up * 1440f * Time.deltaTime); // 每秒转两圈
        }

        // 动作完成后自动重置
        if (currentState.IsName("Hurricane Kick") && currentState.normalizedTime >= 1.0f)
        {
            ResetCombo();
        }
        else if ((comboStep > 0 && Time.time - lastAttackTime > comboTimeout) || currentState.IsName("Idle"))
        {
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        comboStep = 0;
    }

    // 举例：在攻击范围内攻击敌人
    void TryHitEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 1.5f, 1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<Health>()?.TakeDamage(20);
            }
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
