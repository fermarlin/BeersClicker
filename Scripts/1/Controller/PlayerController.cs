using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController
{
    private int m_damage;
    private int m_maxLife;
    private int m_defense;
    private int m_currentLife;

    public PlayerController(int damage, int defense, int maxlife)
    {
        m_damage = damage;
        m_defense = defense;
        m_currentLife = maxlife;
        m_maxLife = maxlife;
    }

    public void ReceiveDamage(int damage)
    {
        int currentDamage = damage - m_defense;

        if (currentDamage > 0)
        {
            m_currentLife -= currentDamage;
            Debug.Log("Daño recibido: " + currentDamage);
        }
    }

    public void HealDamage(int heal)
    {
        m_currentLife += heal;

        if (m_currentLife > m_maxLife)
            m_currentLife = m_maxLife;
    }

    public int Attack(EnemyData enemy)
    {
        int damageAux = m_damage;
        return damageAux;
    }

    public int GetCurrentLife()
    {
        return m_currentLife;
    }

    public int GetMaxLife()
    {
        return m_maxLife;
    }

    public bool IsDied()
    {
        // Mejor <= 0 para que 0 de vida ya cuente como muerto
        return m_currentLife <= 0;
    }
}
