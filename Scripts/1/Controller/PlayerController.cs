using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private int m_damage;
    private int m_maxLife;
    private int m_currentLife;

    public PlayerController(int damage)
    {
        m_damage = damage;
    }

    public int Attack()
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
        if (m_currentLife <= 0)
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
