using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Referencias a los paneles del menú principal y el menú de opciones
    public RectTransform OpcionesCanvas;   
    public RectTransform MainMenuCanvas;

    // Posición fuera de la pantalla para ocultar paneles
    public Vector2 offScreenPos = new Vector2(2000, 0);

    void Start()
    {
        // Al iniciar, mostrar el menú principal y ocultar el menú de opciones
        MainMenuCanvas.anchoredPosition = Vector2.zero;   // Centrar el menú principal
        OpcionesCanvas.anchoredPosition = offScreenPos;   // Mover las opciones fuera de la vista

        // Hacer visible el cursor y desbloquearlo (para usar en los menús)
        Cursor.visible = true;                   
        Cursor.lockState = CursorLockMode.None;  
    }

    // Botón "Jugar" → inicia la partida
    public void IniciarJuego()
    {
        // Indica que el juego se está iniciando (usado posiblemente por MenuLoader)
        MenuLoader.iniciarJuego = true;

        // Cargar la escena seleccionada (por ahora solo "Maze_Scene2")
        int escenaIndex = PlayerPrefs.GetInt("EscenarioSeleccionado", 0);
        string[] escenas = { "Maze_Scene2" };
        SceneManager.LoadScene(escenas[escenaIndex]);

        // Ocultar y bloquear el cursor durante el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Botón "Opciones" → muestra el menú de opciones
    public void AbrirOpciones()
    {
        // Ocultar el menú principal y mostrar el menú de opciones
        MainMenuCanvas.anchoredPosition = offScreenPos;
        OpcionesCanvas.anchoredPosition = Vector2.zero;
    }

    // Botón "Salir" → cierra la aplicación
    public void SalirJuego()
    {
        Application.Quit(); // Cierra el juego (solo funciona en versión compilada)
        Debug.Log("Saliendo del juego..."); // Mensaje visible en el editor de Unity
    }
}