using UnityEngine;

public class CofreInteractuable : MonoBehaviour
{
    public int monedasDentro = 1;                   // Cantidad de monedas que se ganan al abrirlo
    public GameObject PulsaE;                       // El objeto de UI (texto/imagen) que dice "Pulsa E"
    public Transform tapaCofre;                     // El objeto 3D de la tapa que vamos a rotar

    public float anguloAbierto = 70f;               // Ángulo máximo al que girará la tapa
    public float velocidadApertura = 2f;            // Velocidad a la que rota la tapa al abrirse

    public float alcance = 2f;                      // Tamaño del radio para detectar al jugador

    public VendedorNPC NPCVendedor;                 // Script del vendedor donde sumaremos las monedas

    public AudioClip sonidoApertura;                // Archivo de audio que suena al abrir
    private AudioSource audioSource;                // Componente necesario para reproducir el sonido

    private bool jugadorCerca = false;              // Controla si el jugador está dentro del área
    private bool abierto = false;                   // Estado para asegurar que solo se abra una vez
    private bool abriendoTapa = false;              // Activa la animación de rotación en el Update

    void Start()
    {
        // Ocultar el mensaje “Pulsa E” al inicio
        if (PulsaE != null)
            PulsaE.SetActive(false);

        // Comprobar si el objeto tiene un SphereCollider (usado para detectar al jugador)
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc == null)
        {
            // Si no tiene, se crea automáticamente
            sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true; // El collider solo detectará, no colisionará físicamente
        }
        sc.radius = alcance; // Ajustar el radio del collider al alcance definido

        // Crear un AudioSource si el objeto no tiene uno
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Evita que el sonido se reproduzca al inicio
    }

    void Update()
    {
        // Si el jugador está cerca, el cofre no está abierto y se pulsa “E” → abrir cofre
        if (jugadorCerca && !abierto && Input.GetKeyDown(KeyCode.E))
        {
            AbrirCofre();
        }

        // Si la tapa se está abriendo, rotarla progresivamente hasta alcanzar el ángulo deseado
        if (abriendoTapa && tapaCofre != null)
        {
            tapaCofre.localRotation = Quaternion.RotateTowards(
                tapaCofre.localRotation,
                Quaternion.Euler(-anguloAbierto, 0, 0),     // Rotación final de apertura
                velocidadApertura * Time.deltaTime * 100f   // Control de velocidad de apertura
            );

            // Cuando la tapa llega a su posición final, detener la animación
            if (Quaternion.Angle(tapaCofre.localRotation, Quaternion.Euler(-anguloAbierto, 0, 0)) < 0.1f)
                abriendoTapa = false;
        }
    }

    void AbrirCofre()
    {
        abierto = true; // Marcar el cofre como abierto

        // Añadir las monedas al NPC vendedor, si hay referencia
        if (NPCVendedor != null)
            NPCVendedor.AgregarMoneda(monedasDentro);

        // Reproducir sonido de apertura, si existe
        if (sonidoApertura != null && audioSource != null)
            audioSource.PlayOneShot(sonidoApertura);

        // Ocultar el mensaje de “Pulsa E”
        if (PulsaE != null)
            PulsaE.SetActive(false);

        // Iniciar la animación de apertura de la tapa
        if (tapaCofre != null)
            abriendoTapa = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el cofre ya está abierto, no hacer nada
        if (abierto) return;

        // Detectar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true; // Marcar que el jugador está cerca

            // Mostrar el mensaje “Pulsa E”
            if (PulsaE != null)
                PulsaE.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detectar si el jugador se aleja del cofre
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false; // El jugador ya no está cerca

            // Ocultar el mensaje “Pulsa E”
            if (PulsaE != null)
                PulsaE.SetActive(false);
        }
    }
}