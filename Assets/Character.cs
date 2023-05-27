using UnityEngine;

public enum CharacterState { waiting, moving };

public class Character : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition;
    public float smoothTime = 0.5f;
    public bool moving = false;
    
    public CharacterState charState;
    
    // private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        //targetPosition = transform.position;
        charState = CharacterState.waiting;
    }

    // Update is called once per frame

    Vector3 velocity;
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.02f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);
        }
        else
        {
            transform.position = targetPosition;
            if (charState == CharacterState.moving)
            {
                charState = CharacterState.waiting;
            }
        }
    }

    public void MovePlayer(Vector3 goal)
    {
        //Debug.Log(x);
        goal.y += 0.95f;
        targetPosition = goal;
        charState = CharacterState.moving;
        //transform.Translate(movement * speed * Time.deltaTime);
    }

    public void TeleportPlayer(Vector3 goal)
    {
        goal.y += 0.95f;
        transform.position = goal;
        targetPosition = goal;
    }
}
