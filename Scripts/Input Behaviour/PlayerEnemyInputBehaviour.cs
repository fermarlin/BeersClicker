using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Este script controla el input del jugador para atacar a enemigos.
 La idea es que un click corto haga un ataque normal
 y un click mantenido haga un ataque fuerte.
*/
public class PlayerEnemyInputBehavout : MonoBehaviour
{
    public GameManager m_gameManager;     // Referencia al GameManager
    float m_counterButtomTime = 0;        // Tiempo cuando se pulso el boton por primera vez

    /*
     Al iniciar, registramos el tiempo inicial y buscamos el GameManager
     si no fue asignado manualmente en el inspector.
    */
    void Start()
    {
        m_counterButtomTime = Time.time;

        if (m_gameManager == null)
        {
            m_gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    /*
     Cada frame comprobamos si se hace click. Con MouseDown guardamos el tiempo 
     en que comenzo el click y con MouseUp: segun el tiempo que lo haya
     mantenido, lanzo ataque normal o fuerte
    */
    void Update()
    {
        // Si no hay GameManager o el juego se ha terminado, ignoramos los inputs
        if (m_gameManager == null || m_gameManager.IsGameOver())
            return;

        // Registro de cuando se pulsa
        if (Input.GetMouseButtonDown(0))
        {
            m_counterButtomTime = Time.time;
        }

        // Ataque normal
        if (Input.GetMouseButtonUp(0) && (Time.time - m_counterButtomTime < 0.25f))
        {
            m_gameManager.PlayerAttack();
        }

        // Ataque fuerte
        if (Input.GetMouseButtonUp(0) && (Time.time - m_counterButtomTime >= 0.25f))
        {
            m_gameManager.PlayerStrongAttack();
        }
    }
}
