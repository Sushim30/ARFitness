using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public float totalTime, dailyTime;
    private int todayDay, todayMonth;
    public float highestDailyTime;
    public float playerWeight;
    public float dailyCalorieBurned;
    public int weightAskedOnce;
    private void Awake()
    {
        // Load data from PlayerPrefs
        playerWeight = PlayerPrefs.GetFloat("playerWeight", 50f);
        todayDay = PlayerPrefs.GetInt("todayDay", 0);
        todayMonth = PlayerPrefs.GetInt("todayMonth", 0);
        totalTime = PlayerPrefs.GetFloat("totalTime", 0f);
        dailyTime = PlayerPrefs.GetFloat("dailyTime", 0f);
        highestDailyTime = PlayerPrefs.GetFloat("highestDailyTime", 0f);
        dailyCalorieBurned = PlayerPrefs.GetFloat("calorieBurned", 0f);
        weightAskedOnce = PlayerPrefs.GetInt("weightAskedOnce", 0);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Check if a new day has started
        if ((todayDay != DateTime.Now.Day) || (todayMonth != DateTime.Now.Month))
        {
            dailyTime = 0; // Reset daily time
            todayDay = DateTime.Now.Day;
            todayMonth = DateTime.Now.Month;
            dailyCalorieBurned = 0;
            // Save the updated day and month
            PlayerPrefs.SetFloat("calorieBurned", dailyCalorieBurned);
            PlayerPrefs.SetInt("todayDay", todayDay);
            PlayerPrefs.SetInt("todayMonth", todayMonth);
        }
    }

    private void OnApplicationQuit()
    {
        // Save data to PlayerPrefs on application quit
        PlayerPrefs.SetInt("todayDay", DateTime.Now.Day);
        PlayerPrefs.SetInt("todayMonth", DateTime.Now.Month);
        PlayerPrefs.SetFloat("totalTime", totalTime);
        PlayerPrefs.SetFloat("dailyTime", dailyTime);
        PlayerPrefs.SetFloat("playerWeight", playerWeight);
        PlayerPrefs.SetFloat("calorieBurned", dailyCalorieBurned);
        PlayerPrefs.Save();
    }
}
