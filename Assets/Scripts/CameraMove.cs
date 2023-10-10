using UnityEngine;


public class CameraMove : MonoBehaviour
{
    private void OnEnable()
    {
        RoomSwitch.OnSetCameraPos += PassiveMove;
        RoomSwitch.OnRequestCameraPos += SharePosition;
    }

    private void OnDisable()
    {
        RoomSwitch.OnSetCameraPos -= PassiveMove;
        RoomSwitch.OnRequestCameraPos -= SharePosition;

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
