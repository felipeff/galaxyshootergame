using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;


    // 0 = triple shot
    // 1 = speed
    // 2 = shields
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerupClip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Invalid Powerup ID");
                        break;
                }
                
            }

            AudioSource.PlayClipAtPoint(_powerupClip, transform.position) ;

            Destroy(this.gameObject);
        }
    }
}
