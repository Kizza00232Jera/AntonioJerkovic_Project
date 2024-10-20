using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityUI : MonoBehaviour
{
    public Slider sensitivitySlider;
    private FirstPersonController firstPersonController;

    void Start()
    {
        // Load the saved sensitivity value or set a default
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.value = savedSensitivity;

        // Find the FirstPersonController component in the scene
        firstPersonController = FindObjectOfType<FirstPersonController>();

        // Update sensitivity in FirstPersonController initially
        if (firstPersonController != null)
        {
            firstPersonController.SetLookSensitivity(savedSensitivity);
        }

        // Add listener to update sensitivity when slider changes
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    void UpdateSensitivity(float value)
    {
        // Save the new sensitivity value
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        // Update the sensitivity in the FirstPersonController
        if (firstPersonController != null)
        {
            firstPersonController.SetLookSensitivity(value);
        }
    }
}
