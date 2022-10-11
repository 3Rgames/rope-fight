using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class LoseAnim : MonoBehaviour
{
    [SerializeField] private Image _loseBG;
    [SerializeField] private float _timeToShowLoseBG = 0.5f;
    [SerializeField] private Color _withA, _withoutA;
    private Coroutine _showBGcor;
    [Space]
    [Space]
    [SerializeField] private GameObject _button;
    [SerializeField] private float _xOffsetButton = 400f;
    [SerializeField] private float _timeToShowButton = 1f;
    [SerializeField] private float _timeToScaleButton = 1f;
    [SerializeField] private Vector3 _buttonEndScale;
    private Vector3 _buttonStartScale = Vector3.one;
    private Vector3 _buttonStartPos, _buttonEndPos;
    private Coroutine _showButtonCor;
    [Space]
    [Space]
    [SerializeField] private GameObject _rate;
    [SerializeField] private float _xOffsetRate = 400f;
    [SerializeField] private float _timeToShowRate = 1f;
    [SerializeField] private float _timeToScaleRate = 1f;
    [SerializeField] private Vector3 _rateEndScale;
    private Vector3 _rateStartScale;
    private Vector3 _rateStartPos, _rateEndPos;
    private Coroutine _showRateCor;

    private void OnEnable()
    {
        OnStart();
        StartParams();
        ShowBG();
        ShowButton();
        ShowRate();
    }

    private void ShowBG()
    {
        IEnumerator ShowBGCor()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / _timeToShowLoseBG)
            {
                _loseBG.color = Color.Lerp(_withoutA, _withA, t);
                yield return null;
            }

            _loseBG.color = _withA;
            _showBGcor = null;
            yield break;
        }

        if (_showBGcor == null)
            _showBGcor = StartCoroutine(ShowBGCor());
    }

    private void ShowButton()
    {
        IEnumerator ShowButtonCor()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / _timeToShowButton)
            {
                _button.transform.position = Vector3.Lerp(_buttonStartPos, _buttonEndPos, t);
                yield return null;
            }
            _button.transform.position = _buttonEndPos;

            for (float t = 0; t < 1; t += Time.deltaTime / (_timeToScaleButton / 2f))
            {
                _button.transform.localScale = Vector3.Lerp(_buttonStartScale, _buttonEndScale, t);
                yield return null;
            }
            _button.transform.localScale = _buttonEndScale;

            for (float t = 0; t < 1; t += Time.deltaTime / (_timeToScaleButton / 2f))
            {
                _button.transform.localScale = Vector3.Lerp(_buttonEndScale, _buttonStartScale, t);
                yield return null;
            }
            _button.transform.localScale = _buttonStartScale;

            _showButtonCor = null;
            yield break;
        }

        if (_showButtonCor == null)
            _showButtonCor = StartCoroutine(ShowButtonCor());
    }

    private void ShowRate()
    {
        IEnumerator ShowRateCor()
        {
            yield return new WaitForSeconds(_timeToShowButton);

            for (float t = 0; t < 1; t += Time.deltaTime / _timeToShowRate)
            {
                _rate.transform.position = Vector3.Lerp(_rateStartPos, _rateEndPos, t);
                yield return null;
            }
            _rate.transform.position = _rateEndPos;

            for (float t = 0; t < 1; t += Time.deltaTime / (_timeToScaleRate / 2f))
            {
                _rate.transform.localScale = Vector3.Lerp(_rateStartScale, _rateEndScale, t);
                yield return null;
            }
            _rate.transform.localScale = _rateEndScale;

            for (float t = 0; t < 1; t += Time.deltaTime / (_timeToScaleRate / 2f))
            {
                _rate.transform.localScale = Vector3.Lerp(_rateEndScale, _rateStartScale, t);
                yield return null;
            }
            _rate.transform.localScale = _rateStartScale;

            _showRateCor = null;
            yield break;
        }

        if (_showRateCor == null)
            _showRateCor = StartCoroutine(ShowRateCor());
    }

    private bool _wasStart = false;
    private void OnStart()
    {
        if (_wasStart)
            return;
        _wasStart = true;
        _rateStartScale = _rate.transform.localScale;

        float factorX = 1080f / Screen.width;

        float _moveDistance = _xOffsetButton / factorX;
        _buttonEndPos = _button.transform.position;
        _buttonStartPos = _buttonEndPos + (Vector3.right * _moveDistance);

        _moveDistance = _xOffsetRate / factorX;
        _rateEndPos = _rate.transform.position;
        _rateStartPos = _rateEndPos + (Vector3.left * _moveDistance);
    }

    private void StartParams()
    {
        _rate.transform.localScale = _rateStartScale;
        _rate.transform.position = _rateStartPos;
        _button.transform.position = _buttonStartPos;
        _button.transform.localScale = _buttonStartScale;
        _loseBG.color = _withoutA;
    }

    private void OnDisable()
    {
        _showBGcor = null;
        _showButtonCor = null;
        _showRateCor = null;
        StartParams();
    }
}
