using UnityEngine;


public class Camera_MovementController : MonoBehaviour
{
    private void OnEnable()
    {
        Room_Controller.OnSetCameraPos += PassiveMove;
        Room_Controller.OnRequestCameraPos += SharePosition;
    }

    private void OnDisable()
    {
        Room_Controller.OnSetCameraPos -= PassiveMove;
        Room_Controller.OnRequestCameraPos -= SharePosition;

    }

    void PassiveMove(Vector3 target)
    {
        transform.position = target;

    }
    Vector3 SharePosition()
    {
        return transform.position;
    }
}
