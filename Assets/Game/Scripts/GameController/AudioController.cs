using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoSingleton<AudioController>
{
    [HorizontalLine(color: EColor.Blue)]
    public bool AudioEnabled = true;
    [SerializeField, Foldout("Audio Settings")] private Image _audioImage;
    [SerializeField, Foldout("Audio Settings")] private Sprite _on, _off;
    [SerializeField, Foldout("Audio Settings")] private AudioSource _audioSource;
    //[SerializeField] private AudioClip _examplePlay;

    private void Start()
    {
        StartCoroutine(StartCor());
    }

    private IEnumerator StartCor()
    {
        yield return new WaitForFixedUpdate();
        _audioImage.sprite = AudioEnabled ? _on : _off;
        yield break;
    }

    /*public void ExamplePlay()
    {
        SoundPlay(_examplePlay);
    }*/

    private void SoundPlay(AudioClip _)
    {
        _audioSource.clip = _;
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
    }

    public void Toggle()
    {
        AudioEnabled = !AudioEnabled;
        _audioImage.sprite = AudioEnabled ? _on : _off;

        SLS.Instance.Save();

        TapTicController.Instance.Vibro(TapTicController.TapTicType.Light);
    }

}

// AudioController.Instance.BGSoundPlay();       - to start BGSound
// Yours ever 3R
