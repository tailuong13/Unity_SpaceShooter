using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _laserSpeed = 8.0f;

    private bool _isEnemyLaser = false;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        //Dich chuyen laser len tren
        Vector3 laserPos = Vector3.up;
        transform.Translate(laserPos * _laserSpeed * Time.deltaTime);

        //neu vi tri y cua laser lon hon 7 thi destroy no
        if (transform.position.y > 6.73f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);

        }
    }

    private void MoveDown()
    {
        Vector3 laserPos = Vector3.down;
        transform.Translate(laserPos * _laserSpeed * Time.deltaTime);

        //neu vi tri y cua laser lon hon 7 thi destroy no
        if (transform.position.y < -6.73f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void OnEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}

