using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float delay;
    public float duration;
    public float magnitude;

    float currentDelay;

    public IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;

        float elasped = 0.0f;

        while (elasped < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elasped += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void Shaked()
    {

        currentDelay = Time.time + delay;

        Vector3 originalPos = transform.localPosition;

        while (Time.time < currentDelay)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            Debug.Log("while");
        }

        transform.localPosition = originalPos;
    }
}
