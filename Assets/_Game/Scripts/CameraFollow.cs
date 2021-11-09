using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowPos;

    // Update is called once per frame
    void Update()
    {
        Vector3 CameraPos = transform.position;
        Vector3 NewPos = new Vector3(CameraPos.x, FollowPos.position.y, CameraPos.z);
        transform.position = NewPos;
    }
}
