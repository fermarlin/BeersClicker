using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpriteSelector : MonoBehaviour
{
    public List<SpriteNameTupla> m_enemySpriteTupla;
    public List<SpriteNameTupla> m_itemSpriteTupla;
    public List<SpriteNameTupla> m_trapSpriteTupla;

    public Sprite GetEnemySpriteByName(string name)
    {
        for (int i = 0; i < m_enemySpriteTupla.Count; i++)
        {
            if (m_enemySpriteTupla[i].m_name.Equals(name))
            {
                return m_enemySpriteTupla[i].m_sprite;
            }
        }

        return null;
    }

    public Sprite GetTrapSpriteByName(string name)
    {
        for (int i = 0; i < m_trapSpriteTupla.Count; i++)
        {
            if (m_trapSpriteTupla[i].m_name.Equals(name))
            {
                return m_trapSpriteTupla[i].m_sprite;
            }
        }

        return null;
    }

    public Sprite GetItemSpriteByName(string name)
    {
        for (int i = 0; i < m_itemSpriteTupla.Count; i++)
        {
            if (m_itemSpriteTupla[i].m_name.Equals(name))
            {
                return m_itemSpriteTupla[i].m_sprite;
            }
        }

        return null;
    }
}