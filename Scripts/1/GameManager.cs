using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using NUnit.Framework.Constraints;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private PlayerController m_player;
    private RoomsController m_roomController;
    public UIController m_uiController;
    private LabyrinthData m_labyrith;
    private bool m_gameOver = false;
    private float m_points = 0;
    public TextMeshProUGUI pointsText;
    private GameObject m_gameOverScreen;
    private TextMeshProUGUI m_gameOverScreenText;

    private SpriteSelector m_spriteSelector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_spriteSelector = GetComponent<SpriteSelector>();
        m_gameOverScreen = m_uiController.gameOverScreen;
        m_gameOverScreenText = m_uiController.gameOverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    public void StartGame()
    {
        GetComponent<AudioSource>().Play();

        //CREAMOS EL LABERINTO QUE CONTIENE LAS HABITACIONES
        m_labyrith = new LabyrinthData();

        int randomNum = Random.Range(3, 6);

        //SE AÑADEN LAS HABITACIONES
        TrapData trap = new TrapData("Pinchos", -10);
        ItemData item = new ItemData("Iten 1", 10, 0, 0);
        EnemyData enemy = new EnemyData("Enemy 1", 15, 3, 60, 20, 2, 30);
        RoomData room;

        for (int i = 0; i < randomNum; i++) 
        {
            int randomRoom = Random.Range(0, 3);

            switch (randomRoom)
            {
                default:
                    break;

                case 0:
                    room = new RoomData(trap);
                    //m_labyrith.AddRoom(room);
                    break;

                case 1:
                    room = new RoomData(trap);
                    //m_labyrith.AddRoom(room);
                    break;

                case 2:
                    room = new RoomData(enemy);
                    //m_labyrith.AddRoom(room);
                    break;
            }
        }
        room = new RoomData(enemy);
        //m_labyrith.AddRoom(room);


        m_player = new PlayerController(10, 2, 100);

        m_uiController.EnableGameUI(true);
        m_uiController.EnableStartButtom(false);
        m_uiController.EnableEnemyBar(true);
        m_uiController.EnemyLifeBar(100, 100);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        m_gameOver = false;

        //Arranca el laberinto
        //ChangeRoom();
    }

    public void ChangeRoom()
    {
        m_points += 10;
        CheckPoints();

        /*m_labyrith.ChangeRoom();

        if (m_labyrinth.IsFinished())
        {
            m_gameOverScreen.SetActive(true);
            m_gameOverScreenText.text = "¡HAS GANADO!";
            Debug.Log("FINISH LABYRINTH");
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);

            GAME END
            m_roomController.DisableRooms();
            m_gameOver = true;

            StartCoroutine(nameof(FinishLabytinth));
            return;
        }

        switch (m_labyrith.CurrentRoom().roomType)
        {
            case RoomData.Roomtype.ENEMY:
                m_uiController.EnableEnemyBar(true);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_enemy, m_spriteSelector.GetEnemySpriteByName(m_labyrith.CurrentRoom().m_enemy.m_name));
                break;
            case RoomData.Roomtype.ITEM:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_item, m_spriteSelector.GetItemSpriteByName(m_labyrith.CurrentRoom().m_item.m_name));
                break;
            case RoomData.Roomtype.TAP:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(m_labyrinth.GetCurrentRoom().m_trap, m_spriteSelector.GetTrapSpriteByName(m_labyrith.CurrentRoom().m_trap.m_name));
                break;
            default:
                break;
            }*/
    }

    IEnumerator FinishLabyrinth()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        m_gameOverScreen.SetActive(false);

    }
    // Update is called once per frame
    void Update()
        {

        } 
    public void CheckPoints()
    {
        pointsText.text = m_points.ToString("0");
    }
    public void PlayerAttack()
    {
        if (!m_gameOver)
        {
            /*if (m_labyrith.GetCurrentRoom().m_roomType == RoomData.Roomtype.ENEMY)
            {
                int damageToApply = m_player.Attack(m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData);
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().RecieveDamage(damageToApply);
                m_uiController.EnemyLifeBar(m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData.m_currentLife,
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData.m_maxLife);
            }*/

            if (m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().IsDie())
            {
                Debug.Log("El enemigo ha sido derrotado");
                ChangeRoom();
            }
        }
    }
    public void PlayerStrongAttack()
    {
        if (!m_gameOver)
        {
            /*if (m_labyrith.GetCurrentRoom().m_roomType == RoomData.Roomtype.ENEMY)
            {
                int damageToApply = m_player.Attack(m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData) * 2;
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().RecieveDamage(damageToApply);
                m_uiController.EnemyLifeBar(m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData.m_currentLife,
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().m_enemyData.m_maxLife);
            }*/

            if (m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>().IsDie())
            {
                Debug.Log("El enemigo ha sido derrotado");
                ChangeRoom();
            }
        }
    }
    public void EnemyAttack(int damage)
    {
        m_player.ReceiveDamage(damage);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        if (m_player.IsDied())
        {
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);
            m_gameOverScreen.SetActive(true);
            m_gameOverScreenText.text = "¡HAS PERDIDO!";
            m_roomController.DisableRooms();
            m_gameOver = true;
        }

    }

    public void ApplyTrap(int damage)
    {
        m_player.ReceiveDamage(damage);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());
    }
    public void ApplyItem(int heal)
    {
        m_player.HealDamage(heal);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());
    }

}
