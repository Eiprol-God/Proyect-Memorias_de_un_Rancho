using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalSettingsManager : MonoBehaviour
{
    public Slider brightnessSlider;
    public Slider volumeSlider;

    [Header("Componentes globales")]
    public AudioMixer audioMixer;
    public Image brightnessOverlay;

    private static GlobalSettingsManager instance;

    private void Awake()
    {
        //Esto mantiene las configuraciones en todo el juego
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // No cambair al cambiar de escena

        // Aplicar configuraciones guardadas apenas inicia
        ApplySavedSettings();

        // Escuchar cuando se cargan nuevas escenas
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reconectar referencias
        TryFindComponents();
        ApplySavedSettings();
    }

    private void TryFindComponents()
    {
        // Si no hay overlay o player, buscarlos automáticamente
        if (brightnessOverlay == null)
            brightnessOverlay = FindObjectOfType<Image>();

        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }

    //Llamar cuando se muevan sliders (desde el menú)

    public void ApplySettingsFromUI()
    {
        if (brightnessSlider != null)
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);

        if (volumeSlider != null)
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        PlayerPrefs.Save();
        ApplySavedSettings();
    }

    //Aplica los valores guardados a los componentes actuales

    public void ApplySavedSettings()
    {
        float brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // --- Brillo ---
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - brightness;
            brightnessOverlay.color = c;
        }

        // --- Volumen ---
        if (audioMixer != null)
        {
            float volDB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
            audioMixer.SetFloat("MasterVolume", volDB);
        }
        
        // --- Sincronizar sliders si existen ---

        if (brightnessSlider != null) brightnessSlider.value = brightness;
        if (volumeSlider != null) volumeSlider.value = volume;
    }

    //Restablecer valores predeterminados
    public void ResetDefaults()
    {
        PlayerPrefs.SetFloat("Brightness", 0.5f);
        PlayerPrefs.SetFloat("Volume", 1f);

        PlayerPrefs.Save();
        ApplySavedSettings();
    }
}