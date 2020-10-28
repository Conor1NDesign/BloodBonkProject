using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetHealth(float value)
    {
        slider.value = value;
    }

    public void SetMaxHealth(float maxValue)
    {
        slider.maxValue = maxValue;
    }
}
