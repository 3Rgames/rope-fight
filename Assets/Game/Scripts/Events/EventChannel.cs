using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Event Channel")]
public class EventChannel : ScriptableObject
{
    public UnityAction<float> OnEventFloat;
    public UnityAction<int> OnEventInt;
    public UnityAction<bool> OnEventBool;
    public UnityAction OnEvent;

    public void DoEvent(bool _)
    {
        OnEventBool?.Invoke(_);
    }

    public void DoEvent(int _)
    {
        OnEventInt?.Invoke(_);
    }

    public void DoEvent(float _)
    {
        OnEventFloat?.Invoke(_);
    }

    public void DoEvent()
    {
        OnEvent?.Invoke();
    }
}
