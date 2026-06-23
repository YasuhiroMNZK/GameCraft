using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private int currentScore = 0;
    [SerializeField] private Text scoreText;
    [SerializeField] private string scorePrefix = "Score: ";
    [SerializeField] private UnityEvent onScoreChanged;

    public int CurrentScore => currentScore;

    private void Start()
    {
        RefreshScoreText();
    }

    public void AddScore(int value)
    {
        if (value <= 0)
        {
            return;
        }

        currentScore += value;
        RefreshScoreText();
        onScoreChanged?.Invoke();
    }

    public void ResetScore()
    {
        currentScore = 0;
        RefreshScoreText();
        onScoreChanged?.Invoke();
    }

    private void RefreshScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + currentScore;
        }
    }
}
