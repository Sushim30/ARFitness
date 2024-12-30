using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkoutTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI dailyTimeText;
    public TextMeshProUGUI caloriesBurnedText;
    public float dailyTimeTarget = 15f;
    public Storage storage;
    public TMP_Dropdown dropDown;
    public Slider progressSlider;
    public TMP_InputField weightInputField1, weightInputField2;
    public GameObject weightAskingWindow;

    private void Awake()
    {
        storage = FindObjectOfType<Storage>();
        if (storage == null)
        {
            Debug.LogError("Storage component not found in the scene.");
        }
    }

    private void Start()
    {
        if (storage != null)
        {
            // Initialize input fields with player's weight
            float storedWeight = PlayerPrefs.GetFloat("playerWeight", storage.playerWeight);
            storage.playerWeight = storedWeight;
            weightInputField1.text = storedWeight.ToString("F1");
            weightInputField2.text = storedWeight.ToString("F1");
        }

        UpdateDisplayTime();

        dailyTimeTarget = PlayerPrefs.GetFloat("dailyTimeTarget", 15f);
        progressSlider.maxValue = dailyTimeTarget;

        // Add listeners to synchronize the input fields
        if (weightInputField1 != null)
            weightInputField1.onValueChanged.AddListener(OnWeightInput1Changed);
        if (weightInputField2 != null)
            weightInputField2.onValueChanged.AddListener(OnWeightInput2Changed);

        if (PlayerPrefs.GetInt("weightAskedOnce", 0) == 0)
        {
            weightAskingWindow.SetActive(true);
            PlayerPrefs.SetInt("weightAskedOnce", 1);
            PlayerPrefs.Save();
        }
        else
        {
            weightAskingWindow.SetActive(false);
        }
    }

    public void SetMinExeTime(int timeIndex)
    {
        switch (timeIndex)
        {
            case 0:
                dailyTimeTarget = 15f;
                break;
            case 1:
                dailyTimeTarget = 30f;
                break;
            case 2:
                dailyTimeTarget = 45f;
                break;
            default:
                dailyTimeTarget = 60f;
                break;
        }

        progressSlider.maxValue = dailyTimeTarget;
        PlayerPrefs.SetFloat("dailyTimeTarget", dailyTimeTarget);
        PlayerPrefs.Save();
    }

    public void UpdateDisplayTime()
    {
        if (storage == null) return;

        progressSlider.value = storage.dailyTime / 60f;
        totalTimeText.text = (storage.totalTime / 60f).ToString("F1");
        dailyTimeText.text = (storage.dailyTime / 60f).ToString("F1");
        PlayerPrefs.SetFloat("totalTime", storage.totalTime);
        PlayerPrefs.SetFloat("dailyTime", storage.dailyTime);
        if (storage.highestDailyTime < storage.dailyTime)
        {
            storage.highestDailyTime = storage.dailyTime;
            PlayerPrefs.SetFloat("highestDailyTime", storage.highestDailyTime);
            PlayerPrefs.Save();
        }

        caloriesBurnedText.text = $"{storage.dailyCalorieBurned:F1}\nCalories\nBurned";
    }

    private void OnWeightInput1Changed(string value)
    {
        SynchronizeWeightFields(value, weightInputField2);
    }

    private void OnWeightInput2Changed(string value)
    {
        SynchronizeWeightFields(value, weightInputField1);
    }

    private void SynchronizeWeightFields(string value, TMP_InputField otherField)
    {
        if (float.TryParse(value, out float newWeight))
        {
            storage.playerWeight = newWeight;
            if (otherField != null)
                otherField.text = newWeight.ToString("F1");

            PlayerPrefs.SetFloat("playerWeight", newWeight);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("Invalid weight input. Please enter a valid number.");
        }
    }

    public void UpdateWeightData()
    {
        if (float.TryParse(weightInputField2.text, out float newWeight))
        {
            storage.playerWeight = newWeight;
            PlayerPrefs.SetFloat("playerWeight", newWeight);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogWarning("Invalid weight input. Please enter a valid number.");
        }
    }

    public void ImageDisabler()
    {
        weightAskingWindow.SetActive(false);
    }

    private void OnDestroy()
    {
        // Remove listeners to avoid memory leaks
        if (weightInputField1 != null)
            weightInputField1.onValueChanged.RemoveListener(OnWeightInput1Changed);
        if (weightInputField2 != null)
            weightInputField2.onValueChanged.RemoveListener(OnWeightInput2Changed);
    }
}
