using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    private float _speedMultiplier = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private float _fireRate = 0.2f;
    private float _canFire = -1f;
    [SerializeField] private float _laserOffsetAmount = 0.95f;
    [SerializeField] private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;

    [SerializeField] private GameObject _shieldVisualiser;
    [SerializeField] private int _score;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private AudioClip _laserSound; 
    private AudioSource _audioSource;
    
    private UIManager _uiManager;
        
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI manager is NULL!");
        }
        
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL!");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        RestrainPlayerPosition();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            fireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
       
    }

    void RestrainPlayerPosition()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.9f, 0), 0);

        if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
    }

    void fireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffsetAmount, 0), Quaternion.identity);
        }
        _audioSource.Play();
        
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }
        
        _lives -= 1;
        _uiManager.UpdateLives(_lives);
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(PowerDownTripleShot());
    }

    IEnumerator PowerDownTripleShot()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(PowerDownSpeedBoost());
    }

    IEnumerator PowerDownSpeedBoost()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
