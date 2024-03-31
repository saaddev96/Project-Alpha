using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemaMachinePOVExtension : CinemachineExtension
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform FPS_Pivot;
    [SerializeField] private Transform Player;
    [SerializeField] private float maxLookAngle = 85;
    [SerializeField] private float _mouseSensitivityX;
    [SerializeField] private float _mouseSensitivityY;
    private Vector3 rotation;
    private Vector2 _lookDirection;

    protected override void OnEnable()
    {
        base.OnEnable();
        _inputReader.LookEvent += HandleLookInput;
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (rotation == null) rotation = transform.localRotation.eulerAngles;
                rotation.x += _lookDirection.y * Time.deltaTime * _mouseSensitivityY;
                rotation.y += _lookDirection.x * Time.deltaTime * _mouseSensitivityX;
                rotation.x = Mathf.Clamp(rotation.x, -maxLookAngle, maxLookAngle);
                state.RawOrientation = Quaternion.Euler(-rotation.x, rotation.y, 0f);
                FPS_Pivot.localRotation = Quaternion.Euler(-rotation.x, 0f, 0f);
                Player.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }

    }



    void HandleLookInput(Vector2 dir)
    {
        _lookDirection = dir;
     
    }

    protected  void OnDisable()
    {
        _inputReader.LookEvent -= HandleLookInput;
    }
}
