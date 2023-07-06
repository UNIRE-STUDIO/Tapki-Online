using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UI_Element : MonoBehaviour
{
    public enum SwitchingMode
    {
        Simple,
        AnimationsUsingScripts,
        AnimationsUsingAnimator
    }
    public SwitchingMode currentMode;

    [Header("Simple")]
    public GameObject[] enabledObjects;  // (Нельзя менять на UnityEvent, т.к. при старте (awake) мы должны отключать и включать элементы)
    public GameObject[] disableObjects;  // согласно стартовому меню

    public UnityEvent turnOnOtherEvents;  // <--------
    public UnityEvent turnOffOtherEvents;

    [Header("Animations Using Animator")]
    public Animator[] animators;
    public string nameKey = "isActive";

    public delegate void TurnAnim();
    public TurnAnim turnOn;
    public TurnAnim turnOff;

    [HideInInspector] public bool isActive;
    [HideInInspector] public UI_UpdatePosition ui_UpdatePosition;

    private UI_Element[] subElements;

    //
    public delegate void SomeAction();
    public event SomeAction OnActive;
    public event SomeAction OffActive;

    private void Awake()
    {
        if (enabledObjects.Length > 0) isActive = enabledObjects[0].activeInHierarchy;
        UI_Element[] childs = new UI_Element[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<UI_Element>();
        }
        subElements = childs;
    }

    public virtual void TurnOn()
    {
        if (isActive) return;

        isActive = true;
        switch (currentMode)
        {
            case SwitchingMode.Simple:
                ActiveObjects(true);
                break;
            case SwitchingMode.AnimationsUsingScripts:
                turnOn?.Invoke();
                break;
            case SwitchingMode.AnimationsUsingAnimator:
                foreach (var a in animators)
                {
                    a.SetBool(nameKey, true);
                }
                break;
        }
        OnActive?.Invoke();
        turnOnOtherEvents.Invoke();
    }

    public virtual void TurnOff()
    {
        if (!isActive) return;
        isActive = false;
        
        OffActive?.Invoke();
        turnOffOtherEvents.Invoke();

        switch (currentMode)
        {
            case SwitchingMode.Simple:
                ActiveObjects(false);
                break;
            case SwitchingMode.AnimationsUsingScripts:
                turnOff?.Invoke();
                break;
            case SwitchingMode.AnimationsUsingAnimator:
                foreach (var a in animators)
                {
                    a.SetBool(nameKey, true);
                }
                break;
        }
    }

    public virtual void ActiveObjects(bool active)
    {
        foreach (var a in enabledObjects)
        {
            a.SetActive(active);
        }

        foreach (var a in disableObjects)
        {
            a.SetActive(!active);
        }
        if (subElements.Length > 0)
        {
            foreach (UI_Element a in subElements)
            {
                a.TurnOff();
            }
        }
    }

    public void SwitchActive()
    {
        if (isActive) TurnOff();
        else TurnOn();
    }

    public void MoveElementOnWorldPoint(Vector3 newPosOnWorld, bool update = false) // Перемещаем элемент интерфейса
    {
        if (update)
        {
            OnUpdatePosition(newPosOnWorld);
        }
        else
        {
            enabledObjects[0].GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(newPosOnWorld);
        }
    }

    public void OnUpdatePosition(Vector3 newPos)
    {
        ui_UpdatePosition.enabled = true;
        ui_UpdatePosition.position = newPos;
        ui_UpdatePosition.onWorld = true;
    }
}
