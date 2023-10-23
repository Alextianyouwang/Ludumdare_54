
using UnityEngine;
using System;
using System.Collections;
using System.Linq;
public class MemoryObj : MonoBehaviour
{
    public bool _isBeingHold { get; private set; } = false;
    public bool _canBeRelease { get; private set; } = false;

    public MemObjData objData;
    private GameObject[] _memory_Objs_Prefab;
    private GameObject[] _memory_Objs;

    public Canvas TestCanvas;
    public GameObject ActivateEffect;

    public static Action<MemoryObj> OnInteract;
    public Action OnRelease;

    public Room InThisRoom { get; private set; }

    public float DistToPlayer { get; set; } = 0f;

    void Awake()
    {
        Highlight(false);
        PrepareMemObjs();
        GetComponent<MeshRenderer>().enabled = false;

    }

    
    void PrepareMemObjs()
    {
        _memory_Objs_Prefab = objData.roomSection.Select(x => x.DisplayObject).ToArray();
        _memory_Objs = new GameObject[_memory_Objs_Prefab.Length];
        for (int i = 0; i < _memory_Objs.Length; i++)
        {
            _memory_Objs[i] = Instantiate(_memory_Objs_Prefab[i]);
            _memory_Objs[i].SetActive(false);
            _memory_Objs[i].transform.parent = transform;
        }
    }

    public void ActivateFunction()
    {
        PlayEffect(Color.white);
    }

    public void PlayEffect(Color color)
    {
        GameObject effect = Instantiate(ActivateEffect);
        ParticleSystem fx = effect.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = fx.main;
        effect.transform.position = transform.position;
        main.startColor = color;
        fx.Play();
    }

    public void GotHold(Transform target)
    {

        if (_isBeingHold)
            return;
        if (!_isBeingHold)
            _isBeingHold = true;

        ActivateFunction();

        StartCoroutine(SnapToPos(target, 0.2f, StartContinueSnapToPos));
    }

    IEnumerator SnapToPos(Transform target, float time, Action<Transform> next)
    {
        float progress = 0;
        Vector3 startPos = transform.position;

        while (progress < time)
        {
            progress += Time.deltaTime;
            float percent = progress / time;
            transform.position = Vector3.Lerp(startPos, target.position, percent);
            yield return null;
        }
        next?.Invoke(target);
    }
    void StartContinueSnapToPos(Transform target)
    {
        StartCoroutine(ContinueSnapToPos(target, ContinueSnappingExitCondition, ResetCanbeRelease));
    }
    IEnumerator ContinueSnapToPos(Transform target, Func<bool> exitCondition, Action next)
    {
        while (!exitCondition.Invoke())
        {
            transform.position = target.position;
            yield return null;
        }
        next?.Invoke();
    }

    void ResetCanbeRelease()
    {
        _canBeRelease = false;
        _isBeingHold = false;
        ReleaseFunction();
    }

    void ReleaseFunction()
    {
        PlayEffect(Color.yellow);
        OnRelease?.Invoke();
    }

    public void GotReleased()
    {
        _canBeRelease = true;
    }

    bool ContinueSnappingExitCondition()
    {
        return _canBeRelease;
    }
    public void Highlight(bool value)
    {
        TestCanvas.enabled = value;
    }

    void CheckRoom()
    {
        if (RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[0])
        {
            _memory_Objs[0].SetActive(true);
            _memory_Objs[0].transform.position = transform.position;
            _memory_Objs[1].SetActive(false);
            _memory_Objs[2].SetActive(false);
        }
        else if (RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[1])
        {
            _memory_Objs[0].SetActive(false);
            _memory_Objs[1].SetActive(true);
            _memory_Objs[1].transform.position = transform.position;
            _memory_Objs[2].SetActive(false);
        }
        else if (RoomSwitch.GetRoomContainsPlayer() == RoomSwitch._StaticRooms[2])
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
