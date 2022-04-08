using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public enum PanelFade
    {
        Instant,
        Timed,
    }

    private CanvasGroup m_canvasGroup = null;

    [SerializeField] protected PanelFade m_fadeType = PanelFade.Instant;

    [SerializeField] protected bool m_startOn = false;

    private bool m_isShowing = false;

    private IEnumerator m_fadeCoroutine = null;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is missing: " + name);
            return;
        }

        if (m_startOn)
        {
            m_isShowing = false;
            Show();
        }
        else
        {
            m_isShowing = true;
            Hide();
        }
    }

    public void Show()
    {
        if (m_canvasGroup == null) return;
        if (m_isShowing) return;
        m_isShowing = true;
        ChangePanelAlpha(0f, 1f);
        m_canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        if (m_canvasGroup == null) return;
        if (!m_isShowing) return;
        m_isShowing = false;
        ChangePanelAlpha(1f, 0f);
        m_canvasGroup.blocksRaycasts = false;
    }

    private void ChangePanelAlpha(float start, float target)
    {
        switch (m_fadeType)
        {
            case PanelFade.Instant:
                m_canvasGroup.alpha = target;
                break;
            case PanelFade.Timed:
                if (m_fadeCoroutine != null)
                    StopCoroutine(m_fadeCoroutine);
                float current = m_canvasGroup.alpha;
                float startTime = (target - current) / (target - start);
                m_fadeCoroutine = Fade(start, target, startTime);
                StartCoroutine(m_fadeCoroutine);
                break;
        }
    }

    private IEnumerator Fade(float start, float target, float startTime)
    {
        float t = startTime;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            m_canvasGroup.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }

}
