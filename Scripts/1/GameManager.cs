using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerController m_player;
    private RoomsController m_roomController;
    public UIController m_uiController;
    private LabyrinthData m_labyrinth;
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
        m_roomController = GetComponent<RoomsController>();

        m_gameOverScreen = m_uiController.gameOverScreen;
        m_gameOverScreenText = m_uiController.gameOverScreen
            .transform.GetChild(0)
            .GetComponent<TextMeshProUGUI>();
    }

    public void StartGame()
    {
        GetComponent<AudioSource>().Play();

        // CREAMOS EL LABERINTO QUE CONTIENE LAS HABITACIONES
        m_labyrinth = new LabyrinthData();

        int randomNum = Random.Range(3, 6);

        // SE AÑADEN LAS HABITACIONES
        TrapData trap = new TrapData("Pinchos", -10);
        ItemData item = new ItemData("Item 1", 10, 0, 0);
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
                    m_labyrinth.AddRoom(room);
                    break;

                case 1:
                    room = new RoomData(item);
                    m_labyrinth.AddRoom(room);
                    break;

                case 2:
                    room = new RoomData(enemy);
                    m_labyrinth.AddRoom(room);
                    break;
            }
        }

        // Última habitación fija (enemigo, por ejemplo)
        room = new RoomData(enemy);
        m_labyrinth.AddRoom(room);

        // Jugador lógico
        m_player = new PlayerController(10, 2, 100);

        // UI
        m_uiController.EnableGameUI(true);
        m_uiController.EnableStartButtom(false);
        m_uiController.EnableEnemyBar(true);
        m_uiController.EnemyLifeBar(100, 100);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        m_gameOver = false;
        m_points = 0;
        CheckPoints();

        // Arranca el laberinto
        ChangeRoom();
    }

    public void ChangeRoom()
    {
        m_points += 10;
        CheckPoints();

        m_labyrinth.ChangeRoom();

        if (m_labyrinth.IsFinished())
        {
            m_gameOverScreen.SetActive(true);
            m_gameOverScreenText.text = "¡HAS GANADO!";
            Debug.Log("FINISH LABYRINTH");
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);

            if (m_roomController != null)
                m_roomController.DisableRooms();

            m_gameOver = true;

            StartCoroutine(nameof(FinishLabyrinth));
            return;
        }

        RoomData currentRoom = m_labyrinth.CurrentRoom();
        if (currentRoom == null)
            return;

        switch (currentRoom.m_roomtype)
        {
            case RoomData.Roomtype.ENEMY:
                m_uiController.EnableEnemyBar(true);
                m_roomController.SetRoom(
                    currentRoom.m_enemy,
                    m_spriteSelector.GetEnemySpriteByName(currentRoom.m_enemy.m_name)
                );
                break;

            case RoomData.Roomtype.ITEM:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(
                    currentRoom.m_item,
                    m_spriteSelector.GetItemSpriteByName(currentRoom.m_item.m_name)
                );
                break;

            case RoomData.Roomtype.TRAP:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(
                    currentRoom.m_trap,
                    m_spriteSelector.GetTrapSpriteByName(currentRoom.m_trap.m_name)
                );
                break;
        }
    }

    IEnumerator FinishLabyrinth()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        m_gameOverScreen.SetActive(false);
    }

    void Update()
    {
        // Aquí no haces nada de momento
    }

    public void CheckPoints()
    {
        if (pointsText != null)
            pointsText.text = m_points.ToString("0");
    }

    public void PlayerAttack()
    {
        if (m_gameOver)
            return;

        RoomData currentRoom = m_labyrinth.GetCurrentRoom();
        if (currentRoom != null && currentRoom.m_roomtype == RoomData.Roomtype.ENEMY)
        {
            EnemyBehaviour enemyBehaviour =
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>();

            int damageToApply = m_player.Attack(enemyBehaviour.m_enemyData);
            enemyBehaviour.RecieveDamage(damageToApply);

            m_uiController.EnemyLifeBar(
                enemyBehaviour.m_enemyData.m_currentLife,
                enemyBehaviour.m_enemyData.m_maxLife
            );

            if (enemyBehaviour.IsDie())
            {
                Debug.Log("El enemigo ha sido derrotado");
                ChangeRoom();
            }
        }
    }

    public void PlayerStrongAttack()
    {
        if (m_gameOver)
            return;

        RoomData currentRoom = m_labyrinth.GetCurrentRoom();
        if (currentRoom != null && currentRoom.m_roomtype == RoomData.Roomtype.ENEMY)
        {
            EnemyBehaviour enemyBehaviour =
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>();

            int damageToApply = m_player.Attack(enemyBehaviour.m_enemyData) * 2;
            enemyBehaviour.RecieveDamage(damageToApply);

            m_uiController.EnemyLifeBar(
                enemyBehaviour.m_enemyData.m_currentLife,
                enemyBehaviour.m_enemyData.m_maxLife
            );

            if (enemyBehaviour.IsDie())
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

            if (m_roomController != null)
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
