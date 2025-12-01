using UnityEngine;
using TMPro;

public class VendedorNPC : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject PulsaE;                           // Indicador "Pulsa E" para interactuar
    public GameObject FondoMonedas;                     // Fondo del contador de monedas
    public GameObject MonedasEncontradasGO;             // Canvas con fondo + TMP_Text para mostrar monedas

    [Header("Canvases de diálogo")]
    public GameObject TextoIntro;                       // Texto de introducción (primera vez)
    public GameObject TextoNoVenta;                     // Texto cuando no hay suficientes monedas
    public GameObject TextoVenta;                       // Texto al vender la radio

    [Header("Datos del vendedor")]
    public int MonedasNecesarias = 5;                   // Cantidad de monedas necesarias para comprar la radio
    [HideInInspector] public int monedasJugador = 0;    // Monedas actuales del jugador

    private bool jugadorCerca = false;                  // Indica si el jugador está dentro del área de interacción
    private bool primeraVez = true;                     // Para mostrar la introducción solo la primera vez
    public bool radioComprada = false;                  // Indica si la radio ya fue comprada
    private bool introActiva = false;                   // Para controlar si la intro está activa

    [Header("Animación simple")]
    public float BalanceoVelocidad = 1.5f;              // Velocidad de balanceo del NPC
    public float BalanceoAmplitud = 2f;                 // Amplitud del balanceo
    private Vector3 rotacionInicial;                    // Guarda la rotación original del NPC

    private TMP_Text textoMonedas;                      // Referencia al TMP_Text del contador de monedas

    void Start()
    {
        // Guardar la rotación inicial para la animación idle
        rotacionInicial = transform.eulerAngles;

        // Ocultar UI inicial
        if (PulsaE != null) PulsaE.SetActive(false);
        if (FondoMonedas != null) FondoMonedas.SetActive(false);

        if (TextoIntro != null) TextoIntro.SetActive(false);
        if (TextoNoVenta != null) TextoNoVenta.SetActive(false);
        if (TextoVenta != null) TextoVenta.SetActive(false);

        if (MonedasEncontradasGO != null)
        {
            MonedasEncontradasGO.SetActive(false); // No visible hasta que termine la intro
            textoMonedas = MonedasEncontradasGO.GetComponentInChildren<TMP_Text>();
        }

        ActualizarMonedasUI(); // Mostrar el contador inicial
    }

    void Update()
    {
        // Animación idle: balanceo del NPC
        transform.eulerAngles = rotacionInicial + new Vector3(0f, Mathf.Sin(Time.time * BalanceoVelocidad) * BalanceoAmplitud, 0f);

        // Pulsar E para interactuar con el NPC
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Interactuar();
        }

        // Pulsar Q para cerrar la introducción
        if (introActiva && Input.GetKeyDown(KeyCode.Q))
        {
            if (TextoIntro != null) TextoIntro.SetActive(false);
            introActiva = false;

            // Mostrar el contador de monedas tras la intro
            if (MonedasEncontradasGO != null) MonedasEncontradasGO.SetActive(true);
            if (FondoMonedas != null) FondoMonedas.SetActive(true);
        }
    }

    void Interactuar()
    {
        // Ocultar indicador "Pulsa E" al interactuar
        if (PulsaE != null) PulsaE.SetActive(false);

        // Primera interacción: mostrar introducción
        if (primeraVez)
        {
            if (TextoIntro != null) TextoIntro.SetActive(true);
            introActiva = true;
            primeraVez = false;
        }
        // Si no hay suficientes monedas, mostrar mensaje de no venta
        else if (monedasJugador < MonedasNecesarias)
        {
            if (TextoNoVenta != null) TextoNoVenta.SetActive(true);
        }
        // Si hay monedas suficientes y no se ha comprado la radio, realizar venta
        else if (!radioComprada && monedasJugador >= MonedasNecesarias)
        {
            if (TextoVenta != null) TextoVenta.SetActive(true);
            radioComprada = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (PulsaE != null && !introActiva) PulsaE.SetActive(true); // Mostrar "Pulsa E" si no está la intro activa
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            // Ocultar indicador "Pulsa E"
            if (PulsaE != null) PulsaE.SetActive(false);

            // Ocultar textos de interacción
            if (TextoNoVenta != null) TextoNoVenta.SetActive(false);
            if (TextoVenta != null) TextoVenta.SetActive(false);

            // Ocultar contador de monedas solo si la radio ya fue comprada
            if (MonedasEncontradasGO != null && radioComprada)
                MonedasEncontradasGO.SetActive(false);
        }
    }

    // Sumar monedas al jugador y actualizar UI
    public void AgregarMoneda(int cantidad = 1)
    {
        monedasJugador += cantidad;
        ActualizarMonedasUI();
    }

    // Actualizar el texto del contador de monedas
    public void ActualizarMonedasUI()
    {
        if (textoMonedas != null)
            textoMonedas.text = "Monedas encontradas " + monedasJugador + "/" + MonedasNecesarias;
    }
}