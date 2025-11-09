using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;

    private Vector2 rotation = Vector2.zero;

    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        // --- Movement ---
        Vector3 move = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) move += transform.forward;
        if (Keyboard.current.sKey.isPressed) move -= transform.forward;
        if (Keyboard.current.aKey.isPressed) move -= transform.right;
        if (Keyboard.current.dKey.isPressed) move += transform.right;
        transform.position += move * moveSpeed * Time.deltaTime;

        // --- Mouse Look ---
        rotation.x += Mouse.current.delta.x.ReadValue() * lookSpeed;
        rotation.y -= Mouse.current.delta.y.ReadValue() * lookSpeed;
        rotation.y = Mathf.Clamp(rotation.y, -80f, 80f);
        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0f);
    }
}
