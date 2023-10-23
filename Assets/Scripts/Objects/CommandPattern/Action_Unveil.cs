
using UnityEngine;

[CreateAssetMenu(menuName = "Memory Object Actions/Unveil")]
public  class Action_Unveil : Action_Base
{
    public string[] objectToUnveil;
    public override void Act(MemoryObj_Core memObj) 
    {
        foreach (string s in objectToUnveil) 
        {
            //Debug.Log(objectToUnveil);
        }
    }
}
