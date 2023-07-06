using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI_Element))]
public class UI_DragElement : MonoBehaviour
{
    [SerializeField] private RectTransform targetElement;
    private bool isDrag;
    private Vector2 offset;
    
    private void Awake()
    {
        this.enabled = false;
    }

    private void OnEnable() 
    {
        isDrag = false;
    }

    public void PointerDown()
    {
        //mousePosition = Input.mousePosition/screenResolution * canvasResolution;

        offset = new Vector2(Input.mousePosition.x - targetElement.position.x, Input.mousePosition.y - targetElement.position.y);   
        isDrag = true;
    }

    public void PointerUp()
    {
        isDrag = false;
    }

    private void Update()
    {
        if (isDrag)
        {
            targetElement.position = (Vector2)Input.mousePosition-offset;
        }
    }
}
