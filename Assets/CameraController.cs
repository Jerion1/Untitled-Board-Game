using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed = 0.01f;

    public float ScrollSpeed = 1f;
    private float ScrollMomentum = 0f;
    public float ScrollSlowdown = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //print("space key was pressed");
        }

        float xAxisValue = Input.GetAxis("Horizontal") * Speed;
        float zAxisValue = Input.GetAxis("Vertical") * Speed;

        transform.position = new Vector3(transform.position.x + xAxisValue, transform.position.y, transform.position.z + zAxisValue);

        Vector3 pos = Camera.main.transform.position;
        float scrollInput = Input.mouseScrollDelta.y;
        //print(scrollInput);
        if (scrollInput != 0)
        {
            ScrollMomentum = scrollInput * ScrollSpeed;
        }
        else
        {
            if (ScrollMomentum > 0.1f)
            {
                ScrollMomentum -= ScrollSlowdown;
            }
            else if (ScrollMomentum < -0.1f)
            {
                ScrollMomentum += ScrollSlowdown;
            }
            else
            {
                ScrollMomentum = 0;
            }
        }

        //print(ScrollMomentum);
        pos.y -= ScrollMomentum;
        pos.z += ScrollMomentum;
        Camera.main.transform.position = pos;

    }
}
