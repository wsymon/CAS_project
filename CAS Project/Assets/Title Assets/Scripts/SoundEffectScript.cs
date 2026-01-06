using UnityEngine;

public class SoundEffectScriptTitle : MonoBehaviour
{
[SerializeField]
    AudioClip UIClickClip;

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
}