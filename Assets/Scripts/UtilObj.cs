using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UtilObj : MonoBehaviour
{
    private MemoryObj currentObj;
    private void Start()
    {
        
    }

    void MemoryObjRelease() 
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MemoryObj>()) 
        {
            currentObj = other.GetComponent<MemoryObj>();
            currentObj.OnRelease += MemoryObjRelease;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MemoryObj>())
        {
            currentObj = other.GetComponent<MemoryObj>();
            currentObj.OnRelease -= MemoryObjRelease;
            currentObj = null;
        }
    }
}
