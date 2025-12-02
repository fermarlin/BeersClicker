using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 Control del input del jugador en una sala con una trampa
 Aqui se decide si el jugador reacciona demasiado rapido (y recibe dano),
 o si espera el tiempo para no comerse la trampa.
*/
public class PlayerTrapInputBehaviour : MonoBehaviour
{
    public GameManager m_gameManager;   // Referencia al GameManager

    [SerializeField]
    private float m_timeToChangeRoom = 1.5f;  // Tiempo 

    [SerializeField]
    private int m_trapDamage = 15;            // Dano configurable para esta trampa

    private float m_counterRoomTime;          // Contador de tiempo
    private bool m_roomResolved = false;      // Para evitar que se ejecute dos veces

    // Al empezar reseteamos el estado interno de la trampa.
    void Start()
    {
        ResetTrapState();
    }

    /*
     Resetea el contador y marca la sala como no resuelta.
     Se llama tanto al iniciar como cada vez que entramos a una nueva trampa.
    */
    private void ResetTrapState()
    {
        m_counterRoomTime = 0f;
        m_roomResolved = false;
    }

    /*
     Se llama desde GameManager justo cuando entramos a una sala de trampa.
     Hace que el comportamiento empiece limpio.
    */
    public void PrepareForNewTrap()
    {
        ResetTrapState();
    }

    /*
     Aqui se gestiona la logica, si gaces click ANTES del tiempo limite
     recibes dano y cambias de sala si NO haces click y el tiempo se acaba cambias de sala sin recibir dano
    */
    void Update()
    {
        if (m_roomResolved)
            return;

        m_counterRoomTime += Time.deltaTime;

        // Reaccionas demasiado rapido te comes la trampa
        if (Input.GetMouseButtonDown(0) && m_counterRoomTime < m_timeToChangeRoom)
        {
            m_roomResolved = true;

            m_gameManager.ApplyTrap(m_trapDamage); // Aplica el dano configurado
            m_gameManager.ChangeRoom();            // Pasamos a siguiente sala

            return;
        }

        // Esperas lo suficiente y evitas el dano
        if (m_counterRoomTime >= m_timeToChangeRoom)
        {
            m_roomResolved = true;
            m_gameManager.ChangeRoom();
        }
    }
}
