using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Space : MonoBehaviour
{
    //Character character;
    //board board;
    public bool startSpace;
    //public string[] neighbours;
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
        var offset = Vector3.one * 0.5f;
        for ( int i = 0; i < neighbourList.Count; i++)
        {
            if (neighbourList[i] != null)
            {
                Gizmos.color = Color.HSVToRGB(Mathf.Repeat((GetInstanceID())*0.8f+i*0.2f,1),1,1);
                Gizmos.DrawLine(transform.position + offset, neighbourList[i].transform.position - offset+Vector3.up);
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
        var instance = Instantiate(GetComponentInParent<Board>().SpacePrefab,transform.position + new Vector3(5,0,0),Quaternion.identity,transform.parent);
        instance.name = "Plane " + transform.parent.childCount;
        GetComponentInParent<Board>().MarkAsNeighbours(gameObject.GetComponent<Space>(), instance.GetComponent<Space>());
    }
}