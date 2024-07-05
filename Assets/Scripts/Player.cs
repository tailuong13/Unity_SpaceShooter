using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    //Bien toc do 
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMulti = 2f;
    [SerializeField]
    private GameObject _laserPrefabs;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioSource _laserSoundSource;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = 1f;
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private int _score = 50;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _tripleShotPrefabs;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private bool _isTripleShotEnable = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _rightEngineHurt;
    [SerializeField]
    private GameObject _leftEngineHurt;
    [SerializeField]
    private GameObject _explosionPrefabs;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _explosionSoundSource;

    // Start is called before the first frame update
    void Start()
    {
        //lay vi tri hien tai cua player = new position (0, 0, 0)
        transform.position = new Vector3 (0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserSoundSource = GetComponent<AudioSource>();

        if (_spawnManager == null )
        {
            Debug.LogError("Spawn Manager is Null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }

        if ( _laserSoundSource == null ) 
        {
            Debug.LogError("Audio Source is Null");
        }
        else
        {
            _laserSoundSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) 
        {
            InitLaser();
        }
    }

    void PlayerMovement()
    {
        //Input di chuyen chieu ngang cho player
        float horizontalInput = Input.GetAxis("Horizontal");

        //Input di chuyen chieu doc cho player
        float verticalInput = Input.GetAxis("Vertical");

        //new Vector3(1,0,0) * speed * real time
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        //neu vi tri hien tai y cua player >= o thi y = 0
        //neu vi tri hien tai y cua player <= -5.22 thi y = -5.22
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.22f, 0), 0);

        if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
        else if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
    }

    void InitLaser()
    {
        //neu nhan phim cach thi sinh ra gameObject
        _canFire = Time.time + _fireRate;
        if (_isTripleShotEnable == true)
        {
            Instantiate(_tripleShotPrefabs, transform.position + new Vector3(-0.3261116f, 0.2572114f, -6.357627f), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefabs, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);    
        }

        //phat am thanh laser
        _laserSoundSource.Play(0);
    }

    public void OnTripleShot()
    {
        _isTripleShotEnable = true;
        StartCoroutine(TripleShotDownCoroutine());
    }

    public void OnSpeedBoost()
    {
        _speed *= _speedMulti;
        StartCoroutine(SpeedBoostCoroutine());
    }

    public void OnShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int point)
    {
        _score += point;
        _uiManager.UpdateScore(_score);
    }

    public void Damege()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
           _shieldVisualizer.SetActive(false);
            return;
        }

        _playerLives--;

        if (_playerLives == 2)
        {
            _rightEngineHurt.SetActive(true);    
        }
        else if (_playerLives == 1)
        {
            _leftEngineHurt.SetActive(true);
        }
 
        _uiManager.UpdateLiveSprites(_playerLives);
        if (_playerLives < 1) 
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefabs, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


    IEnumerator TripleShotDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotEnable = false;
    }

    IEnumerator SpeedBoostCoroutine()
    {
        yield return new WaitForSeconds(5.0f);

        _speed /= _speedMulti;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
        {
            Damege();
            Destroy(other.gameObject);
        }
    }
}
