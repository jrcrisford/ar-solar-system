using UnityEngine;
using UnityEngine.UI;

public class SolarSystemManager : MonoBehaviour
{
    public PlanetController[] planets;
    public PlanetOrbit[] orbits;
    private bool orbitsPaused = false;
    public Button toggleOrbitButton;
    public Slider scaleSlider;
    public GameObject solarSystemGroup;
    private Vector3 originalScale;

    private Color normalColor = Color.white;
    private Color pausedColor = Color.red;

    private void Start()
    {
        originalScale = solarSystemGroup.transform.localScale;

        if (scaleSlider != null )
        {
            scaleSlider.minValue = 0.1f;
            scaleSlider.maxValue = 2.0f;
            scaleSlider.value = 1.0f;
            scaleSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    public void OnSliderChanged(float value)
    {
        solarSystemGroup.transform.localScale = originalScale * value;
    }

    public void ResetAllPlanets()
    {
        foreach (PlanetController planet in planets)
        {
            planet.ResetTransform();
        }
    }

    public void ToggleAllOrbits()
    {
        orbitsPaused = !orbitsPaused;
        foreach (PlanetOrbit orbit in orbits)
        {
            orbit.ToggleOrbit(orbitsPaused);
        }

        ColorBlock colors = toggleOrbitButton.colors;

        if (orbitsPaused)
        {
            colors.normalColor = pausedColor;
            colors.selectedColor = pausedColor;
        }
        else
        {
            colors.normalColor = normalColor;
            colors.selectedColor = normalColor;
        }

        toggleOrbitButton.colors = colors;
    }
}
