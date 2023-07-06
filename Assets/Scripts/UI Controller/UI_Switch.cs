using UnityEngine;
[System.Serializable]
public class UI_Switch
{
    [SerializeField] private string nameSection; // Нужно только для удобства в инспекторе
    [Space]
    [SerializeField] private UI_Element[] turnOn;

    public void TurnOn()
    {
        foreach (var a in turnOn)
        {
            a.TurnOn();
        }
    }

    public void TurnOff()
    {
        foreach (var a in turnOn)
        {
            a.TurnOff();
        }
    }
    public int idBack;
}
