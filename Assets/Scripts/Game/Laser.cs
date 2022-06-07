using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _laserSpeed = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaserPosition();
        
        DestroyBeyondScene();
    }

    private void DestroyBeyondScene()
    {
        if (transform.position.y > 8.0f)
        {
            if (!transform.parent.IsUnityNull())
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void UpdateLaserPosition()
    {
        transform.Translate(Vector3.up * (_laserSpeed * Time.deltaTime));
    }
}
