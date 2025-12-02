using UnityEngine;

/*
Este script es para cerrar la app, lo hice por si al usuario le da por poner
la pantalla completa aunque por defecto este en ventana
*/
public class AppEscape : MonoBehaviour
{
    public void ExitApp()
    {
        Application.Quit();
    }
}
