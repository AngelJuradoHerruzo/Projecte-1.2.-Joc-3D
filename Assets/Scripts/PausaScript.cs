using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;                  // Panel del menú de pausa
    private bool isPaused = false;                  // Indica si el juego está actualmente pausado
    private string mainMenuSceneName = "Menus";     // Nombre de la escena del menú principal

    void Update()
    {
        // Detectar la tecla Escape para alternar pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();  // Si ya está pausado, reanudar
            else
                Pause();   // Si no está pausado, pausar
        }
    }

    // Reanuda el juego
    public void Resume()
    {
        pauseMenuUI.SetActive(false);             // Ocultar menú de pausa
        Time.timeScale = 1f;                      // Reanudar el tiempo del juego
        Cursor.visible = false;                   // Ocultar cursor
        Cursor.lockState = CursorLockMode.Locked; // Bloquear cursor en el centro
        isPaused = false;                         // Actualizar estado
    }

    // Pausa el juego
    void Pause()
    {
        pauseMenuUI.SetActive(true);             // Mostrar menú de pausa
        Time.timeScale = 0f;                     // Detener el tiempo del juego
        Cursor.visible = true;                   // Mostrar cursor para interactuar con el menú
        Cursor.lockState = CursorLockMode.None;  // Desbloquear cursor
        isPaused = true;                         // Actualizar estado
    }

    // Cargar la escena del menú principal desde el menú de pausa
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;                     // Asegurarse de que el tiempo esté normalizado
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Salir del juego
    public void QuitGame()
    {
        Debug.Log("Juego cerrado");              // Mensaje en la consola
        Application.Quit();                      // Cierra la aplicación
    }
}