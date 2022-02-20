using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class LapTimer : MonoBehaviour
{
    [ShowNonSerializedField] bool clockwise = true;
    [SerializeField] TMPro.TMP_Text lapTimeText;

    [NaughtyAttributes.Tag] [SerializeField] string cartag;


    float lapTime = 0;
    private Coroutine lap;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == cartag)
        {
            if (lap != null)
            {
                StopTimer(clockwise);
            }

            float dot = Vector3.Dot(other.transform.TransformDirection(Vector3.forward), transform.TransformDirection(Vector3.forward));
            //Debug.Log(dot);

            if (dot > 0f)
            {
                clockwise = false;
            }
            else clockwise = true;

            if (lap == null) lap = StartCoroutine(StartLapTime(clockwise));
        }
    }

    private IEnumerator StartLapTime(bool startClockwise)
    {
        lapTime = 0;
        while (true)
        {
            lapTime += Time.deltaTime;
            yield return null;
        }
    }

    private void StopTimer(bool stopClockwise)
    {
        Debug.Log("LAPTIME " + lapTime);
        if (lapTime < float.Parse(lapTimeText.text))
        {
            Debug.Log("New record!");
            lapTimeText.text = lapTime+"";
            lapTimeText.color = Color.green;
            StartCoroutine(ResetLaptimeColor());
        }
        lapTime = 0f;
        StopCoroutine(lap);
        lap = null;
    }

    private IEnumerator ResetLaptimeColor()
    {
        yield return new WaitForSeconds(5f);
        lapTimeText.color = Color.white;
    }
}
