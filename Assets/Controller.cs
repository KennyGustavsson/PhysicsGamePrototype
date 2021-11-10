using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //private Animator anim;
    [SerializeField] private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float inputSmoothing;

    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), ref smoothInputVelocity, inputSmoothing);

        Vector3 pos = transform.position;
        pos += new Vector3(currentInputVector.x, 0f, 0f) * moveSpeed * Time.deltaTime;

        transform.position = pos;
        //anim.SetFloat("Horizontal", currentInputVector.x);

        //if (currentInputVector.x == 0)
        //{
        //    anim.SetBool("Walk", false);
        //}
    }
}
