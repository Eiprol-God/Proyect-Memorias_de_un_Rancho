using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalSettingsManager : MonoBehaviour
{
    [Header("UI opcional (solo si hay menú de opciones en la escena)")]
    public Slider brightnessSlider;
    public Slider volumeSlider;

    [Header("Componentes globales")]
    public AudioMixer audioMixer;
    public Image brightnessOverlay;

    private static GlobalSettingsManager instance;

    private void Awake()
    {
        // --- Singleton para que no haya duplicados ---
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // --- SUSCRIBIR EL EVENTO IMPORTANTÍSIMO ---
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindComponents();
        ApplySavedSettings();
    }

    private void TryFindComponents()
{
    // Buscar overlay por nombre exacto
    if (brightnessOverlay == null)
    {
        GameObject overlayObj = GameObject.Find("BrightnessOverlay");
        if (overlayObj != null)
            brightnessOverlay = overlayObj.GetComponent<Image>();
    }

    // Buscar sliders si existen en la escena
    GameObject bSlider = GameObject.Find("BrightnessSlider");
    if (bSlider != null)
        brightnessSlider = bSlider.GetComponent<Slider>();

    GameObject vSlider = GameObject.Find("VolumeSlider");
    if (vSlider != null)
        volumeSlider = vSlider.GetComponent<Slider>();
}


    //Llamar cuando se muevan sliders
    public void ApplySettingsFromUI()
    {
        if (brightnessSlider != null)
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);

        if (volumeSlider != null)
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        PlayerPrefs.Save();
        ApplySavedSettings();
    }

    public void ApplySavedSettings()
    {
        float brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // Brillo
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - brightness;
            brightnessOverlay.color = c;
        }

        // Volumen
        if (audioMixer != null)
        {
            float volDB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
            audioMixer.SetFloat("MasterVolume", volDB);
        }

        // Sliders (si existen en esta escena)
        if (brightnessSlider != null) brightnessSlider.value = brightness;
        if (volumeSlider != null) volumeSlider.value = volume;
    }

    public void ResetDefaults()
    {
        PlayerPrefs.SetFloat("Brightness", 0.5f);
        PlayerPrefs.SetFloat("Volume", 1f);
        PlayerPrefs.Save();
        ApplySavedSettings();
    }
}