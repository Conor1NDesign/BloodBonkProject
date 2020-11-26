using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupNotification : MonoBehaviour
{
    public Image notificationImage;
    private Color tempColor;
    public float startingAlpha;

    // Start is called before the first frame update
    void Start()
    {
        notificationImage = GetComponent<Image>();
        tempColor = notificationImage.color;
        tempColor.a = startingAlpha;
        notificationImage.color = tempColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (notificationImage.color.a > 0)
        {
            tempColor.a -= 0.02f;
            notificationImage.color = tempColor;
        }

        if (tempColor.a <= 0f)
            tempColor.a = 2f;
    }
}
