using UnityEngine;

public class RoomData
{
    public enum Roomtype { ENEMY, ITEM, TRAP };

    public Roomtype m_roomtype;
    public TrapData m_trap;
    public EnemyData m_enemy;
    public ItemData m_item;
    public LabyrinthData m_labyrinth;

    public RoomData(TrapData trap) 
    {
        m_roomtype = Roomtype.TRAP;
        m_trap = trap;
    }
    public RoomData(EnemyData enemy) 
    {
        m_roomtype = Roomtype.ENEMY;
        m_enemy = enemy;
    }
    public RoomData(ItemData item) 
    {
        m_roomtype = Roomtype.ITEM;
        m_item = item;
    }
}
