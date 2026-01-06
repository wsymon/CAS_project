using UnityEngine;

public class EffectPlayerMap : MonoBehaviour
{
    [SerializeField]
    AudioClip UIClickClip;

    [SerializeField]
    AudioClip ConstructionClip;

    [SerializeField]
    AudioSource EffectsPlayer;

    void Start()
    {
        EffectsPlayer.volume = 0.6f;
        EffectsPlayer.clip = UIClickClip;
    }

    public void UISoundEffect()
    {
        EffectsPlayer.clip = UIClickClip;
        EffectsPlayer.Play();

    }

    public void ConstructionSoundEffect()
    {
        EffectsPlayer.clip = ConstructionClip;
        EffectsPlayer.Play();
    }
}