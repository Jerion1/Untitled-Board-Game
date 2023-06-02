using System.ComponentModel;
using UnityEngine;

public enum CharacterState { waiting, moving };

public class Character : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 targetPosition;
    public float smoothTime = 0.1f;
    public float rotationSpeed = 1.0f;


    private Animator animator;
    
    public CharacterState charState;
    
    // private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        //targetPosition = transform.position;
        charState = CharacterState.waiting;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame

    Vector3 velocity;
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed, Time.deltaTime);
            var angle = Vector3.SignedAngle(transform.forward, (targetPosition - transform.position), Vector3.up);
            transform.Rotate(Vector3.up, angle * rotationSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed, Time.deltaTime);
            charState = CharacterState.waiting;
        }
        else
        {
            transform.position = targetPosition;
            charState = CharacterState.waiting;
            animator.SetBool("move", false);
        }
    }

    public void MovePlayer(Vector3 goal)
    {
        //if (goal == targetPosition) { return; }
        targetPosition = goal;
        charState = CharacterState.moving;
        animator.SetBool("move", true);
        //transform.Translate(movement * speed * Time.deltaTime);
    }

    public void TeleportPlayer(Vector3 goal)
    {
        //goal.y += 0.95f;
        transform.position = goal;
        targetPosition = goal;
    }
}
