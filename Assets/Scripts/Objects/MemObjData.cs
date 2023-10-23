using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Memory Object")]
public class MemObjData : ScriptableObject
{
    [SerializeField]
    public RoomSection[] roomSection;


    public GameObject ObjectToCombine;
    [Serializable]
    public class RoomSection 
    {
        public GameObject DisplayObject;
        public Action_Base DodoWhenEnter;
    }
}

