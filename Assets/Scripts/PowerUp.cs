using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    
    //powerup types 0 for triple shot, 1 for speed, 2 for shields
    [SerializeField] private int _powerUpId;
    
    [SerializeField] private AudioClip _powerUpSound; 


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);
            if (player != null)
            {
                switch (_powerUpId)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    default:
                        Debug.Log("Default Value.");
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
