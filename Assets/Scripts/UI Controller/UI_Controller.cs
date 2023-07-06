using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : Singleton<UI_Controller>
{

    public UI_Switch[] switches;   // Переключатели секций
    public UI_Element[] uI_Elements; // отдельные UI элементы которые нужно включить или выключить программно

    [HideInInspector] public int currentSection; // Текущая секция
    private bool blockBack = false;

    protected void Start()
    {
        DontDestroyOnLoad(this);

        foreach (var item in switches) //Для того что бы в начале всегда включалась первая секция
        {
            item.TurnOff();
        }
        switches[0].TurnOn();
    }

    public void TurnOnSection(int idSection)
    {
        if ((int) currentSection == idSection) return;
        switches[currentSection].TurnOff();
        currentSection = idSection;
        switches[idSection].TurnOn();
    }
    
    public void Back()
    {
        if (blockBack) return;
        else if (currentSection == 0) Application.Quit();
        TurnOnSection(switches[currentSection].idBack);
        StartCoroutine(BlockBack());
    }

    // Ненадолго отключаем кнопку назад, во избежание случайного двойного нажатия
    IEnumerator BlockBack ()
    {
        blockBack = true;
        yield return new WaitForSeconds(0.6f);
        blockBack = false;
    }

    // Для управления через другие скрипты .........................

    public void SwitchElement(int idElement)
    {
        if (uI_Elements[idElement].isActive) uI_Elements[idElement].TurnOff();
        else uI_Elements[idElement].TurnOn();
    }

    public void SwitchElement(int idElement, bool isActive)
    {
        if (!isActive) uI_Elements[idElement].TurnOff();
        else uI_Elements[idElement].TurnOn();
    }

    public void SwitchElement(int idElement, bool isActive, Vector3 newPosOnWorld, bool update) ///Надо тестить!
    {
        uI_Elements[idElement].MoveElementOnWorldPoint(newPosOnWorld, true);
        if (!isActive) uI_Elements[idElement].TurnOff();
        else uI_Elements[idElement].TurnOn();
    }

    // ..............................................
}