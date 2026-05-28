using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource footstepSource;

    public AudioClip menuMusic;
    public AudioClip beachMusic;
    public AudioClip selvaMusic;
    public AudioClip caveMusic;
    public AudioClip pickUpClip;
    public AudioClip footstepClip;

    public TMP_Text contadorRecolectablesText;

    public DatosGuardados datos = new DatosGuardados();

    private string rutaArchivo;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        rutaArchivo = Path.Combine(Application.persistentDataPath, "progreso_jugador.json");
        PrepararAudioSources();
    }

    void Start()
    {
        ReiniciarGuardado();
        Cargar();
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PrepararAudioSources()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (musicSource == null)
        {
            musicSource = sources.Length > 0 ? sources[0] : gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = sources.Length > 1 ? sources[1] : gameObject.AddComponent<AudioSource>();
        }

        if (footstepSource == null)
        {
            footstepSource = sources.Length > 2 ? sources[2] : gameObject.AddComponent<AudioSource>();
        }

        musicSource.playOnAwake = false;
        musicSource.loop = true;
        sfxSource.playOnAwake = false;
        footstepSource.playOnAwake = false;
        footstepSource.loop = true;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayBeachMusic()
    {
        PlayMusic(beachMusic);
    }

    public void PlaySelvaMusic()
    {
        PlayMusic(selvaMusic != null ? selvaMusic : caveMusic);
    }

    public void PlayCaveMusic()
    {
        PlayMusic(caveMusic);
    }

    public void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "Menu")
        {
            PlayMenuMusic();
        }
        else if (sceneName == "Playa")
        {
            PlayBeachMusic();
        }
        else if (sceneName == "Selva")
        {
            PlaySelvaMusic();
        }
        else if (sceneName == "Cuevass" || sceneName == "Cueva")
        {
            PlayCaveMusic();
        }
    }

    public void PlayPickUp()
    {
        PlaySFX(pickUpClip);
    }

    public void PlayFootstep()
    {
        PlaySFX(footstepClip);
    }

    public void StartFootsteps()
    {
        if (footstepSource == null || footstepClip == null) return;
        if (footstepSource.isPlaying) return;

        footstepSource.clip = footstepClip;
        footstepSource.loop = true;
        footstepSource.Play();
    }

    public void StopFootsteps()
    {
        if (footstepSource == null) return;

        footstepSource.Stop();
    }

    public void RegistrarObjetoRecogido(string escena, string objeto)
    {
        if (string.IsNullOrEmpty(escena) || string.IsNullOrEmpty(objeto)) return;

        List<string> lista = ObtenerListaEscena(escena);

        if (!lista.Contains(objeto))
        {
            lista.Add(objeto);
            ActualizarTotales();
            Guardar();
            ActualizarTextoConteo();
        }
    }

    public bool ObjetoYaRecogido(string escena, string objeto)
    {
        if (string.IsNullOrEmpty(escena) || string.IsNullOrEmpty(objeto)) return false;

        return ObtenerListaEscena(escena).Contains(objeto);
    }

    public void CompletarZona(string escena)
    {
        datos.zonaCompletada = escena;
        datos.actividadCompletada = true;
        datos.resultado = "Exito";
        Guardar();
    }

    List<string> ObtenerListaEscena(string escena)
    {
        datos.PrepararListas();

        if (escena == "Selva") return datos.objetosSelva;
        if (escena == "Cueva") return datos.objetosCueva;

        return datos.objetosPlaya;
    }

    void ActualizarTotales()
    {
        datos.PrepararListas();

        datos.objetosPlayaCantidad = datos.objetosPlaya.Count;
        datos.objetosSelvaCantidad = datos.objetosSelva.Count;
        datos.objetosCuevaCantidad = datos.objetosCueva.Count;
        datos.objetosRecogidos = datos.objetosPlayaCantidad + datos.objetosSelvaCantidad + datos.objetosCuevaCantidad;
    }

    public void ActualizarTextoConteo()
    {
        if (contadorRecolectablesText == null) return;

        ActualizarTotales();
        contadorRecolectablesText.text = datos.objetosRecogidos + " / " + (datos.objetosPlayaCantidad + datos.objetosSelvaCantidad + datos.objetosCuevaCantidad);
    }

    public string ObtenerResumenDatos()
    {
        ActualizarTotales();

        return "Objetos recogidos: " + datos.objetosRecogidos +
               "\nPlaya: " + datos.objetosPlayaCantidad +
               "\nSelva: " + datos.objetosSelvaCantidad +
               "\nZona completada: " + datos.zonaCompletada +
               "\nResultado: " + datos.resultado +
               "\nTiempo total: " + Mathf.RoundToInt(datos.tiempoTotal) + " segundos";
    }

    public void Guardar()
    {
        string carpeta = Path.GetDirectoryName(rutaArchivo);

        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        datos.tiempoTotal = Time.time;
        ActualizarTotales();

        File.WriteAllText(rutaArchivo, JsonUtility.ToJson(datos, true));
    }

    public void ReiniciarGuardado()
    {
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
        }

        datos = new DatosGuardados();
        ActualizarTextoConteo();
    }

    public void Cargar()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            datos = JsonUtility.FromJson<DatosGuardados>(json);

            if (datos == null)
            {
                datos = new DatosGuardados();
            }

            datos.PrepararListas();
            ActualizarTotales();
            ActualizarTextoConteo();
            Debug.Log("Datos cargados: " + datos.objetosRecogidos + " objetos recogidos.");
        }
        else
        {
            datos = new DatosGuardados();
            ActualizarTextoConteo();
            Debug.LogWarning("Archivo de guardado no encontrado en: " + rutaArchivo);
        }
    }
}

[System.Serializable]
public class DatosGuardados
{
    public int objetosRecogidos;
    public int objetosPlayaCantidad;
    public int objetosSelvaCantidad;
    public int objetosCuevaCantidad;
    public float tiempoTotal;
    public string zonaCompletada;
    public string resultado;
    public bool actividadCompletada = false;
    public List<string> objetosPlaya = new List<string>();
    public List<string> objetosSelva = new List<string>();
    public List<string> objetosCueva = new List<string>();

    public void PrepararListas()
    {
        if (objetosPlaya == null) objetosPlaya = new List<string>();
        if (objetosSelva == null) objetosSelva = new List<string>();
        if (objetosCueva == null) objetosCueva = new List<string>();
    }
}
