using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition;
    public float smoothTime = 0.5f;
    // private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    
    //public float speed = 10;
    Vector3 velocity;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
    }
   

    public void MovePlayer(float x, float y, float z)
    {
        Debug.Log(x);
        targetPosition = new Vector3(x, 0.95f + y, z);
       
        //transform.Translate(movement * speed * Time.deltaTime);
    }
}
