using System;
using System.Linq;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private MemoryObj _currentObject,_previousObject;
    public float InteractionDistance = 1.0f;
    
    private void OnEnable()
    {
        MemoryObj.OnInteract += GetMemoryObj;
    }
    private void OnDisable()
    {
        
        MemoryObj.OnInteract -= GetMemoryObj;
    }
    public void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            if (!_currentObject)
                return;

          if ( !_currentObject._isBeingHold)
                _currentObject.GotHold(transform);
            else if ( _currentObject._isBeingHold)
                _currentObject.GotReleased();
        }
       
        CheckObjectHighlight();
       
    }
    void CheckObjectHighlight() 
    {
        _currentObject = GetCurrentMemoryObj();
        _currentObject?.Highlight(true);
        if (_currentObject == null && _previousObject != null)
            _previousObject.Highlight(false);
        else if (_currentObject != _previousObject && _currentObject != null && _previousObject != null) 
            _previousObject.Highlight(false);

        _previousObject = _currentObject;
    }

    MemoryObj GetCurrentMemoryObj() 
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, InteractionDistance, LayerMask.GetMask("Interactable"));
        MemoryObj[] mems = objs.Select(x => x.GetComponent<MemoryObj>()).ToArray();
        if (mems.Length == 0)
            return null;
        float dist = float.MaxValue;
        foreach (MemoryObj m in mems) 
        {
            dist = Mathf.Min(Vector3.Distance(transform.position, m.transform.position), dist);
            m.DistToPlayer = dist;
        }
        return mems.First(x => x.DistToPlayer == dist && x != null);
    }
    void GetMemoryObj(MemoryObj obj) 
    {
        print(obj.name);
    }
}
