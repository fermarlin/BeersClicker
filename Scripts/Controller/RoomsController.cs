using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Controla que habitacion esta activa y se encarga de instanciar y destruir los prefabs dentro de su punto de spawn.
 La idea es que el GameManager le diga que tipo de sala toca ahora y este script se encarga de activar la que toque
*/
public class RoomsController : MonoBehaviour
{
    [Header("Salas")]
    public GameObject m_enemyRoom;  
    public GameObject m_itemRoom;
    public GameObject m_trapRoom;

    [Header("Spawns enemigo")]
    public Transform m_enemySpawnPoint;
    public Transform m_trapSpawnPoint;      
    public Transform m_itemSpawnPoint;      

    /*
     Referencias a las instancias actuales que hay dentro de cada sala.
     Las guardamos para poder destruirlas cuando cambiemos de tipo de habitacion.
    */
    private GameObject m_currentEnemyInstance;
    private GameObject m_currentTrapInstance;   
    private GameObject m_currentItemInstance;   


    /*
     Activa la sala de enemigo y crea el prefab del enemigo pasado por parametro.
     Si no hay datos o no hay prefab, simplemente deja la sala vacia.
    */
    public void SetRoom(EnemyData data)
    {
        m_enemyRoom.SetActive(true);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);

        // Nos aseguramos de limpiar cualquier instancia previa
        DestroyCurrentEnemy();
        DestroyCurrentTrap();
        DestroyCurrentItem();

        if (data == null)
        {
            return;
        }

        if (data.m_prefab == null)
        {
            return;
        }

        // Si tenemos un punto de spawn especifico lo usamos, si no usamos la propia sala
        Transform parent = m_enemySpawnPoint != null ? m_enemySpawnPoint : m_enemyRoom.transform;

        m_currentEnemyInstance =
            Instantiate(data.m_prefab, parent.position, parent.rotation, parent);

        // Intentamos configurar el EnemyBehaviour con su GameManager y sus datos
        EnemyBehaviour behaviour = m_currentEnemyInstance.GetComponent<EnemyBehaviour>();
        if (behaviour != null)
        {
            if (behaviour.m_gameManager == null)
                behaviour.m_gameManager = FindFirstObjectByType<GameManager>();

            behaviour.SetEnemy(data);
        }
    }

    /*
     Activa la sala de trampa y crea el prefab de la trampa 
     Este tipo de sala solo muestra la trampa
    */
    public void SetRoom(TrapData data)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(true);

        // Limpiamos cualquier cosa que quedara de salas anteriores
        DestroyCurrentEnemy();
        DestroyCurrentItem();
        DestroyCurrentTrap();

        // Instanciar prefab de trampa si hay datos y prefab asignado
        if (data != null && data.m_prefab != null)
        {
            // Igual que con el enemigo, si hay punto de spawn especifico lo usamos
            Transform parent = m_trapSpawnPoint != null ? m_trapSpawnPoint : m_trapRoom.transform;

            m_currentTrapInstance =
                Instantiate(data.m_prefab, parent.position, parent.rotation, parent);
        }
    }

    /*
     Activa la sala de item y crea el prefab del item pasado por parametro.
     Sirve para mostrar recompensas, curaciones, mejoras, etc.
    */
    public void SetRoom(ItemData data)
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(true);
        m_trapRoom.SetActive(false);

        // Limpiamos lo que hubiera en otras salas
        DestroyCurrentEnemy();
        DestroyCurrentTrap();
        DestroyCurrentItem();

        // Instanciar prefab de item si los datos son validos
        if (data != null && data.m_prefab != null)
        {
            Transform parent = m_itemSpawnPoint != null ? m_itemSpawnPoint : m_itemRoom.transform;

            m_currentItemInstance =
                Instantiate(data.m_prefab, parent.position, parent.rotation, parent);
        }
    }

    // Destruye el enemigo actual si existe y limpia la referencia
    public void DestroyCurrentEnemy()
    {
        if (m_currentEnemyInstance != null)
        {
            Destroy(m_currentEnemyInstance);
            m_currentEnemyInstance = null;
        }
    }

    // Destruye la trampa actual si existe y limpia la referencia
    public void DestroyCurrentTrap()
    {
        if (m_currentTrapInstance != null)
        {
            Destroy(m_currentTrapInstance);
            m_currentTrapInstance = null;
        }
    }

    // Destruye el item actual si existe y limpia la referencia
    public void DestroyCurrentItem()
    {
        if (m_currentItemInstance != null)
        {
            Destroy(m_currentItemInstance);
            m_currentItemInstance = null;
        }
    }

    // Desactiva todas las salas y destruye cualquier instancia que hubiera en enemigo, trampa o item.
    
    public void DisableRooms()
    {
        m_enemyRoom.SetActive(false);
        m_itemRoom.SetActive(false);
        m_trapRoom.SetActive(false);

        DestroyCurrentEnemy();
        DestroyCurrentTrap();
        DestroyCurrentItem();
    }

    /*
     Devuelve la instancia actual de enemigo en la sala.
     Si no hay enemigo devuelve null.
    */
    public GameObject GetEnemyRoom()
    {
        return m_currentEnemyInstance;
    }
}
