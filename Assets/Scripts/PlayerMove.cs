using TreeEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    private bool _allowMovement = true;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (_allowMovement)
        CheckInput();
    }
    private void OnEnable()
    {
        RoomSwitch.OnStartSlide += PreparePasiveMovement;
        RoomSwitch.OnSliding += PassiveMovement;
        RoomSwitch.OnEndSlide += FinishPassiveMovement;
    }
    private void OnDisable()
    {
        RoomSwitch.OnStartSlide -= PreparePasiveMovement;
        RoomSwitch.OnSliding -= PassiveMovement;
        RoomSwitch.OnEndSlide -= FinishPassiveMovement;

    }

    Vector3 PreparePasiveMovement() 
    {
        return transform.position;
    }
    Vector3 PassiveMovement(Vector3 target) 
    {
        _rb.isKinematic = true;
        _rb.Sleep();
        transform.position = target;
        return transform.position;

    }
    Vector3 FinishPassiveMovement() 
    {
        _rb.isKinematic = false;
        _rb.WakeUp();
        return transform.position;
    }

    private void FixedUpdate()
    {
            Movement();

    }
    private void CheckInput() 
    {
        _horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        
    }
    private void Movement()
    {
        _rb.isKinematic = false;
        Vector3 force = (_horizontal + _vertical).normalized * _moveSpeed;
        _rb.AddForce(force, ForceMode.Force);
    }
    
}
