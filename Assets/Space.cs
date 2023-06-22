using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Space : MonoBehaviour
{
    public bool startSpace;
    public bool visited = false;
    public GameObject linePrefab;
    public List<GameObject> neighbourList;



    private void OnDrawGizmosSelected()
    {
        //var offset = Vector3.one * 0.5f;
        for ( int i = 0; i < neighbourList.Count; i++)
        {
            if (neighbourList[i] != null)
            {
                Gizmos.color = Color.HSVToRGB(Mathf.Repeat((GetInstanceID())*0.8f+i*0.2f,1),1,1);
                Gizmos.DrawLine(transform.position, Vector3.Lerp(transform.position,neighbourList[i].transform.position,0.5f));
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
        var instance = PrefabUtility.InstantiatePrefab(GetComponentInParent<Board>().SpacePrefab, transform.parent);
        instance.GetComponent<Transform>().position = transform.position + new Vector3(5,0,0);
        instance.name = "Plane " + transform.parent.childCount;
        GetComponentInParent<Board>().MarkAsNeighbours(gameObject.GetComponent<Space>(), instance.GetComponent<Space>());
    }
}