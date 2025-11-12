using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerItemMonoBehavior : MonoBehaviour
{
    public GameManager m_gameManager;

    float m_counterRoomTime;
    float m_timeToChangeRomm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       m_counterRoomTime = 0;
       m_timeToChangeRomm = 1;
    }

    // Update is called once per frame
    void Update()
    {
        m_counterRoomTime += Time.time;

        if (Input.GetMouseButtonDown(0) && m_counterRoomTime > m_timeToChangeRomm)
        {
            //m_gameManager.ApplyItem();
            //m_gameManager.ChangeRoom();
        }
    }
}
