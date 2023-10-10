
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    private Renderer _playerRenderer;
    private MaterialPropertyBlock _playerMbp;
    private void Awake()
    {
        _playerMbp = new MaterialPropertyBlock();
        _playerRenderer = FindObjectOfType<PlayerMove>().GetComponent<Renderer>();
    }
    private void OnEnable()
    {
        PlayerMove.OnShareTotalDistTravel += ChangePlayerMaterialPlaySpeed;
    }
    private void OnDisable()
    {
        PlayerMove.OnShareTotalDistTravel-= ChangePlayerMaterialPlaySpeed;
    }

    void ChangePlayerMaterialPlaySpeed(float speed) 
    {
        _playerMbp.SetFloat("_FlipBookFrame", speed * 7f);
        _playerRenderer.SetPropertyBlock(_playerMbp);
    }

}
