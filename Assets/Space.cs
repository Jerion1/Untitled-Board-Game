using UnityEngine;

public class Space : MonoBehaviour
{
    Character character;
    board board;
    public bool startSpace;
    public string[] neighbours;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        board = GameObject.FindGameObjectWithTag("Fields").GetComponent<board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Clicked();
        }
    }


    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider.gameObject.transform.position);
            var target = hit.collider.gameObject.name;

            var pos = hit.collider.gameObject.transform.position;
            //Debug.Log(pos);
            //character.MovePlayer(pos);
            board.MoveToSpace(transform.name);
        }
    }
}
