
using UnityEngine;
using System.Linq;
public class MemoryObj_Core : MonoBehaviour
{
    public MemoryObj_Data ObjData;
    private GameObject[] _memory_Objs_Prefab;
    private GameObject[] _memory_Objs;

    public Room InThisRoom { get; private set; }
    

    void Awake()
    {

        PrepareMemObjs();
        GetComponent<MeshRenderer>().enabled = false;

    }

    
    void PrepareMemObjs()
    {
        _memory_Objs_Prefab = ObjData.roomSection.Select(x => x.DisplayObject).ToArray();
        _memory_Objs = new GameObject[_memory_Objs_Prefab.Length];
        for (int i = 0; i < _memory_Objs.Length; i++)
        {
            _memory_Objs[i] = Instantiate(_memory_Objs_Prefab[i]);
            _memory_Objs[i].SetActive(false);
            _memory_Objs[i].transform.parent = transform;
        }
    }


    void CheckRoom()
    {
        if (Room_Controller.GetRoomContainsPlayer() == Room_Controller._StaticRooms[0])
        {
            _memory_Objs[0].SetActive(true);
            _memory_Objs[0].transform.position = transform.position;
            _memory_Objs[1].SetActive(false);
            _memory_Objs[2].SetActive(false);
            ObjData.roomSection[0].DodoWhenEnter?.Act(this);
        }
        else if (Room_Controller.GetRoomContainsPlayer() == Room_Controller._StaticRooms[1])
        {
            _memory_Objs[0].SetActive(false);
            _memory_Objs[1].SetActive(true);
            _memory_Objs[1].transform.position = transform.position;
            _memory_Objs[2].SetActive(false);
        }
        else if (Room_Controller.GetRoomContainsPlayer() == Room_Controller._StaticRooms[2])
        {
            _memory_Objs[0].SetActive(false);
            _memory_Objs[1].SetActive(false);
            _memory_Objs[2].SetActive(true);
            _memory_Objs[2].transform.position = transform.position;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Room>()) 
        {
            InThisRoom = other.GetComponent<Room>();
        }
    }
    void Update()
    {
        CheckRoom();
    }

}
