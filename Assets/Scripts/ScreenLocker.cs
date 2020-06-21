using UnityEngine;

public class ScreenLocker : MonoBehaviour
{
    private Vector2 _screenSize;
    private float _playerWidth = 0;
    private float _playerHeight = 0;

    #region Lifecycle

    void Start()
    {
        CapsuleCollider collider = GetComponentInChildren<CapsuleCollider>();
        _playerWidth = collider.bounds.extents.x;
        _playerHeight = collider.bounds.extents.y;
        _screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, _screenSize.x + _playerWidth, _screenSize.x * -1 - _playerWidth);
        position.y = Mathf.Clamp(position.y, _screenSize.y + _playerHeight, _screenSize.y * -1 - _playerHeight);
        transform.position = position;
    }

    #endregion
}
