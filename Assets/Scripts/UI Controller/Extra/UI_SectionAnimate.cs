using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UI_Element))]
public class UI_SectionAnimate : MonoBehaviour
{
    public float speedTurnOn;
    public float speedTurnOff;

    private UI_Element _uiElement;

    private const string DelayedTurnOffname = "DelayedTurnOff";
    
    private void Awake()
    {
        _uiElement = GetComponent<UI_Element>();
        _uiElement.turnOn += TurnOn;
        _uiElement.turnOff += TurnOff;
    }

    protected virtual void TurnOn()
    {
        StopCoroutine(DelayedTurnOffname);
        _uiElement.ActiveObjects(true);

    }

    protected virtual void TurnOff()
    {
        StopCoroutine(DelayedTurnOffname);
        StartCoroutine(DelayedTurnOffname);
    }

    protected virtual IEnumerator DelayedTurnOff()
    {
        yield return new WaitForSeconds(speedTurnOff);
        _uiElement.ActiveObjects(false);
    }
}
