using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour
{
    public TMP_Text lapTimeText; // UI Text per mostrare il tempo del giro corrente
    public TMP_Text bestLapTimeText; // UI Text per mostrare il miglior tempo del giro
    public TMP_Text lastLapTimeText; // UI Text per mostrare l'ultimo tempo del giro

    private float lapTime = 0f; // Tempo del giro corrente
    private float bestLapTime = Mathf.Infinity; // Miglior tempo del giro, inizialmente infinito
    private float lastLapTime = 0f; // Tempo dell'ultimo giro
    private bool isRunning = false; // Indica se il timer è in esecuzione

    private CanvasManager _canvasManager;

    private void Start()
    {
        _canvasManager = FindObjectOfType<CanvasManager>();
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            lapTime += Time.deltaTime;
            lapTimeText.text = "Lap Time: " + FormatTime(lapTime);
        }
    }

    public void StartTimer()
    {
        lapTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
        lastLapTime = lapTime;

        if (lastLapTime < bestLapTime)
        {
            bestLapTime = lastLapTime;
            bestLapTimeText.text = "Best Lap: " + FormatTime(bestLapTime);
        }

        lastLapTimeText.text = "Last Lap: " + FormatTime(lastLapTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            _canvasManager.laps++;
            Debug.Log(_canvasManager.laps + " / " + _canvasManager.totLaps);

            if (_canvasManager.laps >= _canvasManager.totLaps)
            {
                // Se il numero di giri completati è uguale o maggiore al totale, ferma il timer definitivamente
                StopTimer();
                Debug.Log("Race Finished!");
            }
            else
            {
                // Se ci sono ancora giri da completare, ferma e riavvia il timer
                StopTimer();
                StartTimer();
            }
        }
    }
}