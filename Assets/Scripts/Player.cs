using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject[] _engineDamage;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;

    [SerializeField]
    private int _lives = 3;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;

    private Animator _anim;

    

    [SerializeField]
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _anim = GetComponent<Animator>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }


        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator on Player is NULL");
        }

        // Set the initial player position
        transform.position = new Vector3(0f, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }


    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (!_isTripleShotActive) 
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.05f, 0f), Quaternion.identity);
        }
        else 
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);    
        }

        _audioSource.clip = _laserSoundClip;
        _audioSource.Play();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        if(horizontalInput < 0)
        {
            // moving left
            _anim.SetBool("TurnLeft", true);
            _anim.SetBool("TurnRight", false);

        }else if(horizontalInput > 0)
        {
            // moving right
            _anim.SetBool("TurnLeft", false);
            _anim.SetBool("TurnRight", true);
        }
        else
        {
            _anim.SetBool("TurnLeft", false);
            _anim.SetBool("TurnRight", false);
        }

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
         
        // Mathf.Clamp ensures that the value stays between the bounds specified.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }


    public void Damage()
    {
        // if shield is active then we get a free hit
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if(_lives == 2)
        {
            _engineDamage[0].SetActive(true);
        }else if(_lives == 1)
        {
            _engineDamage[1].SetActive(true);
        }else if(_lives <= 0)
        {
            _uiManager.CheckForBestScore(_score);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        // start the power down coroutine to triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
    }

}
