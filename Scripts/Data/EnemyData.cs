using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Aqui guardamos todo sobre los enemigos su nombre, estadisticas basicas, prefab asociado
 y informacion de combate
*/
[System.Serializable]
public class EnemyData
{
    [Header("Stats")]
    public string m_name;                   // Nombre del enemigo
    public int m_damage;                    // Dano
    public int m_defense;                   // Defensa
    public int m_maxLife;                   // Vida maxima
    public int m_rewardExperience;          // Experiencia
    public float m_timeBetweenAttacks = 2f; // Tiempo entre ataques en combate

    [Range(0, 100)]
    public int m_percentageStrongAttack = 20;   // Probabilidad de usar ataque fuerte

    [Header("Prefab")]
    public GameObject m_prefab;   // Prefab del enemigo

    [HideInInspector]
    public int m_currentLife;     // Vida actual del enemigo


     //Constructor para crear un enemigo
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

    /*
     Esto sirve para que cada sala tenga su propia instancia de vida independiente.
    */
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

    // Resetea la vida del enemigo 
    public void ResetLife()
    {
        m_currentLife = m_maxLife;
    }
}
