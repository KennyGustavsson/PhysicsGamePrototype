using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] public GameObject Explosion;
    [SerializeField]  float DestroyObjectTimer = 1;

    public Ragdoll RagdollRef;

    bool hasCollided;

    void Update()
    {
        if(hasCollided == true)
        {
            if (DestroyObjectTimer > 0)
        {
            DestroyObjectTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollided == false)
        {
            Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            RagdollRef.ToggleRagdoll(true);
        }

        hasCollided = true;
    }
}