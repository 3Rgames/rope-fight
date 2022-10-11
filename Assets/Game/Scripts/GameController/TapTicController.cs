using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TapTicController : MonoSingleton<TapTicController>
{
    [HorizontalLine(color: EColor.Blue)]
    public bool TapTicEnabled = true;
    [SerializeField, Foldout("Vibro Settings")] private Image _tapTicImage;
    [SerializeField, Foldout("Vibro Settings")] private Sprite _on, _off;
    public enum TapTicType { None, Heavy, Light, Medium, Success, Failure, Warning }

    private bool _isRecall = false;

    private void Start()
    {
        StartCoroutine(StartCor());
    }

    private IEnumerator StartCor()
    {
        yield return new WaitForFixedUpdate();
        _tapTicImage.sprite = TapTicEnabled ? _on : _off;
        yield break;
    }

    public void Vibro(TapTicType type)
    {
        if (!TapTicEnabled)
            return;

        DoVibration(type);
    }

    public void Vibro(TapTicType type, float recallTime)
    {
        if (!TapTicEnabled)
            return;

        IEnumerator Cor()
        {
            _isRecall = true;
            DoVibration(type);
            yield return new WaitForSeconds(recallTime);
            _isRecall = false;
        }
        if(!_isRecall)
            StartCoroutine(Cor());
    }

    private void DoVibration(TapTicType type)
    {
        switch (type)
        {
            case TapTicType.Heavy:
                Heavy(); break;
            case TapTicType.Light:
                Light(); break;
            case TapTicType.Medium:
                Medium(); break;
            case TapTicType.Success:
                Success(); break;
            case TapTicType.Failure:
                Failure(); break;
            case TapTicType.Warning:
                Warning(); break;
        }
    }

    private void Warning()
    {
        MMVibrationManager.Haptic(HapticTypes.Warning, false, true, this);
    }

    private void Failure()
    {
        MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
    }

    private void Success()
    {
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
    }

    private void Medium()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
    }

    private void Light()
    {
        MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
    }

    private void Heavy()
    {
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
    }

    public void Toggle()
    {
        TapTicEnabled = !TapTicEnabled;
        _tapTicImage.sprite = TapTicEnabled ? _on : _off;

        SLS.Instance.Save();
        Light();
    }

}

// TapTicController.Instance.Warning();       - to call TapTic Warning
// Yours ever 3R