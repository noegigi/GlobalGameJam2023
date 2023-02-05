using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventForwarder : MonoBehaviour
{
    public UnityEvent toForward;
    public UnityEvent midToForward;

    public void OnEvent()
    {
        toForward.Invoke();
    }

    public void OnMidEvent()
    {
        midToForward.Invoke();
    }
}
