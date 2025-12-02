using UnityEngine;

/*
 Este script sirve para representar objetos que mejoran las estadisticas del jugador
 como vida, dano o defensa, y por supuesto todo lo visual
*/
[System.Serializable]
public class ItemData
{
    public string m_name;      // Nombre del item
    public int m_lifeUp;       // Cuanta vida maxima aumenta
    public int m_damageUp;     // Cuanto dano aumenta
    public int m_defenseUp;    // Cuanta defensa aumenta

    public GameObject m_prefab;    // Prefab visual del item

    // Constructor para crear un item
    public ItemData(string name, int lifeUp, int damageUp, int defenseUp)
    {
        m_name = name;
        m_lifeUp = lifeUp;
        m_damageUp = damageUp;
        m_defenseUp = defenseUp;
    }

    /*
     Esto sirve para que cuando generemos items en distintas salas
     cada uno tenga instancias separadas.
    */
    public ItemData(ItemData other)
    {
        m_name = other.m_name;
        m_lifeUp = other.m_lifeUp;
        m_damageUp = other.m_damageUp;
        m_defenseUp = other.m_defenseUp;
        m_prefab = other.m_prefab;
    }
}
