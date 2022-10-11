using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Input = UnityEngine.Input;

namespace Game
{
    [Serializable]
    public class InputData
    {
        [SerializeField] bool _isTouching;
        [SerializeField] float _touchTimeScaled;
        [SerializeField] float _touchTimeUnscaled;
        [SerializeField] Vector2? _previousTouchPosition;
        [SerializeField] float _summaryDragDistance;
        [SerializeField] Vector2 _dragDelta;

        public bool IsTouching => _isTouching;
        public float TouchTimeScaled => _touchTimeScaled;
        public float TouchTimeUnscaled => _touchTimeUnscaled;
        public float SummaryDragDistance => _summaryDragDistance;
        public Vector2? PrevMousePos => _previousTouchPosition;

        public Vector2 DragDelta => _dragDelta;
        public Vector2 TouchPosition => Input.mousePosition;
        public Vector2 TouchPositioPointer => InputController.ScreenPointToViewport(Input.mousePosition);
        public float TouchDurationScaled => Time.time - _touchTimeScaled;
        public float TouchDurationUnscaled => Time.unscaledTime - _touchTimeUnscaled;


        public InputData()
        {
            SetDefaultValues();
        }

        public InputData(float time, float unscaledTime)
        {
            SetDefaultValues();
            _touchTimeScaled = time;
            _touchTimeUnscaled = unscaledTime;
        }

        public InputData(InputData inputData, Vector2 mousePos)
        {
            _isTouching = true;

            Vector2 delta;
            if (inputData._previousTouchPosition.HasValue)
                delta = mousePos - inputData._previousTouchPosition.Value;
            else
                delta = Vector2.zero;

            _dragDelta = delta;
            _summaryDragDistance = inputData._summaryDragDistance + delta.magnitude;

            _touchTimeScaled = inputData._touchTimeScaled;
            _touchTimeUnscaled = inputData._touchTimeUnscaled;

            _previousTouchPosition = mousePos;
        }

        private void SetDefaultValues()
        {
            _isTouching = false;
            _touchTimeScaled = 0;
            _touchTimeUnscaled = 0;
            _summaryDragDistance = 0f;
            _previousTouchPosition = null;
            _dragDelta = Vector2.zero;
        }
    }

    public class InputController : MonoBehaviour
    {
        public static InputController Instance { get; private set; }
        
        public InputData InputData { get; private set; }

        public Action<InputData> OnTouched;
        public Action<InputData> OnTouchHold;
        public Action<InputData> OnTouchReleased;

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            InputData = new InputData();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InputData = new InputData(Time.time, Time.unscaledTime);
                OnTouched?.Invoke(InputData);
            }

            if (Input.GetMouseButton(0))
            {
                OnTouchHold?.Invoke(InputData);
                InputData = new InputData(InputData,
                    ScreenPointToViewport(UnityEngine.Input.mousePosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnTouchReleased?.Invoke(InputData);
                InputData = new InputData(Time.time, Time.unscaledTime);
            }
        }

        public static Vector2 ScreenPointToViewport(Vector2 screenPoint)
        {
            return new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        }
    }
}