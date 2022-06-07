using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Serialized field is still private to other classes but visible to the Unity editor
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _trpileLaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngineVisualizer;
    [SerializeField] private GameObject _rightEngineVisualizer;
    [SerializeField] private AudioClip _lazerSoundClip;
    [SerializeField] private AudioClip _explosionSoundClip;

    [SerializeField] private float _playerSpeed = 3.5f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _powerupDuration = 5.0f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    private bool _tripleShotEnabled = false;
    private bool _shieldEnabled = false;

    private float _nextFire = 0.0f;
    private float _speedBoostMultipler = 1.5f;

    

    // Start is called before the first frame update
    void Start()
    {
        // Take the current position = new position(0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        
        _rightEngineVisualizer.SetActive(false);
        _leftEngineVisualizer.SetActive(false);

        if (_uiManager == null)
            Debug.Log("The UI manger is null");

        if (_spawnManager == null)
            Debug.Log("The spawn manger is null");
        
        if (_gameManager == null)
            Debug.Log("The game manger is null");

        if (_audioSource == null)
        {
            Debug.Log("The audio source is null");
        }
        else
        {
            _audioSource.clip = _lazerSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        ShootLaser();
    }

    /// <summary>
    /// Check cooldown and fire laser
    /// </summary>
    private void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            if (!_tripleShotEnabled)
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            else
            {
                Instantiate(_trpileLaserPrefab, transform.position, Quaternion.identity);
            }
            
            _audioSource.Play();
        }
    }

    /// <summary>
    /// Handle all the player movement
    /// </summary>
    private void CalculateMovement()
    {
        // Get horizontal and vertical inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        // Translate position first updates can prevent the player from shaking
        transform.Translate(Vector3.right * (horizontalInput * _playerSpeed * Time.deltaTime));
        transform.Translate(Vector3.up * (verticalInput * _playerSpeed * Time.deltaTime));


        // Reset player position when player goes beyond the bound
        if (transform.position.x >= 10.5f)
            transform.position = new Vector3(-10.5f, transform.position.y, 0);
        if (transform.position.x < -10.5f)
            transform.position = new Vector3(10.5f, transform.position.y, 0);
        if (transform.position.y >= 6.5f)
            transform.position = new Vector3(transform.position.x, -6.5f, 0);
        if (transform.position.y < -6.5f)
            transform.position = new Vector3(transform.position.x, 6.5f, 0);
    }

    /// <summary>
    /// Reduced player's hit-point and stop spawning enemy when player dead.
    /// </summary>
    public void Damage()
    {
        if (_shieldEnabled)
        {
            _shieldVisualizer.SetActive(false);
            _shieldEnabled = false;
            return;
        }
        
        _lives--;
        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            case 1:
                _leftEngineVisualizer.SetActive(true);
                _rightEngineVisualizer.SetActive(true);
                break;
            case 2:
                _leftEngineVisualizer.SetActive(true);
                _rightEngineVisualizer.SetActive(false);
                break;
            default:
                _leftEngineVisualizer.SetActive(false);
                _rightEngineVisualizer.SetActive(false);
                break;
        }

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();

            _audioSource.clip = _explosionSoundClip;
            _audioSource.Play();
            
            Destroy(this.gameObject);
            
            _gameManager.GameOver();
        }
    }

    public void TripleShotActivate()
    {
        _tripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDeactivate());
    }

    IEnumerator TripleShotPowerDeactivate()
    {
        yield return new WaitForSeconds(_powerupDuration);
        _tripleShotEnabled = false;
    }

    public void SpeedUpActivate()
    {
        _playerSpeed *= _speedBoostMultipler;
        StartCoroutine(SpeedUpDeactivate());
    }

    IEnumerator SpeedUpDeactivate()
    {
        yield return new WaitForSeconds(_powerupDuration);
        _playerSpeed /= _speedBoostMultipler;
    }

    public void ShieldActivate()
    {
        _shieldVisualizer.SetActive(true);
        _shieldEnabled = true;
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }
}