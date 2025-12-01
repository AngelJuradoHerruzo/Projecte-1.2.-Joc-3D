using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalMenu : MonoBehaviour
{
    // Texto donde se mostrarán las estadísticas finales del jugador (tiempo, caídas, monedas, etc.)
    public TMP_Text statsText;

    private void Start()
    {
        // Mostrar el cursor del ratón y desbloquearlo al entrar al menú final
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Si existen las referencias necesarias (texto y gestor de estadísticas)
        if (statsText != null && GameStatsManager.Instance != null)
        {
            // Mostrar las estadísticas finales recogidas durante la partida
            statsText.text =
                "- Tiempo total: " + GameStatsManager.Instance.GetTiempoFormateado() + "\n" +
                "- Veces caído: " + GameStatsManager.Instance.caidas + "\n" +
                "- Monedas recogidas: " + GameStatsManager.Instance.monedas + "/" + GameStatsManager.Instance.monedasNecesarias;
        }
    }
            
    // Botón "Reiniciar" → recarga la escena del juego principal
    public void BotonReiniciar()
    {
        SceneManager.LoadScene("Maze_Scene2");  // Carga de nuevo el nivel principal
    }

    // Botón "Menú Principal" → lleva al menú principal del juego
    public void BotonMenuPrincipal()
    {
        SceneManager.LoadScene("Menus");  // Carga la escena del menú principal
    }

    // Botón "Salir" → redirige al menú principal
    public void BotonSalir()
    {
        SceneManager.LoadScene("Menus");
    }
}