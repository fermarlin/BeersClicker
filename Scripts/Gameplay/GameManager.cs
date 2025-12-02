using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 Este script controla absolutamente TODO el juego, desde generar el laberinto,
 mover al jugador de sala en sala, ataques, items, trampas,
 actualizar la UI, reproducir sonidos y manejar el estado de victoria/derrota.

 Al final es el nucleo del juego y actua como "director de orquesta" coordinando a los
 otros scripts
*/
public class GameManager : MonoBehaviour
{
    private PlayerController m_player;            // Jugador
    private RoomsController m_roomController;     // Controlador visual de salas
    public UIController m_uiController;           // UI del juego
    private LabyrinthData m_labyrinth;            // Datos del laberinto
    private bool m_gameOver = false;              // Estado actual
    private float m_points = 0;                   // Puntos acumulados en partida

    public TextMeshProUGUI pointsText;            // Texto donde se muestran los puntos
    private GameObject m_gameOverScreen;          // Pantalla final
    private TextMeshProUGUI m_gameOverScreenText; // Texto dentro de la pantalla final

    public int maxRooms = 6;
    public int minRooms = 3;

    public AudioClip playerHitAudio;
    public AudioClip playerHeavyHitAudio;

    [SerializeField] private List<EnemyData> m_enemyDefinitions;  // Lista de enemigos 
    [SerializeField] private List<TrapData> m_trapDefinitions;    // Lista de trampas 
    [SerializeField] private List<ItemData> m_itemDefinitions;    // Lista de items 

    [Header("Audios mensajes")]
    public AudioClip enemyAppearClip;
    public AudioClip enemyDefeatedClip;
    public AudioClip trapAppearClip;
    public AudioClip itemAppearClip;
    public AudioClip playerReceiveDamageClip;
    public AudioClip playerDealDamageClip;

    [Header("QTE Ataque Potente")]
    public QuickTimePowerAttack qteManager;   // Referencia al script de Quick time events

    [Header("Mejoras")]
    public int healPrice = 30;                // Coste de curacion
    public int healAmount = 20;               // Vida recuperada
    public int damageUpgradePrice = 50;       // Coste de subir dano
    public float damageBonusPerLevel = 0.25f; // +25% dano por nivel de mejora

    private int m_damageUpgradeLevel = 0;     // Niveles totales

    [Header("Fondos")]
    [SerializeField] private RawImage fondoSala;
    [SerializeField] private Image sueloSala;

    [Header("Colores de las sala")]
    public Color enemyColor = Color.red;
    public Color trapColor = new Color(0.5f, 0f, 0.5f, 1f);
    public Color itemColor = Color.green;
    public Color winColor = Color.green;
    public Color loseColor = Color.red;

    public TextMeshProUGUI attackText;         //Para representar el ataque
    public TextMeshProUGUI defenseText;        //Para representar la defensa

    //  Inicializa UI y las salas.
    void Start()
    {
        m_roomController = GetComponent<RoomsController>();

        m_gameOverScreen = m_uiController.gameOverScreen;
        m_gameOverScreenText = m_uiController.gameOverScreen
            .transform.GetChild(0)
            .GetComponent<TextMeshProUGUI>();
    }

    public bool IsGameOver()
    {
        return m_gameOver;
    }

    // Calcula el multiplicador total de dano segun mejoras
    private float GetDamageMultiplier()
    {
        return 1f + m_damageUpgradeLevel * damageBonusPerLevel;
    }

    /*
     Inicia una partida nueva
     Genera el laberinto aleatorio, resetea jugador y las posibles mejoras, 
     activa UI y muestra mensaje inicial
    */
    public void StartGame()
    {
        m_gameOverScreen.SetActive(false);

        m_labyrinth = new LabyrinthData();
        int randomNum = Random.Range(minRooms, maxRooms);

        // Generamos varias salas aleatorias (trampa, item o enemigo, segun random).
        RoomData room;
        for (int i = 0; i < randomNum; i++)
        {
            int randomRoom = Random.Range(0, 3);

            switch (randomRoom)
            {
                case 0: room = new RoomData(GetRandomTrap()); break;
                case 1: room = new RoomData(GetRandomItem()); break;
                default: room = new RoomData(GetRandomEnemy()); break;
            }

            m_labyrinth.AddRoom(room);
        }

        room = new RoomData(GetRandomEnemy());
        m_labyrinth.AddRoom(room);

        // Jugador
        m_player = new PlayerController(10, 2, 100);

        // Reseteamos mejoras
        m_damageUpgradeLevel = 0;

        // UI inicial
        m_uiController.EnableGameUI(true);
        m_uiController.EnableStartButtom(false);
        m_uiController.EnableEnemyBar(true);
        m_uiController.EnemyLifeBar(100, 100);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        m_gameOver = false;
        m_points = 0;
        CheckPoints();

        ShowGameMessage("C:/SISTEMA> reparar_sistema.exe");

        RefreshAttackUI();
        RefreshDefenseUI();

        ChangeRoom();
    }

