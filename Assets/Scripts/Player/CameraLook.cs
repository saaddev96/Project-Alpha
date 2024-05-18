using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private InputReader _inputReader;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform FPS_CameraRot;
    [SerializeField] private float maxLookAngle = 85;
    [SerializeField] private float _mouseSensitivityX = 10;
    [SerializeField] private float _mouseSensitivityY = 10;
    private Vector3 rotation;
    private Vector2 _lookDirection;

    public Vector3 Rotation => rotation;
    public Vector3 lookDir => _lookDirection;
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }
    private void OnEnable()
    {
        _inputReader.LookEvent += HandleLookInput;
    }
    protected void OnDisable()
    {
        _inputReader.LookEvent -= HandleLookInput;
    }
    private void Update()
    {
        Look();
    }
    void HandleLookInput(Vector2 dir)
    {
        _lookDirection = dir;
       

    }
    void Look()
    {

        if (rotation == null) rotation = transform.localRotation.eulerAngles;
        rotation.x += _lookDirection.y * Time.deltaTime * _mouseSensitivityY;
        rotation.y += _lookDirection.x * Time.deltaTime * _mouseSensitivityX;
        rotation.x = Mathf.Clamp(rotation.x, -maxLookAngle, maxLookAngle);
        FPS_CameraRot.localRotation = Quaternion.Euler(-rotation.x,0f, 0f);
        Player.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }
}
