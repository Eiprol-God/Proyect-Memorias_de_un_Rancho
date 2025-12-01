using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalSettingsManager : MonoBehaviour
{
    public Slider brightnessSlider;
    public Slider volumeSlider;

    [Header("Global Components")]
    public AudioMixer audioMixer;
    public Image brightnessOverlay;

    private static GlobalSettingsManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindUI();
        ApplySavedSettings();
        ConnectSliderEvents();
    }

    // -------------------------------------------------------------------
    // AUTO-BUSCAR UI EN CADA ESCENA
    // -------------------------------------------------------------------
    private void TryFindUI()
    {
        // ---- BRILLO ----
        if (brightnessOverlay == null)
        {
            Image[] images = FindObjectsOfType<Image>(true);
            foreach (Image img in images)
            {
                if (img.name == "BrightnessOverlay")
                {
                    brightnessOverlay = img;
                    break;
                }
            }
        }

        // ---- SLIDER DE BRILLO ----
        if (brightnessSlider == null)
        {
            Slider[] sliders = FindObjectsOfType<Slider>(true);
            foreach (Slider s in sliders)
            {
                if (s.name == "BrightnessSlider")
                {
                    brightnessSlider = s;
                    break;
                }
            }
        }

        // ---- SLIDER DE VOLUMEN ----
        if (volumeSlider == null)
        {
            Slider[] sliders = FindObjectsOfType<Slider>(true);
            foreach (Slider s in sliders)
            {
                if (s.name == "VolumeSlider")
                {
                    volumeSlider = s;
                    break;
                }
            }
        }
    }

    // CONECTAR EVENTOS DE SLIDERS
    private void ConnectSliderEvents()
    {
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.AddListener(OnBrightnessValueChanged);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeValueChanged);
        }
    }

    // CAMBIO EN TIEMPO REAL (ANTES DE APLICAR)
    private void OnBrightnessValueChanged(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value;
            brightnessOverlay.color = c;
        }
    }

    private void OnVolumeValueChanged(float value)
    {
        if (audioMixer != null)
        {
            float volDB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
            audioMixer.SetFloat("MasterVolume", volDB);
        }
    }

    // BOTÓN: APLICAR
    public void ApplySettingsFromUI()
    {
        if (brightnessSlider != null)
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);

        if (volumeSlider != null)
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);

        PlayerPrefs.Save();
    }

    // -------------------------------------------------------------------
    // APLICAR VALORES GUARDADOS
    // -------------------------------------------------------------------
    public void ApplySavedSettings()
    {
        float brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // Actualizar sliders
        if (brightnessSlider != null)
            brightnessSlider.value = brightness;

        if (volumeSlider != null)
            volumeSlider.value = volume;

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
    }

    // BOTÓN: RESET
    public void ResetDefaults()
    {
        PlayerPrefs.SetFloat("Brightness", 0.5f);
        PlayerPrefs.SetFloat("Volume", 1f);
        PlayerPrefs.Save();

        ApplySavedSettings();
    }
}