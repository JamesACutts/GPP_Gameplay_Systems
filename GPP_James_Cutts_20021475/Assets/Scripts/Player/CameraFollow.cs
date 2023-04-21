using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] 
    Transform playerCam;
    [SerializeField]
    private float sensitivity = 0.5f;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float height = 2.0f;
    [SerializeField]
    private float minDistance = 4.0f;
    [SerializeField]
    private float maxDistance = 10.0f;
    [SerializeField]
    private float minHeight = 0.5f;
    [SerializeField]
    private float maxHeight = 5.0f;
    [SerializeField]
    private float smoothSpeed = 10.0f;


    private Vector2 rotation = Vector2.zero;
    private float currentDistance = 0.0f;
    private float currentHeight = 0.0f;

    void Start()
    {
        currentDistance = distance;
        currentHeight = height;
    }

    void LateUpdate()
    {
        Gamepad joyStick = Gamepad.current;
        Vector2 input = Vector2.zero;
        if (joyStick != null && joyStick.rightStick.IsActuated())
        {
            input = joyStick.rightStick.ReadValue();
        }
        else
        {
            input = Mouse.current.delta.ReadValue() * sensitivity;
        }

        input *= sensitivity;
        input.y *= -1;

        rotation += input;
        rotation.y = Mathf.Clamp(rotation.y, -80.0f, 80.0f);

        Vector3 position = playerCam.TransformPoint(new Vector3(0, currentHeight, -currentDistance));
        Quaternion rotationQuaternion = Quaternion.Euler(rotation.y, rotation.x, 0.0f);
        position = playerCam.position + rotationQuaternion * new Vector3(0.0f, height, -distance);

        RaycastHit hit;
        if (Physics.Linecast(playerCam.position, position, out hit))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            currentHeight = Mathf.Clamp(hit.point.y - playerCam.position.y, minHeight, maxHeight);
            position = playerCam.position + rotationQuaternion * new Vector3(0.0f, currentHeight, -currentDistance);
        }

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationQuaternion, Time.deltaTime * smoothSpeed);
    }
}
