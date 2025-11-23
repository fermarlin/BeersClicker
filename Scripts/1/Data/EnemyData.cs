using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string m_name;
    public int m_damage;
    public int m_defense;
    public int m__maxLife;
    public int m_rewardExperience;
    public float _timeBetweenAttacks;
    public int m_percentageStrongAttack;

    public int m_currentLife;
    public int m_timeBetweenAttacks;
    public int m_maxLife;
    public GameObject m_prefab;
    
    public EnemyData(string name, int damage, int defense, int maxLife, int rewardExperience, int timeBetweenAttacks, int percentageStrongAttack)
    {
        m_name = name;
        m_damage = damage;
        m_defense = defense;
        m_maxLife = maxLife;
        m_rewardExperience = rewardExperience;
        m_timeBetweenAttacks = timeBetweenAttacks;
        m_percentageStrongAttack = percentageStrongAttack;

        m_currentLife = maxLife;
    }
}
