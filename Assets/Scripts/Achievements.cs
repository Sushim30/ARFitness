using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public Image thirtyMinAchievement, sixtyMinAchievement, oneTwentyMinAchievement;
    public Image oneDay, threeDay, fiveDay;
    public Storage storage;

    private void Awake()
    {
        storage = GameObject.FindObjectOfType<Storage>();
        if (storage == null)
        {
            Debug.LogWarning("Storage object not found!");
            return;
        }

        SetImageColor(thirtyMinAchievement, 75);
        SetImageColor(sixtyMinAchievement, 75);
        SetImageColor(oneTwentyMinAchievement, 75);
        SetImageColor(oneDay, 75);
        SetImageColor(threeDay, 75);
        SetImageColor(fiveDay, 75);
    }

    private void Start()
    {
        if (storage == null) return;

        if (storage.highestDailyTime/60f >= 30f)
            SetImageColor(thirtyMinAchievement, 255);
        if (storage.highestDailyTime/60f >= 60f)
            SetImageColor(sixtyMinAchievement, 255);
        if (storage.highestDailyTime/60f >= 120f)
            SetImageColor(oneTwentyMinAchievement, 255);
        if (storage.totalTime/60f >= (24 * 60f))
            SetImageColor(oneDay, 255);
        if (storage.totalTime/60f >= (24 * 3 * 60f))
            SetImageColor(threeDay, 255);
        if (storage.totalTime/60f >= (24 * 5 * 60f))
            SetImageColor(fiveDay, 255);
    }

    private void SetImageColor(Image image, float value)
    {
        if (image != null)
            image.color = new Color(value / 255f, value / 255f, value / 255f);
    }
}
