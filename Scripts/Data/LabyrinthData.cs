using System.Collections.Generic;
using UnityEngine;

/*
 En este script guardamos todas las salas en orden y controlamos cual es la sala
 actual en la que se encuentra el jugador.

 La idea es que GameManager use esta clase para poder avanzar d sala, comprobar si hemos terminado, reiniciar, etc.
*/
public class LabyrinthData
{
    private List<RoomData> m_rooms;           // Lista de todas las salas que forman el laberinto
    private int m_currentRoomIndex;           // Indice de la sala actual

    /*
     Constructor base, inicializa la lista vacia y deja el indice en -1
     para indicar que todavia no se ha entrado a ninguna sala.
    */
    public LabyrinthData()
    {
        m_rooms = new List<RoomData>();
        m_currentRoomIndex = -1;
    }

    /*
     Anadimos una sala nueva al laberinto.
     Se asigna el puntero inverso (room.m_labyrinth = this) para que la sala
     pueda consultar datos del laberinto si lo necesita.
    */
    public void AddRoom(RoomData room)
    {
        if (room == null)
            return;

        room.m_labyrinth = this;
        m_rooms.Add(room);
    }

    /*
     Avanza a la siguiente sala del laberinto.
     Al llamar a este metodo subimos el indice de la sala actual.
    */
    public void ChangeRoom()
    {
        if (m_rooms.Count == 0)
        {
            m_currentRoomIndex = -1;
            return;
        }

        m_currentRoomIndex++;

        if (m_currentRoomIndex > m_rooms.Count)
        {
            m_currentRoomIndex = m_rooms.Count;
        }
    }

    /*
     Devuelve la sala actual.
     Si el indice esta fuera de rango devolvemos null para indicar que no hay sala activa.
    */
    public RoomData CurrentRoom()
    {
        if (m_currentRoomIndex < 0 || m_currentRoomIndex >= m_rooms.Count)
            return null;

        return m_rooms[m_currentRoomIndex];
    }

    // Para acceder a la sala desde cualquier lado
    public RoomData GetCurrentRoom()
    {
        return CurrentRoom();
    }

    /*
     Devuelve true si ya no quedan salas por recorrer.
     Esto ocurre cuando el indice es igual o mayor al numero total de salas.
    */
    public bool IsFinished()
    {
        return m_currentRoomIndex >= m_rooms.Count;
    }

    // Reinicia el progreso dentro del laberinto.

    public void Reset()
    {
        m_currentRoomIndex = -1;
    }

    /*
     Devuelve cuantas salas hay en total dentro del laberinto.
    */
    public int GetRoomCount()
    {
        return m_rooms.Count;
    }
}
