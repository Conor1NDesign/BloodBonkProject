using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float value)
    {
        slider.value = value;
    }

    public void SetMaxHealth(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
