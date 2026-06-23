using UnityEngine;
using UnityEngine.Events;

public class Contact : MonoBehaviour
{
    [SerializeField] private Transform targetParent;
    [SerializeField] private Score scoreManager;
    [SerializeField] private int defaultEnemyScore = 100;
    [SerializeField] private GameObject defeatEffectPrefab;
    [SerializeField] private float effectAutoDestroySeconds = 1.5f;
    [SerializeField] private UnityEvent contactEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleContact(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleContact(collision.gameObject);
    }

    private void HandleContact(GameObject other)
    {
        // ガード: このコンポーネントや GameObject が無効なら実行しない
        if (!enabled || !gameObject.activeInHierarchy) return;

        // targetParent が設定されているか、アクティブかをチェック
        if (targetParent == null || !targetParent.gameObject.activeInHierarchy) return;

        // 接触相手が targetParent の子でなければ無視
        if (!other.transform.IsChildOf(targetParent))
        {
            return;
        }

        int gainedScore = defaultEnemyScore;
        EnemyScoreValue enemyScoreValue = other.GetComponent<EnemyScoreValue>();
        if (enemyScoreValue != null)
        {
            gainedScore = enemyScoreValue.ScoreValue;
        }

        if (scoreManager != null && gainedScore > 0)
        {
            scoreManager.AddScore(gainedScore);
        }

        SpawnDefeatEffect(other.transform.position);

        // 相手を削除してイベントを発火
        Destroy(other);
        contactEvent?.Invoke();
    }

    private void SpawnDefeatEffect(Vector3 worldPosition)
    {
        if (defeatEffectPrefab == null)
        {
            return;
        }

        GameObject effectInstance = Instantiate(defeatEffectPrefab, worldPosition, Quaternion.identity);
        if (effectAutoDestroySeconds > 0f)
        {
            Destroy(effectInstance, effectAutoDestroySeconds);
        }
    }
}
