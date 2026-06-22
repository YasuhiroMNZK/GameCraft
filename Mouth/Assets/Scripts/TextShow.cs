using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class TextShow : MonoBehaviour
{
    public List<Kotodama> kotodamaList = new List<Kotodama>();
    public Text displayText;
    public float charDelay = 0.05f;
    public Slider alertSlider;
    public Text alertValueText;
    public UnityEvent onAlertLimitReached;
    public float alertLimitWithoutSlider = 100f;

    private Coroutine typingCoroutine;
    private string lastText = null;
    private float currentAlertValue = 0f;

    public void ShowKotodamaText()
    {
        if (kotodamaList != null && kotodamaList.Count > 0)
        {
            int count = kotodamaList.Count;
            int idx = 0;

            if (count == 1)
            {
                idx = 0;
            }
            else
            {
                bool allSameAsLast = true;
                if (lastText == null) allSameAsLast = false;
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (kotodamaList[i] != null && kotodamaList[i].text != lastText)
                        {
                            allSameAsLast = false;
                            break;
                        }
                    }
                }

                if (allSameAsLast)
                {
                    idx = UnityEngine.Random.Range(0, count);
                }
                else
                {
                    idx = UnityEngine.Random.Range(0, count);
                    int safety = 0;
                    while (kotodamaList[idx] != null && kotodamaList[idx].text == lastText && safety < 50)
                    {
                        idx = UnityEngine.Random.Range(0, count);
                        safety++;
                    }
                }
            }

            Kotodama targetKotodama = kotodamaList[idx];
            string msg = targetKotodama != null ? targetKotodama.text : string.Empty;
            ShowText(msg, targetKotodama);
            lastText = msg;
            return;
        }

        ShowText(string.Empty);
    }

    public void ShowText(string message)
    {
        ShowText(message, null);
    }

    private void ShowText(string message, Kotodama targetKotodama)
    {
        if (displayText == null)
        {
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        typingCoroutine = StartCoroutine(TypeText(message, targetKotodama));
    }

    private IEnumerator TypeText(string message, Kotodama targetKotodama)
    {
        displayText.text = string.Empty;

        if (string.IsNullOrEmpty(message))
        {
            typingCoroutine = null;
            yield break;
        }

        for (int i = 0; i < message.Length; i++)
        {
            UpdateAlertByCurrentChar(message, i, targetKotodama);
            displayText.text += message[i];
            yield return new WaitForSeconds(charDelay);
        }

        typingCoroutine = null;
    }

    private void UpdateAlertByCurrentChar(string message, int index, Kotodama targetKotodama)
    {
        if (targetKotodama == null)
        {
            return;
        }

        bool isKyoKotoba = IsKyoKotobaStart(message, index, targetKotodama.KyoKotoba);
        float alertDelta = isKyoKotoba ? targetKotodama.KyoAlert : targetKotodama.CommonAlert;
        ApplyAlertDelta(alertDelta);
    }

    private void ApplyAlertDelta(float alertDelta)
    {
        float baseValue = alertSlider != null ? alertSlider.value : currentAlertValue;
        float nextValue = baseValue + alertDelta;
        bool reachedLimit = false;

        if (alertSlider != null)
        {
            if (nextValue >= alertSlider.maxValue)
            {
                nextValue = 0f;
                reachedLimit = true;
            }

            alertSlider.value = nextValue;
            nextValue = alertSlider.value;
        }
        else
        {
            if (nextValue >= alertLimitWithoutSlider)
            {
                nextValue = 0f;
                reachedLimit = true;
            }
        }

        currentAlertValue = nextValue;

        if (reachedLimit && onAlertLimitReached != null)
        {
            onAlertLimitReached.Invoke();
        }

        if (alertValueText != null)
        {
            alertValueText.text = nextValue.ToString("0.##");
        }
    }

    private bool IsKyoKotobaStart(string message, int index, string[] kyoKotobaList)
    {
        if (string.IsNullOrEmpty(message) || kyoKotobaList == null || kyoKotobaList.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < kyoKotobaList.Length; i++)
        {
            string kyoKotoba = kyoKotobaList[i];
            if (string.IsNullOrEmpty(kyoKotoba))
            {
                continue;
            }

            if (index + kyoKotoba.Length > message.Length)
            {
                continue;
            }

            if (string.Compare(message, index, kyoKotoba, 0, kyoKotoba.Length, StringComparison.Ordinal) == 0)
            {
                return true;
            }
        }

        return false;
    }

}