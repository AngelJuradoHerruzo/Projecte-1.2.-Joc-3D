using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenuLoader : MonoBehaviour
{
    // Instancia única del gestor (patrón Singleton)
    public static MenuLoader instance;

    // Indica si el juego se está iniciando desde el menú
    public static bool iniciarJuego = false;

    [Header("UI Menú")]
    public RectTransform MainMenuPanel;                     // Panel del menú principal
    public RectTransform OpcionesPanel;                     // Panel del menú de opciones
    public Vector2 offScreenPos = new Vector2(2000, 0);     // Posición fuera de pantalla para ocultar paneles

    [Header("Datos del jugador")]
    public int monedasJugador = 0;                              // Total de monedas del jugador
    public bool radioComprada = false;                          // Indica si el jugador compró la radio
    public List<string> cofresAbiertos = new List<string>();    // Cofres ya abiertos (por ID)

    //Monedas recogidas específicas por escena (evita duplicados al volver a entrar)
    private HashSet<string> monedasRecogidas = new HashSet<string>();

    void Awake()
    {
        // Implementación del patrón Singleton para que solo haya una instancia global
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados
            return;
        }

        // Si el juego ya fue iniciado desde el menú, no ejecutar la carga inicial
        if (iniciarJuego) return;
    }

    void Start()
    {
        // Si el objeto está en una escena diferente a "Menus", redirige automáticamente al menú
        if (SceneManager.GetActiveScene().name != "Menus")
        {
            StartCoroutine(IrAMenus());
        }
        else
        {
            // Configuración inicial de los paneles del menú
            MainMenuPanel.anchoredPosition = Vector2.zero;
            OpcionesPanel.anchoredPosition = offScreenPos;

            // Mostrar cursor (para interactuar con el menú)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Corrutina que redirige al menú principal
    System.Collections.IEnumerator IrAMenus()
    {
        yield return null;
        SceneManager.LoadScene("Menus");
    }

    // ------------------------------------------
    //  FUNCIONES GLOBALES DE ESTADO DEL JUGADOR
    // ------------------------------------------

    // Añadir monedas al contador global
    public void AgregarMonedas(int cantidad)
    {
        monedasJugador += cantidad;
        Debug.Log("Monedas globales: " + monedasJugador);
    }

    // Intentar gastar monedas, devuelve true si se ha podido pagar
    public bool GastarMonedas(int cantidad)
    {
        if (monedasJugador >= cantidad)
        {
            monedasJugador -= cantidad;
            return true;
        }
        return false;
    }

    // Marca la radio como comprada
    public void ComprarRadio()
    {
        radioComprada = true;
        Debug.Log("Radio comprada");
    }

    // Registra un cofre como abierto (usando un ID único)
    public void CofreAbierto(string idCofre)
    {
        if (!cofresAbiertos.Contains(idCofre))
            cofresAbiertos.Add(idCofre);
    }

    // Comprueba si un cofre ya fue abierto previamente
    public bool EstaCofreAbierto(string idCofre)
    {
        return cofresAbiertos.Contains(idCofre);
    }

    // Registra una moneda recogida (usa un ID único por escena o por objeto)
    public void RegistrarMonedaRecogida(string idMoneda)
    {
        // Solo agregar si no estaba registrada (evita duplicar monedas al recargar escena)
        if (!monedasRecogidas.Contains(idMoneda))
        {
            monedasRecogidas.Add(idMoneda);
            AgregarMonedas(1);
        }
    }

    // Comprueba si una moneda ya fue recogida
    public bool EstaMonedaRecogida(string idMoneda)
    {
        return monedasRecogidas.Contains(idMoneda);
    }
}