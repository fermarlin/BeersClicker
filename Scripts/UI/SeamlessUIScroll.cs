using UnityEngine;
using UnityEngine.UI;

// Este script es para el fondo, y que haga scroll infinito

[RequireComponent(typeof(RawImage))]
public class SeamlessUIScroll : MonoBehaviour
{
    [SerializeField]
    private Vector2 scrollSpeed = new Vector2(0.1f, 0f);  // Velocidad de scroll en X/Y

    private RawImage rawImage;   // Referencia al RawImage al que vamos a mover UV
    private Rect uvRect;         // Rect de UV que vamos actualizando cada frame

    // Guardamos la referencia al RawImage y copiamos su UV inicial.

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
    }

    /*
     Cada frame movemos el UVRect segun la velocidad.
     Mathf.Repeat evita que los valores crezcan demasiado
    */
    void Update()
    {
        uvRect.x += scrollSpeed.x * Time.deltaTime;
        uvRect.y += scrollSpeed.y * Time.deltaTime;

        // Evita overflow y mantiene el bucle infinito limpio
        uvRect.x = Mathf.Repeat(uvRect.x, 1f);
        uvRect.y = Mathf.Repeat(uvRect.y, 1f);

        rawImage.uvRect = uvRect;
    }
}
