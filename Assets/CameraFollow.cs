using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 角色
    public Vector3 offset = new Vector3(0, 8, -8); // 相机偏移
    public float smoothSpeed = 5f; // 跟随速度

    void LateUpdate()
    {
        if (!target)
        {
            Debug.LogWarning("Missing target!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        transform.LookAt(target);

        Debug.Log("Camera position: " + transform.position + " | Target: " + target.position);
    }
}