    // Muestra un mensaje en la consola UI
    private void ShowGameMessage(string message, AudioClip clip = null)
    {
        if (TextBoxManager.instance != null)
            TextBoxManager.instance.ShowMessage(message, clip);
    }

    // Cambia el color del fondo segun tipo de sala.
    private void SetRoomTypeBackground(RoomData.Roomtype roomType)
    {
        Color chosen = Color.white;

        switch (roomType)
        {
            case RoomData.Roomtype.ENEMY: chosen = enemyColor; break;
            case RoomData.Roomtype.TRAP:  chosen = trapColor; break;
            case RoomData.Roomtype.ITEM:  chosen = itemColor; break;
        }

        if (fondoSala != null) fondoSala.color = chosen;
        if (sueloSala != null) sueloSala.color = chosen;
    }

    // Colores del fondo al ganar o perder.
    private void SetEndGameBackground(bool win)
    {
        Color chosen = win ? winColor : loseColor;

        if (fondoSala != null) fondoSala.color = chosen;
        if (sueloSala != null) sueloSala.color = chosen;
    }

    /*
     Avanza a la siguiente sala del laberinto, suma los puntos,
     comprueba si acabo el laberinto y activa sala correspondiente en RoomsController
    */
    public void ChangeRoom()
    {
        if (m_labyrinth == null) return;

        if (m_roomController == null)
            m_roomController = GetComponent<RoomsController>();

        // Damos puntos por avanzar
        m_points += 10;
        CheckPoints();

        m_labyrinth.ChangeRoom();

        // Si hemos terminado el laberinto, pantalla final
        if (m_labyrinth.IsFinished())
        {
            m_gameOverScreen.SetActive(true);
            m_gameOverScreenText.text = "ORDENADOR ARREGLADO!";
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);

            if (m_roomController != null)
                m_roomController.DisableRooms();

            m_gameOver = true;

            SetEndGameBackground(true);

            StartCoroutine(FinishLabyrinth());
            return;
        }

        RoomData currentRoom = m_labyrinth.CurrentRoom();
        if (currentRoom == null) return;

        SetRoomTypeBackground(currentRoom.m_roomtype);

        /* Segun tipo de sala cargamos enemigo y activamos barra,
        aplicamos prefab de item o aplicamos prefab de trampa
        */
        switch (currentRoom.m_roomtype)
        {
            case RoomData.Roomtype.ENEMY:
                m_uiController.EnableEnemyBar(true);
                m_roomController.SetRoom(currentRoom.m_enemy);
                ShowGameMessage($"Un enemigo aparece: {currentRoom.m_enemy.m_name}!", enemyAppearClip);
                break;

            case RoomData.Roomtype.ITEM:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(currentRoom.m_item);
                ShowGameMessage($"Has encontrado un objeto: {currentRoom.m_item.m_name}.", itemAppearClip);
                break;

            case RoomData.Roomtype.TRAP:
                m_uiController.EnableEnemyBar(false);
                m_roomController.SetRoom(currentRoom.m_trap);
                ShowGameMessage($"Cuidado! Trampa detectada: {currentRoom.m_trap.m_name}.", trapAppearClip);

                PlayerTrapInputBehaviour trapInput = FindAnyObjectByType<PlayerTrapInputBehaviour>();
                if (trapInput != null) trapInput.PrepareForNewTrap();
                break;
        }

