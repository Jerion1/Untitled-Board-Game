using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition;
    public float smoothTime = 0.5f;
    private bool moving = false;
    // private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        //targetPosition = transform.position;
    }

    // Update is called once per frame

    //public float speed = 10;
    Vector3 velocity;
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
        }
        else
        {
            transform.position = targetPosition;
            if (moving)
            {
                moving = false;
            }
        }
    }


    public void MovePlayer(Vector3 goal)
    {
        //Debug.Log(x);
        goal.y += 0.95f;
        targetPosition = goal;
        moving = true;
        //transform.Translate(movement * speed * Time.deltaTime);
    }

    public void TeleportPlayer(Vector3 goal)
    {
        goal.y += 0.95f;
        transform.position = goal;
        targetPosition = goal;
    }
}
