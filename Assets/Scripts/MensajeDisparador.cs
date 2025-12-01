using UnityEngine;
using TMPro;

public class MensajeDisparador : MonoBehaviour
{
    public string mensajeEmergente = "¡Mensaje predeterminado! Pulsa Q para ocultar."; // Texto del mensaje
    public GameObject panelMensaje;                     // Panel contenedor de la UI
    public TMP_Text textoMensaje;                       // Componente de texto donde escribir

    public AudioClip clipAudioOpcional;                 // Sonido al mostrar el mensaje
    
    private AudioSource fuenteAudio;                    // Referencia al AudioSource
    private bool mensajeActivo = false;                 // Estado del mensaje (visible o no)
    private bool yaMostradoEnEstaSesion = false;        // Control para mostrarlo solo una vez

    void Start()
    {
        fuenteAudio = GetComponent<AudioSource>();
        
        // Ocultar panel al iniciar
        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
        }

        // Comprobación de seguridad del trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"[ADVERTENCIA] El Collider de '{gameObject.name}' no es Trigger.");
        }
    }

    void Update()
    {
        // Si el mensaje está activo, permite al jugador ocultarlo con Q
        if (mensajeActivo && Input.GetKeyDown(KeyCode.Q))
        {
            OcultarMensaje();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo el jugador y si no se ha mostrado antes
        if (other.CompareTag("Player") && !yaMostradoEnEstaSesion)
        {
            MostrarMensaje();
            
            // Desactivar el collider para evitar repeticiones
            Collider miCollider = GetComponent<Collider>();
            if (miCollider != null)
            {
                miCollider.enabled = false;
            }
        }
    }

    void MostrarMensaje()
    {
        if (panelMensaje == null || textoMensaje == null)
        {
            Debug.LogError("[FALLO UI] Asegura que 'Panel Mensaje' y 'Texto Mensaje' están asignados en el Inspector.");
            return;
        }

        yaMostradoEnEstaSesion = true; // Marcar como visto
        
        // Reproducir audio
        if (clipAudioOpcional != null && fuenteAudio != null)
        {
            fuenteAudio.clip = clipAudioOpcional;
            fuenteAudio.Play(); 
        }
        
        // Configurar texto y mostrar panel
        textoMensaje.text = mensajeEmergente + "\n\n(Pulsa Q para ocultar)";
        panelMensaje.SetActive(true);
        mensajeActivo = true;
    }

    public void OcultarMensaje()
    {
        // Cerrar el panel
        if (panelMensaje != null)
        {
            panelMensaje.SetActive(false);
            mensajeActivo = false;
        }
    }
}