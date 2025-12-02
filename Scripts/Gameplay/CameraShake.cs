using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /*
        Este script se encarga de hacer el efecto de temblor de camara, 
        por ejemplo un golpe fuerte o una explosion. 
        Lo bueno es que funciona de forma generica y se puede llamar desde cualquier otro script 
        sin necesidad de buscar la camara en la escena
    */
    public static CameraShake instance;

    [Header("Valores por defecto (Inspector)")]
    [SerializeField] private float defaultDuration = 0.25f; //Duracion del temblor por defecto
    [SerializeField] private float defaultMagnitude = 0.1f; //Que tan fuerte se mueve la camara
    [SerializeField] private float defaultFrequency = 25f; //Cuantos saltos por segundo
    [SerializeField] private AnimationCurve damping; //Curva para representar como queremos que haga de fuerte la rotacion

    [Header("Rotación")]
    [SerializeField, Range(0f, 10f)] private float rotationMagnitude = 2f; //Cuantos grados puede rotar maximo

    // Variables internas para guardar la posicion y rotacion base de la camara antes del shake
    Vector3 baseLocalPos;
    Quaternion baseLocalRot;

    // Estado actual del shake
    float timeLeft;        //Tiempo restante del temblor
    float totalDuration;   //Duracion total
    float magnitude;       //Fuerza actual
    float frequency;       //Frecuencia actual

    // Semillas para el ruido (sirven para generar movimientos aleatorios y distintos en cada eje)
    float seedR;

    void Awake()
    {
        //Establece que si no hay otra instancia del objeto se convierta el en ella, y si no se destruye, porque solo puede haber una
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //Guardamos la posicion y rotacion inicial de la camara (para volver a ella despues del shake)
        baseLocalPos = transform.localPosition;
        baseLocalRot = transform.localRotation;

        //Creamos las semillas aleatorias para que cada eje tenga un movimiento distinto
        seedR = Random.Range(0f, 1000f);
    }

    void LateUpdate()
    {
        //Solo hacemos el shake si todavia queda tiempo activo
        if (timeLeft > 0f)
        {
            //Calculamos 
            float t = 1f - (timeLeft / totalDuration);

            //Usamos la curva de damping para que el movimiento empiece fuerte y se atenue
            float damper = damping.Evaluate(t);

            //Avanzamos el tiempo interno del ruido para generar movimiento constante
            float time = (totalDuration - timeLeft) * frequency;

            //Aqui rotamos la camara para que se note mas el efecto de golpe
            float nr = (Mathf.PerlinNoise(seedR, time) * 2f - 1f);
            float zRot = nr * rotationMagnitude * damper;
            transform.localRotation = baseLocalRot * Quaternion.Euler(0f, 0f, zRot);
            

            //Reducimos el tiempo restante
            timeLeft -= Time.deltaTime;

            //Si ya ha terminado el shake, devolvemos la camara a su estado original
            if (timeLeft <= 0f)
            {
                transform.localPosition = baseLocalPos;
                transform.localRotation = baseLocalRot;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            }
        }
        else
        {
            //Cuando no hay shake activo, guardamos los valores de la camara por si se ha movido por otras razones, como por ejemplo que el personaje avance de escena
            baseLocalPos = transform.localPosition;
            baseLocalRot = transform.localRotation;
        }
    }


    // Shake con los valores por defecto, para llamarlo desde otros scripts
    public void Shake()
    {
        StartShake(defaultDuration, defaultMagnitude, defaultFrequency);
    }


    private void StartShake(float duration, float mag, float freq)
    {
        //Volvemos a guardar la camara por si se ha movido justo antes de iniciar el shake
        baseLocalPos = transform.localPosition;
        baseLocalRot = transform.localRotation;

        //Guardamos los valores para este nuevo shake
        totalDuration = duration;
        timeLeft = totalDuration;
        magnitude = mag;
        frequency = freq;
    }
}
