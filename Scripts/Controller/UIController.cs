using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 Este script se encarga de gestionar toda la UI.
 La idea es que el GameManager llame a estos metodos para ir mostrando lo que toca en cada momento.
*/
public class UIController : MonoBehaviour
{
    public GameObject m_gameUI;          // Panel con la UI principal de partida
    public GameObject m_startButtom;     // Boton inicial para comenzar
    public HealthBarUI m_enemyLife;      // Barra de vida del enemigo
    public HealthBarUI m_playerLife;     // Barra de vida del jugador

    public GameObject gameOverScreen;    // Pantalla de game over

    /*
     Activa o desactiva el panel principal de la UI de juego.
     Se usa cuando comienza la partida
    */
    public void EnableGameUI(bool isActive)
    {
        m_gameUI.SetActive(isActive);
    }

    // Activa o desactiva el boton de inicio.
    public void EnableStartButtom(bool isActive)
    {
        m_startButtom.SetActive(isActive);
    }

    /*
     Activa la barra de vida del enemigo cuando comienza un combate.
     Tambien resetea la barra a 1/1 para evitar que se quede la barra de vida del anterior colgando
    */
    public void EnableEnemyBar(bool isActive)
    {
        m_enemyLife.transform.parent.gameObject.SetActive(isActive);
        m_enemyLife.gameObject.SetActive(isActive);

        if (isActive)
            m_enemyLife.UpdateHealthBar(1, 1);
    }

    /*
     Actualiza la barra de vida del enemigo con los valores actuales.
     amount = vida actual
     maxLife = vida maxima
    */
    public void EnemyLifeBar(int amount, int maxLife)
    {
        m_enemyLife.UpdateHealthBar(amount, maxLife);
    }

    /*
     Igual que con el enemigo, pero para el jugador.
     Se llama cada vez que recibe dano o se cura.
    */
    public void PlayerLifeBar(int amount, int maxLife)
    {
        m_playerLife.UpdateHealthBar(amount, maxLife);
    }
}
