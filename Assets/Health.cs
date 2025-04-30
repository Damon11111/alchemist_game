using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public bool IsDead => currentHealth <= 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 受到伤害！当前生命值: {currentHealth}");

        if (IsDead)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} 死亡！");
        // 可加动画、销毁或禁用
        gameObject.SetActive(false);
    }
}
