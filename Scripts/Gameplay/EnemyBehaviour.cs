using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Este script controla a los enemigos su vida actual, su bucle de ataques y el dano que recibe.
*/
public class EnemyBehaviour : MonoBehaviour
{
    public GameManager m_gameManager;   // Referencia al GameManager
    public EnemyData m_enemyData;       // Datos del enemigo cargados desde EnemyData

    private Coroutine m_attackCoroutine; // Handle de la corutina de ataque

    // Al arrancar, si no tenemos referencia al GameManager, la buscamos en la escena.

    void Awake()
    {
        if (m_gameManager == null)
            m_gameManager = FindFirstObjectByType<GameManager>();
    }

    /*
     Asigna los datos de un enemigo a este comportamiento.
     Si la vida esta a 0, se resetea.
     Tambien reinicia el bucle de ataques.
    */
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

    /*
     Corutina que controla los ataques periodicos del enemigo.
     Espera un tiempo entre ataques y luego llama a Attack().

     Se para automaticamente si se muere el enemigo o si se acaba la partida
    */
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

    // Detiene la corutina de ataques
    private void StopAttackLoop()
    {
        if (m_attackCoroutine != null)
        {
            StopCoroutine(m_attackCoroutine);
            m_attackCoroutine = null;
        }
    }

    /*
     Realiza un ataque al jugador.
     Calcula si es ataque fuerte segun el porcentaje definido en EnemyData.
     Luego notifica al GameManager para que aplique el dano.
    */
    public void Attack()
    {
        int attackValue = Random.Range(0, 100);

        int damage = (attackValue <= m_enemyData.m_percentageStrongAttack)
            ? m_enemyData.m_damage * 2
            : m_enemyData.m_damage;

        m_gameManager.EnemyAttack(damage);
    }

    /*
     Cuando recibe dano desde el jugador, se le resta su defensa.
     Si la vida baja a 0 o menos, se llama a Die().
    */
    public void RecieveDamage(int damage)
    {
        int currentDamage = damage - m_enemyData.m_defense;

        if (currentDamage > 0)
        {
            m_enemyData.m_currentLife -= currentDamage;
        }

        if (m_enemyData.m_currentLife <= 0)
        {
            Die();
        }
    }

    // Cuando el enemigo muere simplemente detiene el bucle de ataque.
    private void Die()
    {
        StopAttackLoop();
    }

    // Devuelve true si el enemigo ya no tiene vida.

    public bool IsDie()
    {
        return m_enemyData != null && m_enemyData.m_currentLife <= 0;
    }

    //Esto es por asegurarme, si el objeto se desactiva que pare de atacar

    void OnDisable()
    {
        StopAttackLoop();
    }
}
