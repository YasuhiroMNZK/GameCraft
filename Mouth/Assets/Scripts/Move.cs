using UnityEngine;

public class Move : MonoBehaviour
{
    // インスペクタで設定可能な移動先座標
    [SerializeField] private Vector2 targetPosition = new Vector2(0f, 1f);
    [SerializeField] private float duration = 2f; // 移動にかかる時間（秒）

    private Transform cachedTransform;
    private Rigidbody2D cachedRigidbody2D;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startTime;

    // ObjMove 実行時にだけ移動させるためのフラグ
    private bool isMoving = false;

    void Start()
    {
        cachedTransform = transform;
        cachedRigidbody2D = GetComponent<Rigidbody2D>();

        // 初期位置を記録
        startPosition = GetCurrentPosition();
        endPosition = targetPosition;
        startTime = 0f;
        isMoving = false;
    }

    void Update()
    {
        // ObjMove で移動が開始されていなければ何もしない
        if (!isMoving)
        {
            return;
        }

        // 線形補間で移動
        float elapsedTime = Time.time - startTime;
        float fractionOfJourney = elapsedTime / duration;

        if (fractionOfJourney >= 1f)
        {
            // 移動完了：最終位置に固定して終了
            SetCurrentPosition(endPosition);
            isMoving = false;
            return;
        }

        SetCurrentPosition(Vector2.Lerp(startPosition, endPosition, fractionOfJourney));
    }

    public void ObjMove()
    {
        // 移動開始位置と開始時間を設定し、移動を開始
        startPosition = GetCurrentPosition();
        endPosition = targetPosition;
        startTime = Time.time;
        isMoving = true;
    }

    private Vector2 GetCurrentPosition()
    {
        if (cachedRigidbody2D != null)
        {
            return cachedRigidbody2D.position;
        }

        return cachedTransform.position;
    }

    private void SetCurrentPosition(Vector2 position)
    {
        if (cachedRigidbody2D != null)
        {
            cachedRigidbody2D.MovePosition(position);
            return;
        }

        cachedTransform.position = new Vector3(position.x, position.y, cachedTransform.position.z);
    }
}

