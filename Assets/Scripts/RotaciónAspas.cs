using UnityEngine;
using UnityEngine.SceneManagement;

public class RotacionAspas : MonoBehaviour
{
    [Header("Aspas del helicóptero")]
    public Transform aspaPrincipal;             // Transform del rotor principal
    public Transform aspaCola;                  // Transform del rotor de cola
    public float velocidadPrincipal = 200f;     // Velocidad de rotación (grados/seg) del rotor principal
    public float velocidadCola = 200f;          // Velocidad de rotación (grados/seg) del rotor de cola

    void Update()
    {
        // Rotar el rotor principal en el eje Y si está asignado
        if (aspaPrincipal != null)
            // Multiplicamos por Time.deltaTime para una rotación suave e independiente del frame rate
            aspaPrincipal.Rotate(0, velocidadPrincipal * Time.deltaTime, 0);

        // Rotar el rotor de cola en el eje Y si está asignado
        if (aspaCola != null)
            aspaCola.Rotate(0, velocidadCola * Time.deltaTime, 0);
    }
}