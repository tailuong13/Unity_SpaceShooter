using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astetoid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _asteroidSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefabs;

    private SpawnManager _spawnManager;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, _asteroidSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefabs, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.25f);
        }
    }
}
