using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SettingAnim : MonoBehaviour
{
    [SerializeField] private GameObject _setting;
    [SerializeField] private GameObject _audio;
    [SerializeField] private GameObject _vibro;
    [Space]
    [SerializeField] private float _showSettingTime = 1f;
    [SerializeField] private float _hideSettingTime = 1f;
    private float _startPosX;
    private float _endPosX;
    private Coroutine _cor;
    private bool _isHide = true;

    private void Start()
    {
        _startPosX = _audio.transform.position.x;
        _endPosX = _setting.transform.position.x;
    }

    public void Toggle()
    {
        if(_isHide)
        {
            if (ShowSetting())
                _isHide = false;
        }
        else
        {
            if (HideSetting())
                _isHide = true;
        }
    }

    private bool ShowSetting()
    {
        IEnumerator ShowSettingCor()
        {

            for (float t = 0; t < 1; t += Time.deltaTime / _showSettingTime)
            {
                _audio.transform.position = Vector3.Lerp(new Vector3(_startPosX, _audio.transform.position.y, _audio.transform.position.z), new Vector3(_endPosX, _audio.transform.position.y, _audio.transform.position.z), t);
                _vibro.transform.position = Vector3.Lerp(new Vector3(_startPosX, _vibro.transform.position.y, _vibro.transform.position.z), new Vector3(_endPosX, _vibro.transform.position.y, _vibro.transform.position.z), t);
                yield return null;
            }

            _audio.transform.position = new Vector3(_endPosX, _audio.transform.position.y, _audio.transform.position.z);
            _vibro.transform.position = new Vector3(_endPosX, _vibro.transform.position.y, _vibro.transform.position.z);
            _cor = null;
            yield break;
        }

        if (_cor == null)
        {
            _cor = StartCoroutine(ShowSettingCor());
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool HideSetting()
    {
        IEnumerator HideSettingCor()
        {

            for (float t = 0; t < 1; t += Time.deltaTime / _hideSettingTime)
            {
                _audio.transform.position = Vector3.Lerp(new Vector3(_endPosX, _audio.transform.position.y, _audio.transform.position.z), new Vector3(_startPosX, _audio.transform.position.y, _audio.transform.position.z), t);
                _vibro.transform.position = Vector3.Lerp(new Vector3(_endPosX, _vibro.transform.position.y, _vibro.transform.position.z), new Vector3(_startPosX, _vibro.transform.position.y, _vibro.transform.position.z), t);
                yield return null;
            }

            _audio.transform.position = new Vector3(_startPosX, _audio.transform.position.y, _audio.transform.position.z);
            _vibro.transform.position = new Vector3(_startPosX, _vibro.transform.position.y, _vibro.transform.position.z);
            _cor = null;
            yield break;
        }

        if (_cor == null)
        {
            _cor = StartCoroutine(HideSettingCor());
            return true;
        }
        else
        {
            return false;
        }
    }
}
