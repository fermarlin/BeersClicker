using UnityEngine;
using System.Collections;

/*
 Aqui se controla que puede hacer el player en una sala con un objeto
 Este script detecta el click y aplica el item
 siempre que haya pasado un tiempo minimo antes de permitir la accion.
 (Esto esta genial para que el juador pare un momento y compre mejoras si le
 falta)
*/
public class PlayerItemMonoBehavior : MonoBehaviour
{
    public GameManager m_gameManager;     // Referencia al GameManager

    [SerializeField]
    private float m_timeToChangeRoom = 1f;   // Tiempo minimo antes de poder usar el objeto

    private float m_counterRoomTime;          // Contador de tiempo

    /*
     Iniciamos el contador a 0 para obligar a esperar m_timeToChangeRoom
     antes de permitir que se consuma el item.
    */
    void Start()
    {
        m_counterRoomTime = 0f;
    }

    /*
     En cada frame sumamos tiempo y comprobamos si se ha hecho click.
     Si el tiempo minimo ha pasado y estamos en una sala de item,
     aplicamos el item y saltamos a la siguiente sala.
    */
    void Update()
    {
        m_counterRoomTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && m_counterRoomTime > m_timeToChangeRoom)
        {
            // Pedimos al GameManager la sala actual
            RoomData currentRoom = m_gameManager.GetCurrentRoom();

            // Solo funciona si esta sala es de tipo ITEM
            if (currentRoom != null && currentRoom.m_roomtype == RoomData.Roomtype.ITEM)
            {
                ItemData item = currentRoom.m_item;

                /*
                 Aplicamos realmente el item usando sus valores, recuperar vida
                 hacer mas dano o mas defensa
                */
                m_gameManager.ApplyItem(
                    item.m_lifeUp,
                    item.m_damageUp,
                    item.m_defenseUp
                );

                // Cambiamos de sala justo despues de consumir el item
                m_gameManager.ChangeRoom();

                // Reiniciamos el contador
                m_counterRoomTime = 0f;
            }
        }
    }
}
