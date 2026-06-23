using UnityEngine;

public class EnemyScoreValue : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int scoreValue = 100;

    public int ScoreValue => scoreValue;
}
