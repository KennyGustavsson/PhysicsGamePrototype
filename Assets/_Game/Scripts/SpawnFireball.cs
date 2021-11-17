using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireball : MonoBehaviour
{
    public GameObject Fireball;
    public float SpawnInterval = 2.0f;
    float SpawnTimer;
    public float MoveDirectionAxisX;
    void Start()
    {
        SpawnTimer = SpawnInterval;
    }
    void Update()
    {
        if (SpawnTimer > 0)
        {
            SpawnTimer -= Time.deltaTime;
        }
        else if (SpawnTimer <= 0)
        {
            Instantiate(Fireball, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            SpawnTimer = SpawnInterval; 
        }
    }
}