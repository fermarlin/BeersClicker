using UnityEngine;

/*
 Este objeto solo guarda informacion basica sobre la trampa,
 su nombre, el dano que hace al jugador y tal
*/
[System.Serializable]
public class TrapData
{
    public string m_name;           // Nombre de la trampa
    public int m_playerDamage;      // Dano directo que hace al jugador al activarse

    public GameObject m_prefab;     // Prefab visual asociado a la trampa

    // Constructor principal para crear una trampa desde cero.
    public TrapData(string name, int playerDamage)
    {
        m_name = name;
        m_playerDamage = playerDamage;
    }

    /*
     Esto ermite que cuando generamos una sala nueva tengamos una instancia propia
     con los mismos datos base, evitando modificar la definicion original.
    */
    public TrapData(TrapData other)
    {
        m_name = other.m_name;
        m_playerDamage = other.m_playerDamage;
        m_prefab = other.m_prefab;
    }
}