        RefreshAttackUI();
        RefreshDefenseUI();
    }

    // Oculta la pantalla de final despues de un pequeno tiempo.
    IEnumerator FinishLabyrinth()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        m_gameOverScreen.SetActive(false);
    }

    // Actualiza el texto de puntos en la UI.
    public void CheckPoints()
    {
        if (pointsText != null)
            pointsText.text = m_points.ToString("0");
    }

    // Actualiza la UI para que el jugador pueda ver cuanto dano hace
    public void RefreshAttackUI(string format = "Daño actual: {DMG}")
    {
        if (attackText == null || m_player == null)
            return;

        int baseDamage = m_player.GetAttack();
        int finalDamage = Mathf.RoundToInt(baseDamage * GetDamageMultiplier());

        string result = format
            .Replace("{DMG}", finalDamage.ToString())
            .Replace("{DMG_BASE}", baseDamage.ToString());

        attackText.text = result;
    }

    // Actualiza la UI para que el jugador pueda ver cuanta defensa tiene

    public void RefreshDefenseUI(string format = "Defensa: {DEF}")
    {
        if (defenseText == null || m_player == null)
            return;

        int defense = m_player.GetDefense();
        string result = format.Replace("{DEF}", defense.ToString());
        defenseText.text = result;
    }


    /*
    Aqui se gestiona el ataque del jugador, este le aplica dano al enemigo
    actualiza la barra de vida y avanza de sala si el enemigo muere
    */
    public void PlayerAttack()
    {
        if (m_gameOver) return;

        RoomData currentRoom = m_labyrinth.GetCurrentRoom();
        if (currentRoom != null && currentRoom.m_roomtype == RoomData.Roomtype.ENEMY)
        {
            EnemyBehaviour enemyBehaviour =
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>();

            int baseDamage = m_player.Attack(enemyBehaviour.m_enemyData);
            int damageToApply = Mathf.RoundToInt(baseDamage * GetDamageMultiplier());

            enemyBehaviour.RecieveDamage(damageToApply);

            if (playerHitAudio != null)
                SoundManager.instance.PlayAudioClip(playerHitAudio);

            ShowGameMessage($"Has infligido {damageToApply} puntos de daño.", playerDealDamageClip);

            m_uiController.EnemyLifeBar(
                enemyBehaviour.m_enemyData.m_currentLife,
                enemyBehaviour.m_enemyData.m_maxLife
            );

            // Enemigo derrotado
            if (enemyBehaviour.IsDie())
            {
                ShowGameMessage("El enemigo ha sido derrotado!", enemyDefeatedClip);
                m_roomController.DestroyCurrentEnemy();
                ChangeRoom();
            }
            else
            {
                if (qteManager != null)
                    qteManager.TrySpawnRandomQTE(enemyBehaviour);
            }
        }
    }

    /*
     Ataque fuerte
     Misma logica que PlayerAttack pero con mas dano
    */
    public void PlayerStrongAttack()
    {
        if (m_gameOver) return;

        RoomData currentRoom = m_labyrinth.GetCurrentRoom();
        if (currentRoom != null && currentRoom.m_roomtype == RoomData.Roomtype.ENEMY)
        {
            EnemyBehaviour enemyBehaviour =
                m_roomController.GetEnemyRoom().GetComponent<EnemyBehaviour>();

            int baseDamage = m_player.Attack(enemyBehaviour.m_enemyData) * 2;
            int damageToApply = Mathf.RoundToInt(baseDamage * GetDamageMultiplier());

            enemyBehaviour.RecieveDamage(damageToApply);

            if (playerHeavyHitAudio != null)
                SoundManager.instance.PlayAudioClip(playerHeavyHitAudio);

            ShowGameMessage($"Golpe critico! Haces {damageToApply} puntos de daño.", playerDealDamageClip);

            m_uiController.EnemyLifeBar(
                enemyBehaviour.m_enemyData.m_currentLife,
                enemyBehaviour.m_enemyData.m_maxLife
            );

            if (enemyBehaviour.IsDie())
            {
                ShowGameMessage("El enemigo ha sido derrotado!", enemyDefeatedClip);
                m_roomController.DestroyCurrentEnemy();
                ChangeRoom();
            }
            else
            {
                if (qteManager != null)
                    qteManager.TrySpawnRandomQTE(enemyBehaviour);
            }
        }
    }

    /*
     Aqui se gestiona los ataques del enemigo, te resta vida,
     actualiza barra de vida y comprueba si te ha matado
    */
    public void EnemyAttack(int damage)
    {
        int finalDamage = m_player.ReceiveDamage(damage);

        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());
        CameraShake.instance.Shake();

        ShowGameMessage($"Has recibido {finalDamage} puntos de daño.", playerReceiveDamageClip);

        if (m_player.IsDied())
        {
            m_uiController.EnableGameUI(false);
            m_uiController.EnableStartButtom(true);

            m_gameOverScreen.SetActive(true);
            m_gameOverScreenText.text = "OH NO EL PC SE HA ROTO!";

            if (m_roomController != null)
                m_roomController.DisableRooms();

            m_gameOver = true;

            SetEndGameBackground(false);
        }
    }

    // Para obtener un enemigo random de los establecidos
    private EnemyData GetRandomEnemy()
    {
        if (m_enemyDefinitions == null || m_enemyDefinitions.Count == 0)
            return null;

        int index = Random.Range(0, m_enemyDefinitions.Count);
        return new EnemyData(m_enemyDefinitions[index]);
    }

    // Para obtener una trampa random de las establecidas
    private TrapData GetRandomTrap()
    {
        if (m_trapDefinitions == null || m_trapDefinitions.Count == 0)
            return null;

        int index = Random.Range(0, m_trapDefinitions.Count);
        return new TrapData(m_trapDefinitions[index]);
    }

    // Para obtener un objeto random de los establecidos
    private ItemData GetRandomItem()
    {
        if (m_itemDefinitions == null || m_itemDefinitions.Count == 0)
            return null;

        int index = Random.Range(0, m_itemDefinitions.Count);
        return new ItemData(m_itemDefinitions[index]);
    }


    // Aplica el dano de una trampa al jugador.
    public void ApplyTrap(int damage)
    {
        int finalDamage = m_player.ReceiveDamage(damage);

        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        ShowGameMessage($"La trampa te causa {finalDamage} puntos de daño.", playerReceiveDamageClip);
    }


    /*
     Aplica un item que puede curar vida
     aumentar dano o aumentar defensa
     El mensaje se construye segun los valores aplicados.
    */
    public void ApplyItem(int heal, int damageBonus, int defenseBonus)
    {
        if (m_player == null) return;

        bool changedSomething = false;

        if (heal != 0) { m_player.HealDamage(heal); changedSomething = true; }
        if (damageBonus != 0) { m_player.AddDamage(damageBonus); changedSomething = true; }
        if (defenseBonus != 0) { m_player.AddDefense(defenseBonus); changedSomething = true; }

        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        RefreshAttackUI();
        RefreshDefenseUI();

        if (changedSomething)
        {
            string msg = BuildItemEffectMessage(heal, damageBonus, defenseBonus);
            ShowGameMessage(msg);
        }
        else
        {
            ShowGameMessage("El objeto no ha tenido ningun efecto apreciable.");
        }
    }

    /*
     Construye mensaje adaptado al item aplicado.
     Para que el jugador vea si se ha curado o han mejorado sus stats
    */
    private string BuildItemEffectMessage(int heal, int damageBonus, int defenseBonus)
    {
        List<string> parts = new List<string>();

        if (heal != 0) parts.Add($"recuperas {heal} puntos de vida");
        if (damageBonus != 0) parts.Add($"tu daño aumenta en {damageBonus} puntos");
        if (defenseBonus != 0) parts.Add($"tu defensa aumenta en {defenseBonus} puntos");

        if (parts.Count == 0)
            return "El objeto no ha tenido ningun efecto.";

        string sentence = parts[0];
        for (int i = 1; i < parts.Count; i++)
        {
            if (i == parts.Count - 1) sentence += " y " + parts[i];
            else sentence += ", " + parts[i];
        }

        if (!string.IsNullOrEmpty(sentence))
            sentence = char.ToUpper(sentence[0]) + sentence.Substring(1) + ".";

        return sentence;
    }

    //Compra de vida, lo tipico, revisa si tienes suficiente dinero y si es asi te curas
    public void BuyHeal()
    {
        if (m_gameOver || m_player == null)
            return;

        if (m_points < healPrice)
        {
            ShowGameMessage($"C:/SISTEMA> reparar_sistema.exe /modo:CURA /coste:{healPrice}\n[ERROR] Puntos insuficientes.");
            return;
        }

        m_points -= healPrice;
        CheckPoints();

        m_player.HealDamage(healAmount);
        m_uiController.PlayerLifeBar(m_player.GetCurrentLife(), m_player.GetMaxLife());

        RefreshAttackUI();
        RefreshDefenseUI();

        ShowGameMessage(
            $"C:/SISTEMA> reparar_sistema.exe /modo:CURA /coste:{healPrice}\n[OK] Vida restaurada +{healAmount}."
        );
    }

    public RoomData GetCurrentRoom()
    {
        if (m_labyrinth == null)
            return null;

        return m_labyrinth.GetCurrentRoom();
    }

    // Igual que la curacion de vida pero con las mejoras de dano
    public void BuyDamageUpgrade()
    {
        if (m_gameOver || m_player == null)
            return;

        if (m_points < damageUpgradePrice)
        {
            ShowGameMessage($"C:/SISTEMA> optimizar_sistema.exe /modo:AUMENTAR_DANO /coste:{damageUpgradePrice}\n[ERROR] Puntos insuficientes.");
            return;
        }

        m_points -= damageUpgradePrice;
        CheckPoints();

        m_damageUpgradeLevel++;

        float totalBonusPercent = (GetDamageMultiplier() - 1f) * 100f;

        RefreshAttackUI();

        ShowGameMessage(
            $"C:/SISTEMA> optimizar_sistema.exe /modo:AUMENTAR_DANO /coste:{damageUpgradePrice}\n[OK] Dano aumentado. Bonus total: {totalBonusPercent:0}%."
        );
    }
}
