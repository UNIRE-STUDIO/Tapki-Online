using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private TankMovement targetTankMovement;

    [SerializeField] private string m_MovementAxisName = "Vertical";
    [SerializeField] private string m_TurnAxisName = "Horizontal";

    private void Start()
    {
        
    }

    private void Update()
    {
        targetTankMovement.SetAxis(Input.GetAxis(m_MovementAxisName), Input.GetAxis(m_TurnAxisName));
    }

    private void FixedUpdate()
    {
        targetTankMovement.Move();
        targetTankMovement.Turn();
    }
}
