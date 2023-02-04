using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProvider : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<AudioClip> _meows;

    public void PlayMeow()
    {
        _source.PlayOneShot(_meows[Random.Range(0, _meows.Count)]);
    }
}
