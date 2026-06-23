using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 10f;
    [SerializeField] private UnityEvent onCountdownZero;
    [SerializeField] private bool autoStartOnAwake = false;
    [SerializeField] private Text timeDisplay;
    [SerializeField] private string timeFormat = "0.##";

    private bool isCountingDown = false;
    private bool hasReachedZero = false;

    public float TimeRemaining => timeRemaining;
    public bool IsCountingDown => isCountingDown;

    private void Awake()
    {
        if (autoStartOnAwake)
        {
            StartCountdown();
        }
    }

    private void Update()
    {
        if (!isCountingDown)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;
        UpdateTimeDisplay();
        if (timeRemaining <= 0f && !hasReachedZero)
        {
            timeRemaining = 0f;
            hasReachedZero = true;
            isCountingDown = false;
            UpdateTimeDisplay();
            onCountdownZero?.Invoke();
        }
    }

    public void StartCountdown()
    {
        isCountingDown = true;
        hasReachedZero = false;
    }

    public void StopCountdown()
    {
        isCountingDown = false;
    }

    public void ResetCountdown(float newTime)
    {
        timeRemaining = newTime;
        hasReachedZero = false;
        isCountingDown = false;
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        if (timeDisplay != null)
        {
            float total = Mathf.Max(0f, timeRemaining);
            int minutes = (int)(total / 60f);
            int seconds = (int)(total % 60f);
            int hundredths = (int)(Mathf.Floor(total * 100f) % 100);
            timeDisplay.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, hundredths);
        }
    }
}
