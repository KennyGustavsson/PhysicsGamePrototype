using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRewindUI : MonoBehaviour
{
    private TimeManager timeManager;
    private Slider timeRewindSlider;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        timeRewindSlider = GetComponent<Slider>();

        timeRewindSlider.maxValue = timeManager.MaxRewindFrames;
    }

    private void Update()
    {
        timeRewindSlider.value = timeManager.FrameCounter;
    }
}
