using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _powerupSpeed = 3.0f;
    
    // 0 = triple shot, 1 = speed up, 2 = shield
    [SerializeField] private int _powerupID;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        DestroyBeyondScene();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_powerupSpeed * Time.deltaTime));
    }

    void DestroyBeyondScene()
    {
        if (transform.position.y < -7.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();

            if (player == null)
            {
                Debug.Log("Player not found");
            }

            switch (_powerupID)
            {
                case 0:
                    player.TripleShotActivate();
                    break;
                case 1:
                    player.SpeedUpActivate();
                    break;
                case 2:
                    player.ShieldActivate();
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}