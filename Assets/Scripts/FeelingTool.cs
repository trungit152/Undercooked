using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelingTool : MonoBehaviour
{
    public static FeelingTool instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void ZoomIn(Transform tf, float time = 0.1f, bool isPause = false)
    {
        StartCoroutine(ZoomInCoroutine(tf, time, isPause));
    }

    private IEnumerator ZoomInCoroutine(Transform tf, float time, bool isPause)
    {
        tf.gameObject.SetActive(true);
        Vector3 startScale = Vector3.zero; 
        Vector3 endScale = Vector3.one;  
        float elapsedTime = 0f; 
        while (elapsedTime < time)
        {
            tf.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        tf.localScale = endScale; 
        if(isPause)
        {
            Time.timeScale = 0;
        }
    }

    public void ZoomOut(Transform tf, float time = 0.1f)
    {
        StartCoroutine(ZoomOutCoroutine(tf, time));
    }

    private IEnumerator ZoomOutCoroutine(Transform tf, float time)
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            tf.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tf.localScale = endScale;
        tf.gameObject.SetActive(false);
    }
}
