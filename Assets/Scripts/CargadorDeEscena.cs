using UnityEngine;
using UnityEngine.SceneManagement;

public class CargadorDeEscena : MonoBehaviour
{
    // Nombre de la escena que vamos a cargar
    private string nombreDeEscenaACargar = "Bosc_Scene1"; 

    // Indica si el jugador está dentro del área de interacción
    private bool isPlayerInside = false; 

    // La etiqueta que debe tener el personaje (usamos 'Player' por defecto)
    private const string PlayerTag = "Player";
    
    // Objeto UI para mostrar "Pulsa E"
    public GameObject mensajeDeInteraccionUI;

    void Start()
    {
        // Ocultamos el mensaje de interacción al empezar
        if (mensajeDeInteraccionUI != null)
        {
            mensajeDeInteraccionUI.SetActive(false);
        }
    }

    void Update()
    {
        // Chequeamos si estamos dentro Y pulsamos 'E'
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            CargarNuevaEscena();
        }
    }

    // Se activa cuando un objeto entra en el Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra es el jugador
        if (other.CompareTag(PlayerTag))
        {
            isPlayerInside = true;
            // Mostramos el mensaje UI si está asignado
            if (mensajeDeInteraccionUI != null)
            {
                mensajeDeInteraccionUI.SetActive(true);
            }
        }
    }

    // Se activa cuando un objeto sale del Trigger
    private void OnTriggerExit(Collider other)
    {
        // Si el objeto que sale es el jugador
        if (other.CompareTag(PlayerTag))
        {
            isPlayerInside = false;
            // Ocultamos el mensaje de interacción
            if (mensajeDeInteraccionUI != null)
            {
                mensajeDeInteraccionUI.SetActive(false);
            }
        }
    }

    // Método para cargar la escena
    private void CargarNuevaEscena()
    {
        // Si el nombre está vacío, mostramos un error
        if (string.IsNullOrEmpty(nombreDeEscenaACargar))
        {
            Debug.LogError("El nombre de la escena a cargar no puede estar vacío.");
            return;
        }

        Debug.Log($"Cargando escena: {nombreDeEscenaACargar}...");

        try
        {
            // Intentamos cargar la escena por nombre
            SceneManager.LoadScene(nombreDeEscenaACargar);
        }
        catch (System.Exception ex)
        {
            // Capturamos cualquier error (normalmente, si la escena no está en Build Settings)
            Debug.LogError($"Error al intentar cargar la escena '{nombreDeEscenaACargar}'. Asegúrate de que está añadida a 'File -> Build Settings'. Detalle: {ex.Message}");
        }
    }
}