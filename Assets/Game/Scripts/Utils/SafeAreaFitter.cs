using System;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public sealed class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private RectTransform m_target;

    private void OnValidate()
    {
        if (m_target == null)
        {
            m_target = GetComponent<RectTransform>();
        }
    }

    private void Awake()
    {
        Fit();
    }

    [Button("Force Fit")]
    public void Fit()
    {
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        m_target.anchorMin = anchorMin;
        m_target.anchorMax = anchorMax;
    }
}