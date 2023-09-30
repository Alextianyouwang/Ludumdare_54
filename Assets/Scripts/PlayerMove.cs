using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 horizontal, vertical;
    [SerializeField] private float moveSpeed = 1;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        CheckInput();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void CheckInput() 
    {
        horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        //vertical = new Vector3(0f, 0f, Input.GetAxisRaw("Vertical"));
    }
    private void Movement()
    {
      
        Vector3 force = (horizontal + vertical).normalized * moveSpeed;
        rb.AddForce(force, ForceMode.Force);
    }
}
