using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCam : MonoBehaviour
{
    [SerializeField]  RenderTexture renderTexture;
    void Start()
    {
        Camera renderCamera = GetComponent<Camera>();
        if (renderCamera != null)
        {
            renderCamera.targetTexture = renderTexture;

            // Render Camera ���� (�ʿ信 ���� ����)
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 5;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = Color.clear;
        }
        else
        {
            Debug.LogError("ī�޶� ã�� �� �����ϴ�.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
