using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour
{
    public GameManager m_gameManager;
    public EnemyData m_enemyData;
    public SpriteRenderer m_sprite;

    private Coroutine m_attackCoroutine;


    public void SetEnemy(EnemyData enemyData)
    {
        m_enemyData = enemyData;

        if (m_attackCoroutine != null)
            StopCoroutine(m_attackCoroutine);

        m_attackCoroutine = StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            float delay = m_enemyData.m_timeBetweenAttacks;
            yield return new WaitForSeconds(delay);

            Attack();
        }
    }


    public void Greet()
    {
        Debug.Log("Un enemigo ha aparecido es un " + m_enemyData.m_name + "!");
        Debug.Log("Tiene una fuerza de " + m_enemyData.m_damage);
        Debug.Log("Tiene una vida de " + m_enemyData.m_currentLife);
    }

    public void Attack()
    {
        int attackValue = Random.Range(0, 100);
        int damage = 0;

        if (attackValue <= m_enemyData.m_percentageStrongAttack)
        {
            damage = m_enemyData.m_damage * 2;
        }
        else
        {
            damage = m_enemyData.m_damage;
        }

        m_gameManager.EnemyAttack(damage);
    }

    public void RecieveDamage(int damage)
    {
        int currentDamage = damage - m_enemyData.m_defense;

        if (currentDamage > 0)
        {
            m_enemyData.m_currentLife -= currentDamage;
            Debug.Log("AY");
        }

        if (m_enemyData.m_currentLife < 0)
        {
            Debug.Log("IS DIE");
        }
    }

    public bool IsDie()
    {
        return m_enemyData.m_currentLife < 0;
    }

    void OnDisable()
    {
        // Por si desactivas la room / enemigo, paramos la corutina
        if (m_attackCoroutine != null)
        {
            StopCoroutine(m_attackCoroutine);
            m_attackCoroutine = null;
        }
    }

}
