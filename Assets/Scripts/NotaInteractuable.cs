using UnityEngine;
using TMPro;

//DIARIOS DE LA ESCENA INTERIOR
public class ManejadorEntradaDiario : MonoBehaviour
{
    // --- Configuración ---
    public string contenidoEntrada = "Contenido de la nota no asignado."; // El texto de la nota
    public string IDEntradaUnica = "";              // Clave para guardar el estado de lectura
    public bool destruirDespuesDeLeer = false;      // ¿Se destruye el objeto al leerlo?
    
    // --- Audio ---
    public AudioClip sonidoApertura; // Sonido al abrir la nota

    // --- Referencias de UI ---
    public GameObject indicadorInteraccion;     // El indicador "Pulsa E"
    public GameObject panelContenido;           // Contenedor visual de la nota
    public TMP_Text muestraContenido;           // Componente de texto para el contenido

    // --- Estado Interno ---
    private bool jugadorCerca = false;      // ¿El jugador está en el área de interacción?
    private bool panelActivo = false;       // ¿La nota está actualmente abierta?
    private bool haSidoLeido = false;       // Estado de lectura persistente
    private AudioSource audioSource;        // Componente para reproducir el audio

    private const string PREFIJO_LEIDO = "ENTRADA_LEIDA_";   // Prefijo para la clave de PlayerPrefs

    void Awake()
    {
        // Carga el estado de lectura
        CargarEstadoLectura();
    }
 
    void Start()
    {
        // Obtener o añadir el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        // Asegura que el indicador y el panel estén ocultos al inicio
        if (indicadorInteraccion != null) indicadorInteraccion.SetActive(false);
        if (panelContenido != null) panelContenido.SetActive(false);

        // Si ya fue leído y debe desaparecer, lo oculta
        if (haSidoLeido && destruirDespuesDeLeer)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Si el panel está activo, la 'E' (o 'Q') lo cierra.
        if (panelActivo && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q)))
        {
            CerrarEntrada();
        }
        // Abrir la entrada con 'E' si el jugador está cerca y el panel está cerrado
        else if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !panelActivo)
        {
            AbrirEntrada();
        }
    } 

    void AbrirEntrada()
    {
        if (panelContenido != null && muestraContenido != null)
        {
            // Reproducir sonido
            if (sonidoApertura != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoApertura);
            }

            // Asigna el texto y activa el panel
            muestraContenido.text = contenidoEntrada;
            panelContenido.SetActive(true);
            panelActivo = true;

            // Ocultar el indicador y pausar el juego
            if (indicadorInteraccion != null) indicadorInteraccion.SetActive(false);
            Time.timeScale = 0f;

            // Marcar y guardar estado si es la primera vez que se lee
            if (!haSidoLeido)
            {
                haSidoLeido = true;
                GuardarEstadoLectura();
            }
        }
    }

    void CerrarEntrada()
    {
        if (panelContenido != null)
        {
            panelContenido.SetActive(false);
            panelActivo = false;

            // Reanudar el juego
            Time.timeScale = 1f;

            // Volver a mostrar el indicador si el jugador sigue cerca y la nota no desaparece
            if (jugadorCerca && !destruirDespuesDeLeer)
            {
                if (indicadorInteraccion != null) indicadorInteraccion.SetActive(true);
            }
            // Destruir el objeto si fue leído y está configurado para desaparecer
            else if (destruirDespuesDeLeer && haSidoLeido)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto debe ser visible
        bool deberiaSerVisible = !haSidoLeido || !destruirDespuesDeLeer;

        // Si es el jugador y la nota debe estar visible
        if (other.CompareTag("Player") && deberiaSerVisible)
        {
            jugadorCerca = true;
            if (indicadorInteraccion != null && !panelActivo)
            {
                indicadorInteraccion.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            // Oculta el indicador
            if (indicadorInteraccion != null)
            {
                indicadorInteraccion.SetActive(false);
            }
            // Cierra el panel si el jugador se aleja
            if (panelActivo)
            {
                CerrarEntrada();
            }
        }
    }

    // --- Métodos de Persistencia ---
    private void GuardarEstadoLectura()
    {
        // Guarda '1' para indicar que ha sido leído
        PlayerPrefs.SetInt(PREFIJO_LEIDO + IDEntradaUnica, 1);
        PlayerPrefs.Save(); // Guarda en el disco
    }

    private void CargarEstadoLectura()
    {
        // Carga el estado guardado (0 por defecto si no existe)
        haSidoLeido = PlayerPrefs.GetInt(PREFIJO_LEIDO + IDEntradaUnica, 0) == 1;
    }
}