using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform OpcionesCanvas;    // Panel de opciones
    public RectTransform MainMenuCanvas;    // Panel del menú principal
    public Toggle toggleMusica;             // Toggle para activar/desactivar música

    public Vector2 offScreenPos = new Vector2(2000, 0); // Posición fuera de pantalla para ocultar paneles

    void Start()
    {
        // Inicializar toggle según si la música está reproduciéndose
        if (toggleMusica != null && MusicaAmbientalPersistente.AudioSource != null)
        {
            toggleMusica.isOn = MusicaAmbientalPersistente.AudioSource.isPlaying;

            // Agregar listener para que cambie la música al pulsar
            toggleMusica.onValueChanged.AddListener(ToggleMusica);
        }

        // Mostrar menú principal y ocultar panel de opciones al iniciar
        MainMenuCanvas.anchoredPosition = Vector2.zero;
        OpcionesCanvas.anchoredPosition = offScreenPos;
    }

    // Abrir panel de opciones y ocultar menú principal
    public void AbrirOpciones()
    {
        MainMenuCanvas.anchoredPosition = offScreenPos;
        OpcionesCanvas.anchoredPosition = Vector2.zero;
    }

    // Cerrar panel de opciones y mostrar menú principal
    public void CerrarOpciones()
    {
        OpcionesCanvas.anchoredPosition = offScreenPos;
        MainMenuCanvas.anchoredPosition = Vector2.zero;
    }

    // Cambiar el estado de la música según el toggle
    public void ToggleMusica(bool activo)
    {
        if (MusicaAmbientalPersistente.AudioSource != null)
        {
            if (activo)
                MusicaAmbientalPersistente.AudioSource.UnPause(); // Reanuda la música si estaba pausada
            else
                MusicaAmbientalPersistente.AudioSource.Pause();   // Pausa la música
        }
    }
}