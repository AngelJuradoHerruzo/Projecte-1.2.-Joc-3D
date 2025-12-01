using Unity.VisualScripting;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    // Escala o velocidad de rotación del ciclo día/noche
    public int rotationScale = 1;

    void Update()
    {
        // Rota el objeto direccional que representa el sol sobre el eje X en función del tiempo transcurrido.
        // Time.deltaTime garantiza que la rotación sea constante sin importar los FPS.
        transform.Rotate(rotationScale * Time.deltaTime, 0, 0);
    }
}
