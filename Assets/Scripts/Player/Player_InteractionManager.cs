using System;
using System.Linq;
using UnityEngine;

public class Player_InteractionManager : MonoBehaviour
{
    public MemoryObj_Interact _currentObject { get; set; }
    public MemoryObj_Interact _currentHoldingObject { get; set; }
    public MemoryObj_Interact _previousHoldingObject { get; set; }
    public MemoryObj_Interact _previousObject { get; set; }

    public bool CanReleaseObject { get; set; } = true;
    public float InteractionDistance = 1.0f;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!_currentObject)
                return;

            if (!_currentObject._isBeingHold)
            {
                _currentHoldingObject = _currentObject;
                _currentObject.GotHold(transform);

            }
            else if (_currentObject._isBeingHold) 
            {
                if (CanReleaseObject) 
                {
                    _currentObject.GotReleased();
                    _currentHoldingObject = null;
                }
     
            }
        }

        _previousHoldingObject = _currentHoldingObject== null? _previousHoldingObject: _currentHoldingObject;
        CheckObjectHighlight();
       
    }
    void CheckObjectHighlight() 
    {
        _currentObject = GetCurrentMemoryObj();
        _currentObject?.GetComponent<MemoryObj_Effect>()?.Highlight(true);
        if (_currentObject == null && _previousObject != null)
            _previousObject?.GetComponent<MemoryObj_Effect>()?.Highlight(false);
        else if (_currentObject != _previousObject && _currentObject != null && _previousObject != null)
            _previousObject?.GetComponent<MemoryObj_Effect>()?.Highlight(false);
        _previousObject = _currentObject;
    }

    MemoryObj_Interact GetCurrentMemoryObj() 
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, InteractionDistance, LayerMask.GetMask("Interactable"));
        MemoryObj_Interact[] mems = objs.Select(x => x.GetComponent<MemoryObj_Interact>()).ToArray();
        if (mems.Length == 0)
            return null;
        float dist = float.MaxValue;
        MemoryObj_Interact selected = null;
        foreach (MemoryObj_Interact m in mems) 
        {
            float current = Vector3.Distance(transform.position, m.transform.position);
            if (current < dist) 
            {
                dist = current;
                selected = m;
            }
        }
        return selected;
    }

}
