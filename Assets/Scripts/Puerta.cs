using UnityEngine;
using UnityEngine.SceneManagement;

    public class Puerta : MonoBehaviour
    {
    // --- Configuración de la Puerta ---
    public string nombreEscena;                             // Nombre de la escena a cargar al cruzar la puerta
    public Vector3 offsetDeSpawn = new Vector3(0, 0, 1);    // Vector de desplazamiento para el punto de spawn (relativo a la posición de la puerta)

    // --- Configuración de Spawn Fijo ---
    public bool usarPosicionFija = false;   // Si es TRUE, el jugador aparecerá en 'posicionFijaDeSpawn', ignorando el offset
    public Vector3 posicionFijaDeSpawn;     // Coordenadas absolutas de spawn si 'usarPosicionFija' es TRUE  

    // --- UI y Estado ---
    public GameObject mensajeInteractuar;   // Objeto de interfaz de usuario que se muestra cuando el jugador está cerca
    private bool jugadorCerca = false;      // Bandera privada que indica si el jugador está dentro del área de interacción (Trigger)

    void Start()
    {
        // Oculta el mensaje UI al inicio
        if (mensajeInteractuar != null)
            mensajeInteractuar.SetActive(false);
    }
    
    void Update()
    {
        // Si el jugador presiona 'E'
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 posicionSpawn;

            // Define el punto de spawn según la configuración
            if (usarPosicionFija)
            {
                posicionSpawn = posicionFijaDeSpawn;
            }
            else
            {
                // Calcula el punto de spawn (puerta + offset)
                posicionSpawn = transform.position + offsetDeSpawn;
            }
            
            // Guarda la posición de spawn y una bandera en PlayerPrefs
            PlayerPrefs.SetInt("VengoDePuerta", 1);
            PlayerPrefs.SetFloat("PlayerX", posicionSpawn.x);
            PlayerPrefs.SetFloat("PlayerY", posicionSpawn.y);
            PlayerPrefs.SetFloat("PlayerZ", posicionSpawn.z);
            PlayerPrefs.Save(); 

            // Carga la escena de destino
            if (!string.IsNullOrEmpty(nombreEscena))
                SceneManager.LoadScene(nombreEscena);
            else
                Debug.LogWarning("No has asignado ninguna escena a la puerta.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta la entrada del jugador
        {
            jugadorCerca = true;
            
            if (mensajeInteractuar != null) // Muestra el mensaje UI
                mensajeInteractuar.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detecta la salida del jugador
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            
            if (mensajeInteractuar != null) // Oculta el mensaje UI
                mensajeInteractuar.SetActive(false);
        }
    }
}