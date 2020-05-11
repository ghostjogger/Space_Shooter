using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _rotationSpeed = 20.0f;
    private Player _player;
    private Animator _anim;
    private CircleCollider2D _collider;
    [SerializeField] private AudioClip _explosionSound; 
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
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

        _collider = transform.GetComponent<CircleCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("The collider is NULL.");
        }   
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the asteroid is NULL!");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        if (transform.position.y < -6.5)
        {
            Destroy(this.gameObject);
        }
        transform.Rotate(Vector3.forward,_rotationSpeed * Time.deltaTime);
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
            _anim.SetTrigger("Asteroid_Destroyed");
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
            _anim.SetTrigger("Asteroid_Destroyed");
            _collider.enabled = false;
            _speed = 0;
            Destroy(this.gameObject, 2.37f);
        }
    }
}
