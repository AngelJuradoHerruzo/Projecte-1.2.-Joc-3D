using UnityEngine;

public class MusicaAmbientalPersistente : MonoBehaviour
{
    // Singleton para que solo exista una instancia de la música ambiental
    public static AudioSource AudioSource;

    void Awake()
    {
        // Si aún no hay AudioSource global, se crea esta instancia
        if (AudioSource == null)
        {
            // Obtener el AudioSource de este GameObject
            AudioSource = GetComponent<AudioSource>();

            // Hacer que este objeto persista al cambiar de escena
            DontDestroyOnLoad(gameObject);

            // Reproducir la música si aún no está sonando
            if (!AudioSource.isPlaying)
                AudioSource.Play();
        }
        else
        {
            // Si ya existe un AudioSource, eliminar duplicados
            Destroy(gameObject);
        }
    }

    // Reproducir o reanudar la música
    public void PlayMusic()
    {
        if (!AudioSource.isPlaying)
            AudioSource.Play();      // Inicia la reproducción si estaba detenida
        else
            AudioSource.UnPause();   // Reanuda si estaba pausada
    }
    
    // Pausar la música sin reiniciarla
    public void StopMusic()
    {
        if (AudioSource.isPlaying)
            AudioSource.Pause();     // Pausa la música para poder reanudar después
    }
}