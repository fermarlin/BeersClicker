using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/*
 Aqui se gestiona la consola el ultimo mensaje aparece arriba 
 el resto baja una linea y solo se muestran los ultimos maxLines mensajes.
 Tambien gestiona el color del mensaje mas reciente (blanco) y los anteriores (gris),
 ademas de reproducir un audio asociado al mensaje si se pasa por parametro.

*/
public class TextBoxManager : MonoBehaviour
{
    public static TextBoxManager instance;   // Singleton para poder acceder desde cualquier script

    [SerializeField]
    private TextMeshProUGUI txtBox;          // Texto donde se muestran los mensajes

    [SerializeField]
    private int maxLines = 5;                // Numero maximo de lineas

    [Header("Colores de la consola")]
    [SerializeField]
    private Color lastMessageColor = Color.white;                     // Color del mensaje mas reciente

    [SerializeField]
    private Color previousMessagesColor = new Color(0.8f,0.8f,0.8f,1f); // Color del resto de mensajes

    private Animator textBoxAnimator;         // Animator
    private List<string> messageHistory = new List<string>();  // Historial

    /*
     Inicializamos el singleton, pillo el Animator y activamos RichText
     para poder colorear cada linea de forma independiente.
    */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        textBoxAnimator = GetComponent<Animator>();

        if (txtBox != null)
        {
            txtBox.richText = true;
        }
    }

    /*
     Muestro un mensaje en la consola.
     Se llama a una corutina para sincronizar audio + animacion.
    */
    public void ShowMessage(string text, AudioClip audioText = null)
    {
        StartCoroutine(ChangeTextCoroutine(text, audioText));
    }

    /*
     Esta corutina controla todo, cancela el audio anterior anade el nuevo mensaje al historial 
     activa la animacion y reproduce el audio de mensaje
    */
    public IEnumerator ChangeTextCoroutine(string text, AudioClip audioText)
    {
        SoundManager.instance.AudioCancel();

        AddToHistory(text);

        if (textBoxAnimator != null)
        {
            textBoxAnimator.SetTrigger("MssgActive");
        }

        if (audioText != null)
        {
            SoundManager.instance.PlayAudioClipDefaultPitch(audioText);
            yield return new WaitForSeconds(audioText.length);
        }
    }

    /*
     Anade un mensaje nuevo al historial
     Si superamos maxLines, eliminamos el mas viejo
     Despues reconstruimos el texto aplicando color a cada linea.
    */
    private void AddToHistory(string newMessage)
    {
        if (string.IsNullOrEmpty(newMessage) || txtBox == null)
            return;

        messageHistory.Insert(0, newMessage);

        if (messageHistory.Count > maxLines)
        {
            messageHistory.RemoveAt(messageHistory.Count - 1);
        }

        string lastHex = ColorUtility.ToHtmlStringRGB(lastMessageColor);
        string prevHex = ColorUtility.ToHtmlStringRGB(previousMessagesColor);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < messageHistory.Count; i++)
        {
            if (i > 0)
                sb.Append("\n");

            string hex = (i == 0) ? lastHex : prevHex;

            sb.Append("<color=#");
            sb.Append(hex);
            sb.Append(">");
            sb.Append(messageHistory[i]);
            sb.Append("</color>");
        }

        txtBox.text = sb.ToString();
    }
}
