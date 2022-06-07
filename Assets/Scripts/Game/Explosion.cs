using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundClip;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        if (_audioSource == null)
        {
            Debug.Log("The audio source is null");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }
        
        _audioSource.Play();
        
        Destroy(this.gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
