using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    private InputSystem_Actions mInputAction;
    [SerializeField] GameObject mCamera;

    [SerializeField] private float Speed;
    private bool bFlipped;
    private float targetRotation;

    private void Awake()
    {
        mInputAction = new InputSystem_Actions();
        mInputAction.Player.SwitchCamera.performed += SwitchCamera;

        targetRotation = 45f;
    }

    private void SwitchCamera(InputAction.CallbackContext context)
    {
        bFlipped = !bFlipped;
        targetRotation = bFlipped ? 45 : 135;
    }

    private void Update()
    {
        float currentY = mCamera.transform.rotation.eulerAngles.y;

        float y = Mathf.LerpAngle(currentY, targetRotation, Time.deltaTime * Speed);

        mCamera.transform.rotation = Quaternion.Euler(mCamera.transform.rotation.eulerAngles.x,y, mCamera.transform.rotation.eulerAngles.z);
    }

    private void OnEnable() => mInputAction.Enable();
    private void OnDisable() => mInputAction.Disable();
}
