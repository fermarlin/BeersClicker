using UnityEngine;

/*
 Datos de las salas

 Esta clase no contiene logica, simplemente representa que tipo de contenido
 tiene la sala y almacena la referencia a los datos necesarios.
*/
public class RoomData
{
    public enum Roomtype { ENEMY, ITEM, TRAP };

    public Roomtype m_roomtype;      // Tipo de sala
    public TrapData m_trap;          // Datos de la trampa si es TRAP
    public EnemyData m_enemy;        // Datos del enemigo si es ENEMY
    public ItemData m_item;          // Datos del item si es ITEM
    public LabyrinthData m_labyrinth; // Referencia al laberinto que contiene esta sala

    
    // Constructor especifico para crear una sala de trampa.
    
    public RoomData(TrapData trap)
    {
        m_roomtype = Roomtype.TRAP;
        m_trap = trap;
    }

    
    //Constructor para crear una sala de enemigo.
    
    public RoomData(EnemyData enemy)
    {
        m_roomtype = Roomtype.ENEMY;
        m_enemy = enemy;
    }

    
     //Constructor para crear una sala de item.
    
    public RoomData(ItemData item)
    {
        m_roomtype = Roomtype.ITEM;
        m_item = item;
    }
}
