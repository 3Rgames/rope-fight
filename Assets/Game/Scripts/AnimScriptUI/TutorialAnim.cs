using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnim : MonoBehaviour
{
    [SerializeField] private GameObject _hand;
    [SerializeField] private float _moveTime = 1f;
    [SerializeField] private float _moveDistance = 100f;
    [SerializeField] private float _curveZ = 15;
    private Coroutine _cor;
    private float _startPosX, _endPosX;
    private Quaternion _startRot, _endRot;

    //1920x1080 defaul screen param

    private void Start()
    {
        _startRot = Quaternion.Euler(0, 0, -_curveZ);
        _endRot = Quaternion.Euler(0, 0, _curveZ);

        float factorX = 1080f / Screen.width;
        _moveDistance = _moveDistance / factorX;

        _startPosX = _hand.transform.position.x - _moveDistance;
        _endPosX = _hand.transform.position.x + _moveDistance;

        _hand.transform.position = new Vector3(_startPosX, _hand.transform.position.y, _hand.transform.position.z);
    }

    private void FixedUpdate()
    {
        MoveHand();
    }

    private void MoveHand()
    {
        IEnumerator MoveHandCor()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / (_moveTime/2f))
            {
                _hand.transform.position = Vector3.Lerp(new Vector3(_startPosX, _hand.transform.position.y, _hand.transform.position.z), new Vector3(_endPosX, _hand.transform.position.y, _hand.transform.position.z), t);
                _hand.transform.rotation = Quaternion.Lerp(_startRot, _endRot, t);
                yield return null;
            }
            _hand.transform.rotation = _endRot;
            _hand.transform.position = new Vector3(_endPosX, _hand.transform.position.y, _hand.transform.position.z);

            for (float t = 0; t < 1; t += Time.deltaTime / (_moveTime / 2f))
            {
                _hand.transform.position = Vector3.Lerp(new Vector3(_endPosX, _hand.transform.position.y, _hand.transform.position.z), new Vector3(_startPosX, _hand.transform.position.y, _hand.transform.position.z), t);
                _hand.transform.rotation = Quaternion.Lerp(_endRot, _startRot, t);
                yield return null;
            }
            _hand.transform.rotation = _startRot;
            _hand.transform.position = new Vector3(_startPosX, _hand.transform.position.y, _hand.transform.position.z);

            _cor = null;
            yield break;
        }

        if (_cor == null)
            _cor = StartCoroutine(MoveHandCor());
    }

    private void OnDisable()
    {
        _cor = null;
    }

}
