using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
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
            m_currentLife -= damage - m_defense;
            Debug.Log("AY NIGGA");
        }
    }
    public void HealDamage(int heal)
    {
        m_currentLife += heal;
        
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
        if (m_currentLife < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
