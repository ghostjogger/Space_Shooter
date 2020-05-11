using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private Player _player;
    private Animator _anim;
    private BoxCollider2D _collider;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private AudioClip _explosionSound; 
    private AudioSource _audioSource;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is NULL.");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The animator is NULL.");
        }

        _collider = transform.GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The collider is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the enemy is NULL!");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }

    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            fireLaser();
        }
    }

    private void calculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            _speed = 0;
            Destroy(this.gameObject, 2.37f);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(3,11));
            }
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            _collider.enabled = false;
            _speed = 0;
            Destroy(this.gameObject, 2.37f);
        }
    }

    private void fireLaser()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

}
