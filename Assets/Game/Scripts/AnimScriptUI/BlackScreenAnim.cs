using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class BlackScreenAnim : MonoBehaviour
{
    [SerializeField] private Image _blackScreenImage;
    [SerializeField] private GameObject _blackScreenObject;
    [Space]
    [SerializeField, Range(0, 10)] private float _timeToShowBlackScreen = 1f;
    [SerializeField, Range(0, 10)] private float _timeToHideBlackScreen = 1f;
  
    [SerializeField, Foldout("Colors")] private Color _withA, _withoutA;

    private Coroutine _showCor;
    private Coroutine _hideCor;

    public float ShowBlackScreen()
    {
        IEnumerator ShowBlackScreenCor()
        {
            _blackScreenObject.SetActive(true);

            for (float t = 0; t < 1; t += Time.deltaTime / _timeToShowBlackScreen)
            {
                _blackScreenImage.color = Color.Lerp(_withoutA, _withA, t);
                yield return null;
            }

            _blackScreenImage.color = _withA;
            _showCor = null;
            yield break;
        }

        if(_showCor == null)
            _showCor = StartCoroutine(ShowBlackScreenCor());
        return _timeToShowBlackScreen;
    }

    public float HideBlackScreen()
    {
        IEnumerator HideBlackScreenCor()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / _timeToHideBlackScreen)
            {
                _blackScreenImage.color = Color.Lerp(_withA, _withoutA, t);
                yield return null;
            }

            _blackScreenObject.SetActive(false);
            _blackScreenImage.color = _withoutA;
            _hideCor = null;  
            yield break;
        }

        if (_hideCor == null)
            _hideCor = StartCoroutine(HideBlackScreenCor());
        return _timeToHideBlackScreen;
    }
}
