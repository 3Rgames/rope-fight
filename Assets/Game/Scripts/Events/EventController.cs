using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoSingleton<EventController>
{
    public EventChannel OnWin;
    public EventChannel OnLose;
    public EventChannel OnTutorial;
}
