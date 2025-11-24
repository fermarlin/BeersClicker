using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomsController : MonoBehaviour
{
    [Header("Rooms raíz")]
    public GameObject m_enemyRoom;  // contenedor de la habitación de enemigo
    public GameObject m_itemRoom;
    public GameObject m_trapRoom;

    [Header("Spawn enemigo")]
    public Transform m_enemySpawnPoint; // donde aparecerá el prefab

    [Header("Sprites estáticos")]
    public SpriteRenderer m_itemSprite;
    public SpriteRenderer m_trapSprite;

    private GameObject m_currentEnemyInstance;

    // ENEMY ROOM (prefab con animator)
    public void SetRoom(EnemyData data)
    {
        m_enemyRoom.SetActive(true);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);

        DestroyCurrentEnemy();

        if (data == null)
        {
            Debug.LogWarning("SetRoom(EnemyData): data es null");
            return;
        }

        if (data.m_prefab == null)
        {
            Debug.LogWarning($"EnemyData '{data.m_name}' no tiene prefab asignado.");
            return;
        }

        Transform parent = m_enemyRoom.transform;
        Vector3 pos = parent.position;
        Quaternion rot = parent.rotation;

        if (m_enemySpawnPoint != null)
        {
            pos = m_enemySpawnPoint.position;
            rot = m_enemySpawnPoint.rotation;
        }

        m_currentEnemyInstance = Object.Instantiate(data.m_prefab, pos, rot, m_enemySpawnPoint.transform);

        EnemyBehaviour behaviour = m_currentEnemyInstance.GetComponent<EnemyBehaviour>();
        if (behaviour == null)
        {
            Debug.LogWarning($"El prefab '{data.m_prefab.name}' no tiene EnemyBehaviour.");
        }
        else
        {
            if (behaviour.m_gameManager == null)
                behaviour.m_gameManager = Object.FindFirstObjectByType<GameManager>();

            behaviour.SetEnemy(data);
        }
    }

    // TRAP ROOM
    public void SetRoom(TrapData data, Sprite sprite)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(true);

        if (m_trapSprite != null)
            m_trapSprite.sprite = sprite;
    }

    // ITEM ROOM
    public void SetRoom(ItemData data, Sprite sprite)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(true);
        m_trapRoom.SetActive(false);

        if (m_itemSprite != null)
            m_itemSprite.sprite = sprite;
    }

    public void DisableRooms()
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);

        DestroyCurrentEnemy();
    }

    public void DestroyCurrentEnemy()
    {
        if (m_currentEnemyInstance != null)
        {
            Object.Destroy(m_currentEnemyInstance);
            m_currentEnemyInstance = null;
        }
    }

    public GameObject GetEnemyRoom()
    {
        return m_currentEnemyInstance;
    }
}
