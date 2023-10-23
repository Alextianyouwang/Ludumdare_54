using UnityEngine;

public class MemoryObj_Effect : MonoBehaviour
{

    public Canvas TestCanvas;
    public GameObject ActivateEffect;

    private void Awake()
    {
        Highlight(false);
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


    public void Highlight(bool value)
    {
        TestCanvas.enabled = value;
    }

}
