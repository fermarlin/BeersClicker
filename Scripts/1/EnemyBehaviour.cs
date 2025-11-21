using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour
{
    public GameManager m_gameManager;
    public EnemyData m_enemyData;
    public SpriteRenderer m_sprite;
    public float m_counterTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_counterTime = 0;
    }

    public void SetEnemy(EnemyData enemyData)
    {
        m_enemyData = enemyData;
    }

    public void Greet()
    {
        Debug.Log("Un enemigo ha aparecido es un " + m_enemyData.m_name + "!");
        Debug.Log("Tiene una fuerza de " + m_enemyData.m_damage);
        Debug.Log("Un enemigo ha aparecido es un " + m_enemyData.m_currentLife);

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
            m_enemyData.m_currentLife -= damage - m_enemyData.m_defense;
            Debug.Log("AY");

        }

        if (m_enemyData.m_currentLife < 0)
        {
            Debug.Log("IS DIE");
        }
    }

    public bool IsDie()
    {
        if (m_enemyData.m_currentLife < 0)
        {
            return true;
        }
        else 
        {
            return false;   
        }
    }

    void Update()
    {
        m_counterTime += Time.deltaTime;

        if (m_counterTime > m_enemyData.m_timeBetweenAttacks)
        {
            m_counterTime = 0;
            Attack();
        }
    }
}
