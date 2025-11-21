using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomsController : MonoBehaviour
{
    public GameObject m_enemyRoom;
    public GameObject m_itemRoom;
    public GameObject m_trapRoom;

    public SpriteRenderer m_enemySprite;
    public SpriteRenderer m_itemSprite;
    public SpriteRenderer m_trapSprite;


    public void SetRoom(EnemyData data, Sprite sprite)
    {
        m_enemyRoom.SetActive(true);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);
        m_enemyRoom.GetComponent<EnemyBehaviour>().SetEnemy(data);
        m_enemySprite.sprite = sprite;
    }

    public void SetRoom(TrapData data, Sprite sprite)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(true);
        m_trapRoom.SetActive(false);

        m_trapSprite.sprite = sprite;
    }

    public void SetRoom(ItemData data, Sprite sprite)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(true);
        m_itemSprite.sprite = sprite;
    }

    public void DisableRooms()
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);
    }

    public GameObject GetEnemyRoom()
    {
        return m_enemyRoom;
    }
}