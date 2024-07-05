using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _enemySpeed = 3f;

    private Player _player;

    private Animator _animator;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefabs;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if ( _player == null )
        {
            Debug.Log("Player is NULL");
        }

        if ( _animator == null )
        {
            Debug.Log("Animator is NULL");
        }

        if (_audioSource == null )
        {
            Debug.Log("Audio Source is NULL");
        }

    }
    // Update is called once per frame  
    void Update()
    {
        EnemyMovement();

        FireLaser();
    }

    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefabs, transform.position + new Vector3(0, -1.013f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i< lasers.Length; i++ ) {
                lasers[i].OnEnemyLaser();
            }
        }
    }

    private void EnemyMovement()
    {
        transform.Translate(new Vector3(0, -1, 0) * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -6.38f)
        {
            float randomX = Random.Range(-9.4f, 9.4f);
            transform.position = new Vector3(randomX, 6.38f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //neu other la nguoi choi, dame player va destroy enemy
        if (other.tag == "Enemy_Laser")
        {
            return;
        }
        
        
        if (other.tag == "Player") 
        {
            Player player = other.transform.GetComponent<Player>();
            //Gay sat thuong len player
            if (player != null)
            {
                player.Damege();
            }
            _animator.SetTrigger("EnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play(0);
            Destroy(this.gameObject, 2.6f); 
        }
        //ney other la laser, destroy laser va destroy enemy
        
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("EnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play(0);

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }
    }
}
