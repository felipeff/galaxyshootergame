using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;
    private Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = gameObject.GetComponent<Animator>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {
            transform.position = new Vector3(Random.Range(-9.4f, 9.4f), 6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            // Damage player    
            if (_player != null)
            {
                _player.Damage();
            }

        }else if(other.CompareTag("Laser"))
        {
            if(_player != null)
            {
                _player.AddScore(10);
            }
          
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player") || other.CompareTag("Laser"))
        {
            // To fix a bug where you destroy the enemy but then hit in the explosion animation
            BoxCollider2D _collider = gameObject.GetComponent<BoxCollider2D>();
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            _anim.SetTrigger("OnEnemyDeath");
            StartCoroutine(CheckAnimationPosition());
        }

    }

    IEnumerator CheckAnimationPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (transform.position.y < -5.5f)
            {
                // out of screen, stop the animation
                Destroy(gameObject);
            }
        }
    }
}
