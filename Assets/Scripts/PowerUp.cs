using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    //triple shot id = 0 
    //speed id = 1
    //shield id =2 
    [SerializeField]
    private int powerUpID;

    [SerializeField]
    private AudioClip _explosionAudio;

    // Update is called once per frame

    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (transform.position.y < -6.8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                
                AudioSource.PlayClipAtPoint(_explosionAudio, transform.position);
                switch (powerUpID)
                {
                    case 0:
                        player.OnTripleShot();
                        break;
                    case 1:
                        player.OnSpeedBoost();
                        break;
                    case 2:
                        player.OnShieldActive();
                        break;
                    default:
                        Debug.Log("Default");
                        break;
                }
                    
            }
            Destroy(this.gameObject);
        }
    }
}
