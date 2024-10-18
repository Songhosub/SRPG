using UnityEngine;
using UnityEngine.EventSystems;

public class FallowCamera : MonoBehaviour
{
    Vector2 clickPoint;
    float dragSpeed = 10.0f;
    bool uiClick = false;

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                uiClick = true;
            }

            else
            {
                uiClick = false;
            }

            clickPoint = Input.mousePosition;
        }

        if (!uiClick)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 position = Camera.main.ScreenToViewportPoint(clickPoint - (Vector2)Input.mousePosition);

                Vector3 move = position * (Time.deltaTime * dragSpeed);

                float z = transform.position.z;

                transform.Translate(move);
                //transform.transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }
        

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float size = GetComponent<Camera>().orthographicSize;
            size += Input.GetAxis("Mouse ScrollWheel");
            if (size <= 5 && size >= 2)
            {
                GetComponent<Camera>().orthographicSize = size;
            }            
        }
    }

    public void TargetPosition(Transform target)
    {
        transform.position = target.position + Vector3.forward * -5;
    }
}
