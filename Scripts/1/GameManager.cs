using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private PlayerController m_player;
    private RoomsController m_roomController;
    public UIController m_uiController;
    private LabyrinthData m_labyrith;
    private bool m_gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartGame()
    {
        m_player = new PlayerController (10);
        m_labyrith = new LabyrinthData();

        TrapData trap = new TrapData("Pinchos", -10);
        RoomData room = new RoomData(trap);
        m_labyrith.AddRoom(room);


        m_uiController.EnableGameUI(true);
        m_uiController.EnableStartButtom(false);
        m_uiController.EnableEnemyBar(true);
        m_uiController.EnemyLifeBar(100, 100);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        m_gameOver = false;


    }

    public void ChangeRoom()
    {
        m_labyrith.ChangeRoom();

        if (m_labyrinth.IsFinished())
        {
            Debug.Log("FINISH LABYRINTH");
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);

            //GAME END
            m_roomController.DisableRooms();
            m_gameOver = true;
            return;
        }

        switch (m_labyrith.CurrentRoom().roomType)
        {
            case RoomData.Roomtype.ENEMY:
                m_uiController.EnableEnemyBar(true);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_enemy,null);
                break;
            case RoomData.Roomtype.ITEM:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_item, null);
                break;
            case RoomData.Roomtype.TAP:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_trap, null);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerAttack()
    {
        if (!m_gameOver)
        {

        }

    }
    public void EnemyAttack()
    {
        if (!m_player.IsDied())
        {

        }

    }
}
