using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_Element))]
public class UI_UpdatePosition : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool onWorld;
    public RectTransform targetRectTransform;

    private UI_Element parent;
    private Camera cam;

    private void Awake()
    {
        parent = GetComponent<UI_Element>();
        parent.ui_UpdatePosition = this;
    }

    private void OnEnable() 
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (onWorld)
        {
            targetRectTransform.position = cam.WorldToScreenPoint(position);
        }
        else
        {
            
        }
    }

}
