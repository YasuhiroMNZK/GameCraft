using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cover : MonoBehaviour
{
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private float maxHoleRadius = 140f;
    [SerializeField] private float expandSpeed = 800f;
    [SerializeField] private float shrinkSpeed = 1200f;
    [SerializeField] private float edgeSoftness = 40f;

    private static readonly int CursorScreenPosId = Shader.PropertyToID("_CursorScreenPos");
    private static readonly int RadiusId = Shader.PropertyToID("_Radius");
    private static readonly int SoftnessId = Shader.PropertyToID("_Softness");
    private Image image;
    private Material runtimeMaterial;
    private RectTransform panelRect;
    private Camera uiCamera;
    private float holeRadius;

    private void Awake()
    {
        image = GetComponent<Image>();
        panelRect = targetPanel != null ? targetPanel : image.rectTransform;

        if (targetCanvas == null)
        {
            targetCanvas = GetComponentInParent<Canvas>();
        }

        uiCamera = targetCanvas != null && targetCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? targetCanvas.worldCamera
            : null;

        Shader shader = Shader.Find("UI/CircularHoleMask");
        if (shader == null)
        {
            Debug.LogError("Shader 'UI/CircularHoleMask' not found. Create the shader file first.", this);
            enabled = false;
            return;
        }

        runtimeMaterial = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };

        image.material = runtimeMaterial;
        UpdateMaterial(Vector2.zero, 0f);
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            if (Application.isPlaying)
            {
                Destroy(runtimeMaterial);
            }
            else
            {
                DestroyImmediate(runtimeMaterial);
            }
        }
    }

    private void Update()
    {
        if (panelRect == null || runtimeMaterial == null)
        {
            return;
        }

        if (Pointer.current == null)
        {
            return;
        }

        Vector2 pointerPosition = Pointer.current.position.ReadValue();

        bool isInside = RectTransformUtility.RectangleContainsScreenPoint(
            panelRect,
            pointerPosition,
            uiCamera);

        if (isInside)
        {
            holeRadius = Mathf.MoveTowards(holeRadius, maxHoleRadius, expandSpeed * Time.deltaTime);
        }

        UpdateMaterial(pointerPosition, holeRadius);
    }

    private void UpdateMaterial(Vector2 cursorScreenPosition, float radius)
    {
        runtimeMaterial.SetVector(CursorScreenPosId, cursorScreenPosition);
        runtimeMaterial.SetFloat(RadiusId, radius);
        runtimeMaterial.SetFloat(SoftnessId, edgeSoftness);
    }
}
