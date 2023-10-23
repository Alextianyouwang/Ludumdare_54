
using UnityEngine;

public class Effect_Controller : MonoBehaviour
{
    private Renderer _playerRenderer;
    private MaterialPropertyBlock _playerMbp;
    private void Awake()
    {
        _playerMbp = new MaterialPropertyBlock();
        _playerRenderer = FindObjectOfType<Player_MovementController>().GetComponent<Renderer>();
    }
    private void OnEnable()
    {
        Player_MovementController.OnShareTotalDistTravel += ChangePlayerMaterialPlaySpeed;
    }
    private void OnDisable()
    {
        Player_MovementController.OnShareTotalDistTravel-= ChangePlayerMaterialPlaySpeed;
    }

    void ChangePlayerMaterialPlaySpeed(float speed) 
    {
        _playerMbp.SetFloat("_FlipBookFrame", speed * 7f);
        _playerRenderer.SetPropertyBlock(_playerMbp);
    }

}
