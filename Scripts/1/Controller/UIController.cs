using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public GameObject m_gameUI;
    public GameObject m_startButtom;
    public Image m_enemyLife;
    public Image m_playerLife;
    public GameObject gameOverScreen;

    public void EnableGameUI (bool isActive)
    {
        m_gameUI.SetActive(isActive);
    }

    public void EnableStartButtom (bool isActive)
    {
        m_startButtom.SetActive(isActive);
    }

    public void EnableEnemyBar (bool isActive)
    {
        m_enemyLife.gameObject.SetActive(isActive);
    }

    public void EnemyLifeBar (int amount, int maxLife)
    {
        m_enemyLife.fillAmount = (float)amount /(float)maxLife;
    }

    public void PlayerLifeBar (int amount, int maxLife)
    {
        m_playerLife.fillAmount = (float)amount / (float)maxLife;
    }
}
