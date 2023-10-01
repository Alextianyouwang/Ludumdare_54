
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MemoryObj : MonoBehaviour
{
    public bool Activatable;
    public bool Moveable;

    private bool _hasBeenActivated = false;
    private bool _isBeingHold = false;

    public Canvas testCanvas;

    public static Action<MemoryObj> OnInteract;

    public float DistToPlayer { get; set; } = 0f;

    private void Awake()
    {
        Highlight(false);
    }

    public void Activate() 
    {
        
     }

    public void Highlight(bool value) 
    {
        testCanvas.enabled = value;
    }

    
    private void OnTriggerEnter(Collider other)
    {

    }
    private void Update()
    {
        
    }
}
