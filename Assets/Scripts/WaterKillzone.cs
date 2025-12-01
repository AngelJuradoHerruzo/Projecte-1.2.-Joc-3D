using UnityEngine;
using System.Reflection;

public class WaterKillzone : MonoBehaviour
{
    public GameObject player;
    public GameObject chest;
    
    public Vector3 defaultTeleportPosition = new Vector3(68.29f, 1.041f, 5.24f); // Posición de respawn inicial
    public Vector3 chestOpenedTeleportPosition = new Vector3(80f, 1f, 10f);      // Posición de respawn tras abrir el cofre   
    public int fallCount = 0;       // Cuenta las caídas
    public AudioClip splashClip;    // Sonido de impacto

    private AudioSource audioSource;   // Reproductor de sonido
    private bool chestOpened = false;  // Bandera para la posición

    private void Start()
    {
        // Crear AudioSource dinámicamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = splashClip;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.2f;

        // Advertencias si faltan referencias
        if (player == null)
            Debug.LogWarning("No se ha asignado el jugador en WaterKillzone!");
        if (chest == null)
            Debug.LogWarning("No se ha asignado el cofre (ChestV1 (2)) en WaterKillzone!");
    }

    private void Update()
    {
        // Chequea si el cofre ha sido abierto (solo una vez)
        if (!chestOpened && chest != null)
        {
            CofreInteractuable cofreScript = chest.GetComponent<CofreInteractuable>();
            if (cofreScript != null)
            {
                // **Reflection**: Acceso al campo privado 'abierto'
                FieldInfo abiertoField = typeof(CofreInteractuable)
                    .GetField("abierto", BindingFlags.NonPublic | BindingFlags.Instance);

                if (abiertoField != null)
                {
                    // Lee el valor del campo
                    bool abierto = (bool)abiertoField.GetValue(cofreScript);
                    if (abierto)
                    {
                        chestOpened = true; // Activa nueva posición de respawn
                        Debug.Log("✅ El cofre ha sido abierto — nueva zona de teletransporte activada.");
                    }
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Asegura que la colisión sea con el jugador
        if (player == null || other.gameObject != player) return;

        Debug.Log("Caída número = " + fallCount);

        // Selecciona la posición de teletransporte
        Vector3 targetPosition = chestOpened ? chestOpenedTeleportPosition : defaultTeleportPosition;

        // Gestión del teletransporte
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            // Si tiene CharacterController: deshabilitar, mover, rehabilitar
            cc.enabled = false;
            player.transform.position = targetPosition;
            cc.enabled = true;
        }
        else
        {
            // Si no tiene CC: intentar mover con Rigidbody...
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.MovePosition(targetPosition);
            // ...o mover con Transform (opción básica)
            else
                player.transform.position = targetPosition;
        }

        fallCount++; // Incrementa contador

        // Reproduce el sonido de splash
        if (audioSource != null && splashClip != null)
        {
            audioSource.PlayOneShot(splashClip, 0.2f);
        }
    }
}