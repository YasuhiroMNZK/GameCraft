using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField] private Slider targetSlider;

    private bool isUpdatingFromScript;

    private void Awake()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    public void AddValue(float delta)
    {
        if (targetSlider == null)
        {
            return;
        }

        targetSlider.value += delta;
    }

    private void OnSliderValueChanged(float value)
    {
        if (targetSlider == null || isUpdatingFromScript)
        {
            return;
        }

        if (value + Mathf.Epsilon < targetSlider.maxValue)
        {
            return;
        }

        isUpdatingFromScript = true;
        targetSlider.value = 0f;
        isUpdatingFromScript = false;
    }
}
