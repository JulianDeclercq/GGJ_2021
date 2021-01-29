using UnityEngine;

public class MouseDraggable : MonoBehaviour
{
    private Vector3 _offset;
    private float _zCoord;

    private void OnMouseDown()
    {
        _zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        _offset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + _offset;
    }
}