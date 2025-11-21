using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEnemyInputBehavout : MonoBehaviour
{
    public GameManager m_gameManager;
    float m_counterButtomTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_counterButtomTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_counterButtomTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && (Time.time - m_counterButtomTime < 0.25f))
        {
            m_gameManager.PlayerAttack();
        }
        if (Input.GetMouseButtonUp(0) && (Time.time - m_counterButtomTime >= 0.25f))
        {
            m_gameManager.PlayerStrongAttack();
        }
    }
}
