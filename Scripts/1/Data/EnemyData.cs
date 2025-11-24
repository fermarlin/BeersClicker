using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    [Header("Stats")]
    public string m_name;
    public int m_damage;
    public int m_defense;
    public int m_maxLife;
    public int m_rewardExperience;
    public float m_timeBetweenAttacks = 2f;
    [Range(0, 100)]
    public int m_percentageStrongAttack = 20;

    [Header("Prefab")]
    public GameObject m_prefab;   // Aquí arrastras el prefab con Animator

    [HideInInspector]
    public int m_currentLife;

    public EnemyData(string name, int damage, int defense, int maxLife,
                     int rewardExperience, float timeBetweenAttacks,
                     int percentageStrongAttack)
    {
        m_name = name;
        m_damage = damage;
        m_defense = defense;
        m_maxLife = maxLife;
        m_rewardExperience = rewardExperience;
        m_timeBetweenAttacks = timeBetweenAttacks;
        m_percentageStrongAttack = percentageStrongAttack;

        ResetLife();
    }

    // Constructor de copia (para que cada room tenga su propia vida)
    public EnemyData(EnemyData other)
    {
        m_name = other.m_name;
        m_damage = other.m_damage;
        m_defense = other.m_defense;
        m_maxLife = other.m_maxLife;
        m_rewardExperience = other.m_rewardExperience;
        m_timeBetweenAttacks = other.m_timeBetweenAttacks;
        m_percentageStrongAttack = other.m_percentageStrongAttack;
        m_prefab = other.m_prefab;

        ResetLife();
    }

    public void ResetLife()
    {
        m_currentLife = m_maxLife;
    }
}
