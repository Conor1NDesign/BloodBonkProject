using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{
    public Slider slider;

    public void SetDashMeter(float value)
    {
        slider.value = value;
    }

    public void SetMaxDashMeter(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
