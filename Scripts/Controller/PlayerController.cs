using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Clase sencilla para llevar las stats basicas del jugador
public class PlayerController
{
    // Dano base del jugador, sobre el que luego se pueden aplicar mejoras permanentes
    private int m_damage;

    // Vida maxima que puede alcanzar el jugador
    private int m_maxLife;

    // Defensa actual del jugador, reduce el dano recibido
    private int m_defense;

    // Vida actual del jugador en este momento de la partida
    private int m_currentLife;

    /* Constructor que inicializa los valores de stats del jugador
     Se le pasa el dano base, la defensa inicial y la vida maxima*/
    public PlayerController(int damage, int defense, int maxlife)
    {
        m_damage = damage;
        m_defense = defense;
        m_currentLife = maxlife;    // Empezamos con la vida llena
        m_maxLife = maxlife;
    }

    /* Calcula el dano que hace el jugador a un enemigo
     De momento solo devuelve el dano base, pero aqui se podria meter logica extra
     en funcion del enemigo, debilidades, criticos, etc.*/
    public int Attack(EnemyData enemy)
    {
        int damageAux = m_damage;
        return damageAux;
    }

    // Devuelve la vida actual del jugador
    public int GetCurrentLife()
    {
        return m_currentLife;
    }

    // Devuelve la vida maxima del jugador
    public int GetMaxLife()
    {
        return m_maxLife;
    }

    // Cura al jugador una cantidad concreta
    // Si la curacion es negativa o cero, simplemente no hace nada
    public void HealDamage(int heal)
    {
        if (heal <= 0) return;

        m_currentLife += heal;

        // Nos aseguramos de no pasar de la vida maxima
        if (m_currentLife > m_maxLife)
            m_currentLife = m_maxLife;
    }

    // Aplica dano al jugador teniendo en cuenta la defensa
    // rawDamage es el dano del enemigo antes de la reduccion por defensa
    // Devuelve el dano final que realmente ha recibido el jugador
    public int ReceiveDamage(int rawDamage)
    {
        // Restamos la defensa al dano bruto y nos aseguramos de no bajar de 0
        int finalDamage = Mathf.Max(rawDamage - m_defense, 0);

        m_currentLife -= finalDamage;

        // Si nos hemos pasado por debajo de 0, dejamos la vida en 0 (muerto)
        if (m_currentLife < 0)
            m_currentLife = 0;

        return finalDamage;
    }

    // Sube el dano base del jugador
    // Se puede usar, por ejemplo, al comprar mejoras permanentes o coger items
    public void AddDamage(int amount)
    {
        m_damage += amount;

        // Por si acaso, evitamos que el dano baje de 0
        if (m_damage < 0)
            m_damage = 0;
    }

    // Sube la defensa del jugador
    // Igual que AddDamage, pero aplicado a la reduccion de dano
    public void AddDefense(int amount)
    {
        m_defense += amount;

        // Evitamos que la defensa se vuelva negativa
        if (m_defense < 0)
            m_defense = 0;
    }

    // Devuelve el ataque actual del jugador (dano base)
    public int GetAttack()
    {
        return m_damage;
    }

    // Devuelve la defensa actual del jugador
    public int GetDefense()
    {
        return m_defense;
    }

    // Devuelve true si el jugador esta muerto (vida 0 o menos)
    public bool IsDied()
    {
        return m_currentLife <= 0;
    }
}
