using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float speed;
    [SerializeField] private float rate;
    private float height;
    private CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        height = virtualCamera.m_Lens.OrthographicSize;
    }

    void LateUpdate()
    {
        height += Input.GetAxis("liftCamera") * speed * Time.deltaTime;
        if (height > maxHeight) height = maxHeight;
        if (height < minHeight) height = minHeight;
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, height, Time.deltaTime * rate);
    }
}
