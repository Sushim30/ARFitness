using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CalorieDisplay : MonoBehaviour
{
    [SerializeField] private GameObject displayImage;
    [SerializeField] private TextMeshProUGUI displayText;

    private ARYogaSessionController controller;
    private Storage storage;

    private static CalorieDisplay instance; // Singleton pattern to avoid duplication

    private void Awake()
    {
        // Ensure only one instance persists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        storage = FindObjectOfType<Storage>();
        if (storage == null)
        {
            Debug.LogError("Storage component not found in the scene.");
        }

        StartCoroutine(FindSessionController());
    }

    public void ShowCalorie(float MET)
    {
        if (storage == null || controller == null)
        {
            Debug.LogWarning("Storage or ARYogaSessionController not initialized. Cannot calculate calories.");
            return;
        }

        displayImage.SetActive(true);
        float caloriesBurned = storage.playerWeight * 0.0175f * MET * (controller.sessionTime / 60f);
        displayText.text = caloriesBurned.ToString("F2");

        StartCoroutine(HideCalorieDisplay());
    }

    private IEnumerator HideCalorieDisplay()
    {
        yield return new WaitForSeconds(3f);
        displayImage.SetActive(false);
    }

    private IEnumerator FindSessionController()
    {
        while (controller == null)
        {
            controller = FindObjectOfType<ARYogaSessionController>();
            if (controller == null)
            {
                yield return new WaitForSeconds(1f); // Retry every second until the controller is found
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("todayDay", DateTime.Now.Day);
        PlayerPrefs.SetInt("todayMonth", DateTime.Now.Month);
        PlayerPrefs.SetFloat("totalTime", storage.totalTime);
        PlayerPrefs.SetFloat("dailyTime", storage.dailyTime);
        PlayerPrefs.SetFloat("playerWeight", storage.playerWeight);
        PlayerPrefs.SetFloat("calorieBurned", storage.dailyCalorieBurned);
        PlayerPrefs.Save();
    }
}
