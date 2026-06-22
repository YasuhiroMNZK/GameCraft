using UnityEngine;
using UnityEngine.UI;

public class Scale : MonoBehaviour
{
    // 指定した最終スケール
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float duration = 1f;
    [SerializeField, Range(0f, 1f)] private float targetAlpha = 1f;
    [SerializeField] private float minForce = 0f;
    [SerializeField] private float maxForce = 5f;
    [SerializeField] private float torqueMultiplier = 1f;
    [SerializeField] private Transform parentOnLaunch;

    private Vector3 startScale;
    private Color startColor;
    private Color endColor;
    private float elapsedTime;
    private bool isScaling = false;
    private bool isPaused = false;

    private SpriteRenderer spriteRenderer;
    private Graphic uiGraphic;
    private Rigidbody2D rb2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiGraphic = GetComponent<Graphic>();
        rb2D = GetComponent<Rigidbody2D>();

        startScale = transform.localScale;
        startColor = GetCurrentColor();
        endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        elapsedTime = 0f;
        isScaling = false;
        isPaused = false;
    }

    void Update()
    {
        if (!isScaling)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        float safeDuration = Mathf.Max(duration, 0.0001f);
        elapsedTime += Time.deltaTime;
        float fraction = elapsedTime / safeDuration;

        if (fraction >= 1f)
        {
            transform.localScale = targetScale;
            SetCurrentColor(endColor);
            ApplyRandomForce2D();
            isScaling = false;
            return;
        }

        transform.localScale = Vector3.Lerp(startScale, targetScale, fraction);
        SetCurrentColor(Color.Lerp(startColor, endColor, fraction));
    }

    public void ObjScale()
    {
        startScale = transform.localScale;
        startColor = GetCurrentColor();
        endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        elapsedTime = 0f;
        isScaling = true;
        isPaused = false;
    }

    public void PauseScale()
    {
        if (!isScaling)
        {
            return;
        }

        isPaused = true;
    }

    public void ResumeScale()
    {
        if (!isScaling)
        {
            return;
        }

        isPaused = false;
    }

    private Color GetCurrentColor()
    {
        if (spriteRenderer != null)
        {
            return spriteRenderer.color;
        }

        if (uiGraphic != null)
        {
            return uiGraphic.color;
        }

        return Color.white;
    }

    private void SetCurrentColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
            return;
        }

        if (uiGraphic != null)
        {
            uiGraphic.color = color;
        }
    }

    private void ApplyRandomForce2D()
    {
        if (rb2D == null)
        {
            return;
        }

        if (parentOnLaunch != null)
        {
            transform.SetParent(parentOnLaunch, true);
        }

        float forceMin = Mathf.Min(minForce, maxForce);
        float forceMax = Mathf.Max(minForce, maxForce);
        float force = Random.Range(forceMin, forceMax);
        Vector2 direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }

        rb2D.AddForce(direction * force, ForceMode2D.Impulse);

        float torqueDirection = Random.value < 0.5f ? -1f : 1f;
        float torque = force * torqueMultiplier * torqueDirection;
        rb2D.AddTorque(torque, ForceMode2D.Impulse);
    }
}
