using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    // Instancia global (patrón Singleton)
    public static GameStatsManager Instance;

    public float tiempoPartida = 0f;    // Tiempo total de la partida
    private bool contando = false;      // Bandera para saber si estamos contando tiempo

    public int caidas = 0;              // Veces que el jugador cayó al agua
    public int monedas = 0;             // Monedas que lleva recogidas
    public int monedasNecesarias = 5;   // Monedas totales que se necesitan

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruye si ya existe otro gestor
        }
    }

    private void Start()
    {
        // Empezar a contar el tiempo al inicio
        IniciarContador();
    }

    private void Update()
    {
        // Si 'contando' es true, sumamos el tiempo de cada frame
        if (contando)
        {
            tiempoPartida += Time.deltaTime; // Suma el tiempo transcurrido
        }
    }

    // Pone el tiempo a cero y activa el contador
    public void IniciarContador()
    {
        tiempoPartida = 0f;
        contando = true;
    }

    // Desactiva el contador de tiempo
    public void DetenerContador()
    {
        contando = false;
    }

    // Devuelve el tiempo como un string formateado "MM:SS"
    public string GetTiempoFormateado()
    {
        int minutos = Mathf.FloorToInt(tiempoPartida / 60f);    // Calcular minutos
        int segundos = Mathf.FloorToInt(tiempoPartida % 60f);   // Calcular segundos restantes
        return $"{minutos:00}:{segundos:00}";                   // Devuelve el formato
    }
}