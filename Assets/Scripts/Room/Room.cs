using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    public GameObject player { get; private set; }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag.Equals("Player")) 
        {
            player = other.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            player = null;

        }
    }
}
