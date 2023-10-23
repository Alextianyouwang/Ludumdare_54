
using UnityEngine;

[CreateAssetMenu(menuName = "Memory Object Actions/Unveil")]
public  class Act_Unveil : Action_Base
{
    public string[] objectToUnveil;
    public override void Act(MemoryObj memObj) 
    {
        foreach (string s in objectToUnveil) 
        {
            //Debug.Log(objectToUnveil);
        }
    }
}
