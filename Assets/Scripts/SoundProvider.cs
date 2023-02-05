using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProvider : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<AudioClip> _meows;

    [SerializeField] AudioSource chargeAudio;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] AudioSource angryAudio;

    public void PlayMeow()
    {
        _source.PlayOneShot(_meows[Random.Range(0, _meows.Count)]);
    }

    public void PlayCharge(){
        chargeAudio.Play();
    }

    public void PlayImpact(){
        impactAudio.Play();
    }

    public void PlayAngry(){
        angryAudio.Play();
    }

}
