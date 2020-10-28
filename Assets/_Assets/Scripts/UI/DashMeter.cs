using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetDashMeter(float value)
    {
        slider.value = value;
    }

    public void SetMaxDashMeter(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
