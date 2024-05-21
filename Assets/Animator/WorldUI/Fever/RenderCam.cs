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

            // Render Camera 설정 (필요에 따라 조정)
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 5;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = Color.clear;
        }
        else
        {
            Debug.LogError("카메라를 찾을 수 없습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
