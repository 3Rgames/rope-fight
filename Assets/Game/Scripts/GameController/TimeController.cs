using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoSingleton<TimeController>
{
    public void SmoothTimeScaleChange(float newTime, float timeToChange)
    {
        IEnumerator SmoothTimeScaleChangeCor()
        {
            float currentTime = Time.timeScale;

            for (float t = 0; t < 1; t += Time.deltaTime / timeToChange)
            {
                TimeScaleChange(Mathf.Lerp(currentTime, newTime, t));
                yield return null;
            }
            TimeScaleChange(newTime);
        }
        StartCoroutine(SmoothTimeScaleChangeCor());
    }

    public void TimeScaleChange(float time)
    {
        Time.timeScale = time;
    }
}

// TimeController.Instance.TimeScaleChange(float time);                                  - to quick change time
// TimeController.Instance.SmoothTimeScaleChange(float newTime, float timeToChange);     - to smoothly change time
// Yours ever 3R