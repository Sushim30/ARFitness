using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkoutTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI dailyTimeText;
    public float dailyTimeTarget = 15f;
    public Storage storage;
    public TMPro.TMP_Dropdown dropDown;
    public Slider progressSlider;
    private void Awake()
    {
        storage = GameObject.FindObjectOfType<Storage>();
    }
    void Start()
    {
        UpdateDisplayTime();
        dailyTimeTarget = PlayerPrefs.GetFloat("dailyTimeTarget", 15f);

    }

    public void SetMinExeTime(int timeIndex)
    {
        if (timeIndex == 0)
        {
            dailyTimeTarget = 15f;
        }
        else if (timeIndex == 1)
        {
            dailyTimeTarget = 30f;
        }
        else if(timeIndex == 2)
        {
            dailyTimeTarget = 45f;
        }
        else
        {
            dailyTimeTarget = 60f;
        }
        progressSlider.maxValue = dailyTimeTarget;
        PlayerPrefs.SetFloat("dailyTimeTarget",dailyTimeTarget);
    }

    public void UpdateDisplayTime()
    {
        progressSlider.value = storage.dailyTime / 60f;
        totalTimeText.text = (storage.totalTime/60f).ToString("F1"); // Display with one decimal point
        dailyTimeText.text = (storage.dailyTime/60f).ToString("F1");
        if (storage.highestDailyTime < storage.dailyTime)
        {
            storage.highestDailyTime = storage.dailyTime;
            PlayerPrefs.SetFloat("highestDailyTime",storage.highestDailyTime);
        }
    }

    private void Update()
    {
        //Debug.Log(dailyTimeTarget);
    }
}
