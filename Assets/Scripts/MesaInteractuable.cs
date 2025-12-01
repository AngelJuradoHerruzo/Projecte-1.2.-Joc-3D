using UnityEngine;

public class MesaInteractuable : MonoBehaviour
{

    public VendedorNPC npcVendedor;             // Referencia al NPC vendedor (para comprobar si la radio fue comprada)
    public GameObject radio;                    // Objeto de la radio (se activará cuando el jugador la coloque)
    public GameObject helicoptero;              // Helicóptero que aparecerá al colocar la radio
    public GameObject PulsaE_Table;             // UI que indica al jugador que puede pulsar "E"


    public Vector3 helicopteroAlturaFinal;      // Posición final del helicóptero al moverse
    public float helicopteroVelocidad = 2f;     // Velocidad de movimiento del helicóptero

    private bool jugadorCerca = false;          // Indica si el jugador está dentro del rango de interacción
    private bool radioColocada = false;         // Indica si la radio ya fue colocada sobre la mesa
    private bool helicopteroActivo = false;     // Indica si el helicóptero ya está activo y moviéndose

    void Start()
    {
        // Desactivar objetos al inicio del juego
        if (radio != null) radio.SetActive(false);
        if (helicoptero != null) helicoptero.SetActive(false);
        if (PulsaE_Table != null) PulsaE_Table.SetActive(false);
    }

    void Update()
    {
        // Mostrar el indicador "Pulsa E" solo si el jugador está cerca y la radio no ha sido colocada aún
        if (PulsaE_Table != null)
            PulsaE_Table.SetActive(jugadorCerca && !radioColocada);

        // Si el jugador está cerca, no ha colocado la radio y pulsa E → intentar colocarla
        if (jugadorCerca && !radioColocada && Input.GetKeyDown(KeyCode.E))
        {
            ColocarRadio();
        }

        // Si el helicóptero está activo, moverlo suavemente hacia su posición final
        if (helicopteroActivo && helicoptero != null)
        {
            helicoptero.transform.position = Vector3.MoveTowards(
                helicoptero.transform.position,             // Posición actual
                helicopteroAlturaFinal,                     // Posición destino
                helicopteroVelocidad * Time.deltaTime       // Movimiento gradual en función del tiempo
            );
        }
    }

    // Método que se ejecuta al pulsar "E" si el jugador tiene la radio comprada
    void ColocarRadio()
    {
        // Comprobar si el jugador ya compró la radio al NPC
        if (npcVendedor != null && npcVendedor.radioComprada)
        {
            // Activar la radio sobre la mesa
            if (radio != null)
                radio.SetActive(true);

            // Activar y empezar el movimiento del helicóptero
            if (helicoptero != null)
            {
                helicoptero.SetActive(true);
                helicopteroActivo = true;
            }

            // Marcar que la radio ya fue colocada
            radioColocada = true;
        }
        else
        {
            // Si no se ha comprado la radio, mostrar mensaje en consola
            Debug.Log("No tienes la radio todavía.");
        }
    }

    // Detectar cuando el jugador entra en el rango del trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    // Detectar cuando el jugador sale del rango del trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            // Ocultar el indicador "Pulsa E" al alejarse del área de interacción
            if (PulsaE_Table != null)
                PulsaE_Table.SetActive(false);
        }
    }
}