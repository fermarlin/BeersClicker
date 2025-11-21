using System.Collections.Generic;
using UnityEngine;

public class LabyrinthData
{
    private List<RoomData> m_rooms;
    private int m_currentRoomIndex;

    public LabyrinthData()
    {
        m_rooms = new List<RoomData>();
        m_currentRoomIndex = -1; 
    }

    public void AddRoom(RoomData room)
    {
        if (room == null)
            return;

        room.m_labyrinth = this;
        m_rooms.Add(room);
    }

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


    public RoomData CurrentRoom()
    {
        if (m_currentRoomIndex < 0 || m_currentRoomIndex >= m_rooms.Count)
            return null;

        return m_rooms[m_currentRoomIndex];
    }


    public RoomData GetCurrentRoom()
    {
        return CurrentRoom();
    }

    public bool IsFinished()
    {
        return m_currentRoomIndex >= m_rooms.Count;
    }


    public void Reset()
    {
        m_currentRoomIndex = -1;
    }


    public int GetRoomCount()
    {
        return m_rooms.Count;
    }
}
