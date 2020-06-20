using UnityEngine;

public class ScreenLocker : MonoBehaviour
{
    private Vector2 ScreenSize;
    private float PlayerWidth = 0;
    private float PlayerHeight = 0;

    void Start()
    {
        CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();
        PlayerWidth = collider.bounds.extents.x;
        PlayerHeight = collider.bounds.extents.y;
        ScreenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, ScreenSize.x + PlayerWidth, ScreenSize.x * -1 - PlayerWidth);
        position.y = Mathf.Clamp(position.y, ScreenSize.y + PlayerHeight, ScreenSize.y * -1 - PlayerHeight);
        transform.position = position;
    }
}
