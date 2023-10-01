
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _horizontal, _vertical;
    [SerializeField] private float _moveSpeed = 1;
    private Rigidbody _rb;
    private bool _allowMovement = true;
    private GameObject _duplicant;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        SetupDuplicant();
    }
    private void Update()
    {
 
        if (_allowMovement)
        CheckInput();
    }
    private void OnEnable()
    {

        RoomSwitch.OnStartSlide += PrepareTeleport;
        RoomSwitch.OnSliding += Teleport;
        RoomSwitch.OnEndSlide += FinishTeleport;
    }
    private void OnDisable()
    {

        RoomSwitch.OnStartSlide -= PrepareTeleport;
        RoomSwitch.OnSliding -= Teleport;
        RoomSwitch.OnEndSlide -= FinishTeleport;

    }
    void SetupDuplicant() 
    {
        _duplicant = new GameObject();
        _duplicant.transform.SetParent(GameObject.Find("--- Actors ---").transform);
        _duplicant.name = "Player_Shadow";
        _duplicant.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
        _duplicant.AddComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
        _duplicant.SetActive(false);
    }
    Vector3 PrepareTeleport() 
    {
        return transform.position;
    }
    Vector3 Teleport(Vector3 target,Vector3 origin) 
    {
        _rb.isKinematic = true;
        _rb.Sleep();
        _duplicant?.SetActive(true);
        _duplicant.transform.position = origin;
        transform.position = target;
        return transform.position;

    }
    Vector3 FinishTeleport() 
    {
        _rb.isKinematic = false;
        _duplicant.SetActive(false);
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
