using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 mousePosition;

    private Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Ativar()
    {
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }
}
