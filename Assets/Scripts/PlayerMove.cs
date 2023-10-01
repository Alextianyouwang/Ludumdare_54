using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    public bool allowMovement = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        CheckInput();
    }
    private void FixedUpdate()
    {
        if (allowMovement)
            Movement();
        else
            PassiveMovement();
    }
    private void CheckInput() 
    {
        _horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        //vertical = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
    }
    private void Movement()
    {
        _rb.isKinematic = false;
        Vector3 force = (_horizontal + _vertical).normalized * _moveSpeed;
        _rb.AddForce(force, ForceMode.Force);
    }
    private void PassiveMovement() 
    {
        _rb.isKinematic = true;

    }
}
