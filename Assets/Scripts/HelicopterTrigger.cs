using UnityEngine;
using UnityEngine.SceneManagement;

public class HelicopterTrigger : MonoBehaviour
{
    public GameObject player;

    public WaterKillzone waterKillzone;  // Referencia a las caídas del jugador
    public VendedorNPC vendedorNPC;      // Referencia a las monedas recogidas

    public string finalSceneName = "MenuFinal"; // Escena que se cargará al terminar

    private bool menuMostrado = false; // Evitar que se active más de una vez

    private void OnTriggerEnter(Collider other)
    {
        // Si no es el jugador, salimos
        if (!other.CompareTag("Player")) return;

        // Si ya terminamos, salimos (control de doble ejecución)
        if (menuMostrado) return;
        menuMostrado = true; // Marcamos el evento como activado

        // Si tenemos el gestor global, guardamos las estadísticas
        if (GameStatsManager.Instance != null)
        {
            // Guardar número de caídas
            if (waterKillzone != null)
                GameStatsManager.Instance.caidas = waterKillzone.fallCount;

            // Guardar monedas recogidas y totales
            if (vendedorNPC != null)
            {
                GameStatsManager.Instance.monedas = vendedorNPC.monedasJugador;
                GameStatsManager.Instance.monedasNecesarias = vendedorNPC.MonedasNecesarias;
            }

            // Parar el cronómetro de la partida
            GameStatsManager.Instance.DetenerContador();
        }

        // Cargar la escena de fin de juego
        SceneManager.LoadScene(finalSceneName);
    }
}