using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _asteroidMoveSpeed;
    [SerializeField] private float _asteroidRotateSpeed;
    [SerializeField] private int _numberOfEnemiesToSpawn;

    [SerializeField] private GameObject _explosionPrefabs;

    private Player _player;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_player == null)
        {
            Debug.Log("Player not found");
        }

        if (_spawnManager == null)
        {
            Debug.Log("Spawn manager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -8.0f)
            Destroy(this.gameObject);
        
        transform.position += (Vector3.down * (_asteroidMoveSpeed * Time.deltaTime));
        
        transform.Rotate(new Vector3(0,0,1) * (_asteroidRotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefabs, transform.position, Quaternion.identity);
            
            _spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 0.1f);
        }

        if (other.CompareTag("Player"))
        {
            _player.Damage();
            Instantiate(_explosionPrefabs, transform.position, Quaternion.identity);
            
            _spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 0.1f);
        }
    }
}
