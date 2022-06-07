using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSoundClip;
    
    [SerializeField] private float _enemySpeed = 4.0f;

    private Player _player;
    private Animator _animator;
    
    private bool _isEnemyDead = false;


    // Start is called before the first frame update
    void Start() 
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = transform.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log("Player not found");
        }
        
        if (_animator == null)
        {
            Debug.Log("Animator not found");
        }
        
        if (_audioSource == null)
        {
            Debug.Log("The audio source is null");
        }
        else
        {
            _audioSource.clip = _explosionSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        RespawnBeyondScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyDead)
            return;
        
        if (other.CompareTag("Laser"))
        {
            if (!_player.IsUnityNull())
                _player.AddScore(10);

            _enemySpeed = 0.0f;

            // Destroy enemy and play animation 
            _animator.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
            Destroy(other.gameObject);

            _isEnemyDead = true;
        }

        if (other.CompareTag("Player"))
        {
            // Script is one of the components of a game object
            Player player = other.GetComponent<Player>();

            if (!player.IsUnityNull())
                other.GetComponent<Player>().Damage();

            _enemySpeed = 0.0f;
            
            // Destroy enemy and play animation 
            _animator.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);

            _isEnemyDead = true;
        }
    }

    /// <summary>
    /// Randomly respawn if the object goes beyond the bottom of the screen
    /// </summary>
    private void RespawnBeyondScene()
    {
        if (transform.position.y <= -7.0f)
            transform.position = new Vector3(Random.Range(-10.5f, 10.5f), 7.0f, 0);
    }

    /// <summary>
    /// Compute enemy movement
    /// </summary>
    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_enemySpeed * Time.deltaTime));
    }
}