using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour
{
    public GameManager m_gameManager;
    public EnemyData m_enemyData;

    private Coroutine m_attackCoroutine;

    void Awake()
    {
        // Por si se te olvida arrastrarlo en el inspector
        if (m_gameManager == null)
            m_gameManager = FindFirstObjectByType<GameManager>();
    }

    public void SetEnemy(EnemyData enemyData)
    {
        m_enemyData = enemyData;

        if (m_enemyData != null && m_enemyData.m_currentLife <= 0)
            m_enemyData.ResetLife();

        if (m_attackCoroutine != null)
            StopCoroutine(m_attackCoroutine);

        if (m_enemyData != null)
            m_attackCoroutine = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (m_enemyData == null || m_enemyData.m_currentLife <= 0)
            {
                StopAttackLoop();
                yield break;
            }

            if (m_gameManager != null && m_gameManager.IsGameOver())
            {
                StopAttackLoop();
                yield break;
            }

            float delay = Mathf.Max(0.1f, m_enemyData.m_timeBetweenAttacks);
            yield return new WaitForSeconds(delay);

            if (m_enemyData == null || m_enemyData.m_currentLife <= 0)
            {
                StopAttackLoop();
                yield break;
            }

            if (m_gameManager != null && !m_gameManager.IsGameOver())
            {
                Attack();
            }
        }
    }

    private void StopAttackLoop()
    {
        if (m_attackCoroutine != null)
        {
            StopCoroutine(m_attackCoroutine);
            m_attackCoroutine = null;
        }
    }

    public void Greet()
    {
        Debug.Log($"Un enemigo ha aparecido: {m_enemyData.m_name}");
        Debug.Log($"Fuerza: {m_enemyData.m_damage}");
        Debug.Log($"Vida: {m_enemyData.m_currentLife}/{m_enemyData.m_maxLife}");
    }

    public void Attack()
    {
        int attackValue = Random.Range(0, 100);
        int damage = (attackValue <= m_enemyData.m_percentageStrongAttack)
            ? m_enemyData.m_damage * 2
            : m_enemyData.m_damage;

        Debug.Log($"[Enemy] {m_enemyData.m_name} ataca por {damage} de daño.");
        m_gameManager.EnemyAttack(damage);
    }

    public void RecieveDamage(int damage)
    {
        int currentDamage = damage - m_enemyData.m_defense;

        if (currentDamage > 0)
        {
            m_enemyData.m_currentLife -= currentDamage;
            Debug.Log($"[Enemy] AY, vida actual: {m_enemyData.m_currentLife}");
        }

        if (m_enemyData.m_currentLife <= 0)
        {
            Debug.Log("[Enemy] IS DIE");
            Die();
        }
    }

    private void Die()
    {
        StopAttackLoop();
    }

    public bool IsDie()
    {
        return m_enemyData != null && m_enemyData.m_currentLife <= 0;
    }

    void OnDisable()
    {
        StopAttackLoop();
    }
}
