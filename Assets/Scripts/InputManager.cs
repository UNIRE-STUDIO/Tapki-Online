using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    // Есть идея сделать UnityEvent для определенного массива имен кнопок (Input.GetButton) для конкретной сцены
    //
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Start()
    {

    }

    void Update()
    {
        // Лучшая смена графики
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QualitySettings.SetQualityLevel(2, true);
        }
    }
}
