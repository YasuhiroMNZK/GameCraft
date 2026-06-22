using UnityEngine;
using UnityEngine.Events;

public class Contact : MonoBehaviour
{
    [SerializeField] private Transform targetParent;
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

        // 相手を削除してイベントを発火
        Destroy(other);
        contactEvent?.Invoke();
    }
}
