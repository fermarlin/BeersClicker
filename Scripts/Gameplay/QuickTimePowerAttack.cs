using UnityEngine;
using UnityEngine.UI;

/*
 Esta es una mecanica nueva que hemos querido meter, un quicktime event
 Cuando aparece, muestra un boton en el Canvas en una posicion aleatoria.
 Si el jugador lo pulsa antes de que acabe el tiempo, se realiza un ataque extra mas fuerte
*/
public class QuickTimePowerAttack : MonoBehaviour
{
    [Header("Referencias")]
    public GameManager gameManager;      // Referencia al GameManager para actualizar vida del enemigo
    public Canvas uiCanvas;              // Canvas donde se instancia el boton
    public GameObject qteButtonPrefab;   // Prefab del boton

    [Header("Parametros QTE")]
    public float buttonLifetime = 3f;    // Tiempo maximo para pulsar
    [Range(0f, 1f)]
    public float damagePercent = 0.25f;  // Porcentaje de dano sobre la vida maxima

    [Header("Probabilidad de aparicion")]
    [Range(0f, 1f)]
    public float spawnChance = 0.2f;     // Probabilidad de que aparezca cuando sea que se intente

    [Header("Posicion aleatoria")]
    public float screenPadding = 50f;    // Margen para que no se quede en los bordes

    private GameObject m_currentButton;  // Instancia actual del boton
    private EnemyBehaviour m_currentEnemy; // Enemigo sobre el que actuara la
    private float m_timer;               // Tiempo transcurrido desde que aparecio
    private bool m_active;               // Si el quick time event esta activo o no

    /*
     En cada frame, si el QTE esta activo, contamos el tiempo.
     Si se agota el tiempo sin pulsar el boton, se cancela.
    */
    void Update()
    {
        if (!m_active)
            return;

        m_timer += Time.deltaTime;

        if (m_timer >= buttonLifetime)
        {
            EndQTE(false);
        }
    }

    /*
     Intenta instanciar una QTE . 
     Solo funciona si hay enemigo y si no hay otra QTE activa.
    */
    public void TrySpawnRandomQTE(EnemyBehaviour enemy)
    {
        if (m_active)
            return;

        if (enemy == null)
            return;

        float r = Random.value;
        if (r <= spawnChance)
        {
            SpawnQTE(enemy);
        }
    }

    /*
     Crea el boton y lo posiciona dentro del canvas.
    */
    public void SpawnQTE(EnemyBehaviour enemy)
    {
        if (m_active)
            return;

        if (enemy == null)
            return;

        if (uiCanvas == null)
        {
            return;
        }

        m_currentEnemy = enemy;
        m_timer = 0f;
        m_active = true;

        m_currentButton = Instantiate(qteButtonPrefab, uiCanvas.transform);

        RectTransform canvasRect = uiCanvas.GetComponent<RectTransform>();
        RectTransform rt = m_currentButton.GetComponent<RectTransform>();

        /*
         Posicion aleatoria dentro del canvas
        */
        if (canvasRect != null && rt != null)
        {
            float halfWidth = canvasRect.rect.width * 0.5f;
            float halfHeight = canvasRect.rect.height * 0.5f;

            float buttonHalfWidth = rt.rect.width * 0.5f;
            float buttonHalfHeight = rt.rect.height * 0.5f;

            float minX = -halfWidth + buttonHalfWidth + screenPadding;
            float maxX =  halfWidth - buttonHalfWidth - screenPadding;
            float minY = -halfHeight + buttonHalfHeight + screenPadding;
            float maxY =  halfHeight - buttonHalfHeight - screenPadding;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            rt.anchoredPosition = new Vector2(randomX, randomY);
        }

        // Subscribimos el boton al evento de click
        Button btn = m_currentButton.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogWarning("El prefab de QTE no tiene componente Button.");
        }
    }

    /*
     Cuando el jugador pulsa el boton de el QTE, aplicamos el dano porcentual
     y terminamos el QTE.
    */
    private void OnButtonClicked()
    {
        if (!m_active)
            return;

        ApplyPercentDamageToEnemy();
        EndQTE(true);
    }

    // Aplica dano y luego actualiza la barra de vida desde el GameManager.
    private void ApplyPercentDamageToEnemy()
    {
        if (m_currentEnemy == null)
            return;

        int maxLife = m_currentEnemy.m_enemyData.m_maxLife;
        int damage = Mathf.RoundToInt(maxLife * damagePercent);

        m_currentEnemy.RecieveDamage(damage);

        if (gameManager != null && gameManager.m_uiController != null)
        {
            gameManager.m_uiController.EnemyLifeBar(
                m_currentEnemy.m_enemyData.m_currentLife,
                m_currentEnemy.m_enemyData.m_maxLife
            );
        }
    }

    /*
     Limpia el QTE tanto si fallo como si acerto.
     Se reinician referencias y se destruye el boton actual.
    */
    private void EndQTE(bool success)
    {
        m_active = false;
        m_timer = 0f;
        m_currentEnemy = null;

        if (m_currentButton != null)
        {
            Destroy(m_currentButton);
            m_currentButton = null;
        }
    }
}
