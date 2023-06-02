using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Space : MonoBehaviour
{
    //Character character;
    //board board;
    public bool startSpace;
    public string[] neighbours;
    public bool visited = false;
    public GameObject linePrefab;
    public List<GameObject> neighbourList;

    // Start is called before the first frame update
    /*void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        board = GameObject.FindGameObjectWithTag("Fields").GetComponent<board>();
    }

    Update is called once per frame
    void Update()
    {

    }*/

    private void OnDrawGizmosSelected()
    {
        foreach (var neighbour in neighbourList)
        {
            if (neighbour != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }

    public void BuildLinesToNeighbours()
    {
        foreach (var neighbour in neighbourList)
        {
            if (neighbour != null)
            {
                BuildMeshLine(transform.position, neighbour.transform.position);
            }
        }
    }

    private void BuildMeshLine(Vector3 from, Vector3 to)
    {
        var LineVec = (to - from);
        var lineobj = Instantiate(linePrefab, from, Quaternion.LookRotation(LineVec), gameObject.transform);
        lineobj.transform.localScale = new Vector3(1, 1, LineVec.magnitude);
    }

    public void BuildNeighbour() {
        var instance = Instantiate(GetComponentInParent<board>().SpacePrefab,transform.position + new Vector3(5,0,0),Quaternion.identity,transform.parent);
        instance.name = "Plane " + transform.parent.childCount;
        GetComponentInParent<board>().MarkAsNeighbours(gameObject.GetComponent<Space>(), instance.GetComponent<Space>());
    }
}