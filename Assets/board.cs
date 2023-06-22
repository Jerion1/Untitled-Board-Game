using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    Character character;
    private int currentSpace;
    private int targetSpace;
    private List<int> route;

    public int maxIterations = 10;
    private int stepsLeft = 0;

    public GameObject SpacePrefab;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var childAccess = transform.GetChild(i);
            if (childAccess.GetComponent<Space>().startSpace)
            {
                character.TeleportPlayer(childAccess.position);
                currentSpace = i;
                //print(currentSpace);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (stepsLeft > 0)
        {
            if (CharacterState.waiting == character.charState)
            {
                Vector3 pos = transform.GetChild(route[0]).position;
                currentSpace = route[0];
                route.RemoveAt(0);
                stepsLeft--;
                character.MovePlayer(pos);
            }
        }

        if (CharacterState.waiting == character.charState)
        {
            //Erstma so, aber OnMouseDown is vllt gut wegen UI maybe dunno... oder halt mit state
            if (Input.GetMouseButtonDown(0))
            {
                Clicked();
            }
        }
    }

    void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var target = hit.collider.GetComponent<Space>();
            if (!target) { return; }
            MoveToSpace(target);
        }
    }

    public void MoveToSpace(Space target)
    {
        print("--------------");
        ResetVisited();
        targetSpace = GetChildID(target.transform);
        List<int> ReturnList;
        ReturnList = GetBestNeighbour(currentSpace);
        if (ReturnList[0] < 0)
        {
            print("Target Too Far Away: " + ReturnList[0]);
        }
        else
        {
            stepsLeft = ReturnList[0];
            print("Route found length: " + stepsLeft);
            ReturnList.RemoveAt(0);
            route = Enumerable.Reverse(ReturnList).ToList();

            print("Route found for: " + string.Join(", ", route));
        }
    }

    private List<int> GetBestNeighbour(int from)
    {
        int BestDepth = -1;
        List<int> BestDepthList = new List<int>();
        if (transform.GetChild(from).GetComponent<Space>().visited == false)
        {
//            string[] neighbourNames = transform.GetChild(from).GetComponent<Space>().neighbours;
            GameObject[] neighbours = transform.GetChild(from).GetComponent<Space>().neighbourList.ToArray();
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] == null) { continue; }
                int id = GetChildID(neighbours[i].transform);
                print("id: " + id + ", target: " + targetSpace);

                if (id == targetSpace)
                {
                    BestDepthList.Clear();
                    BestDepthList.Add(1);
                    BestDepthList.Add(id);
                    print("Neighbour Hit: " + id);
                    return BestDepthList;
                }
                else
                {
                    print("find Neighbour Neighbour: " + id);
                    transform.GetChild(from).GetComponent<Space>().visited = true;
                    List<int> temp = GetBestNeighbour(id);
                    transform.GetChild(from).GetComponent<Space>().visited = false;
                    int tempDepth = temp[0];
                    if (tempDepth != -2)
                    {
                        print("tempDepth: " + tempDepth + ", BestDepth: " + BestDepth);
                        if (BestDepth == -1)
                        {
                            BestDepth = tempDepth;
                            BestDepthList = temp;
                            BestDepthList.Add(id);
                        }
                        else if (BestDepth > tempDepth)
                        {
                            BestDepth = tempDepth;
                            BestDepthList = temp;
                            BestDepthList.Add(id);
                        }

                    }
                }
            }
        }
        else
        {
            print("already Visited: " + from);
        }

        if (BestDepth == -1)
        {
            BestDepthList.Clear();
            BestDepthList.Add(-2);
            //BestDepthList.Add(from);

        }
        else
        {
            print("got Route: " + string.Join(", ", BestDepthList));
            BestDepthList[0] += 1;

        }
        print("go back");
        print("new Route: " + string.Join(", ", BestDepthList));
        //BestDepthList.Add(from);
        print("newer Route: " + string.Join(", ", BestDepthList));
        return BestDepthList;
    }

    /*
    private int GetChildID(string spaceName)
    {
        int ID = -1;
        for (int i = 0; i < transform.childCount; i++)
        {

            if (spaceName == transform.GetChild(i).name)
            {
                print("searchedForName: " + spaceName + ", id: " + i);
                //print("ID Hit: " + i);
                ID = i;
                return ID;
            }

        }
        if (ID == -1)
        {
            throw new System.Exception("child does not exist");
        }

        return ID;
    }*/

    private int GetChildID(Transform t)
    {
        int ID = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (t == transform.GetChild(i))
            {
                ID = i;
                return ID;
            }
        }
        if (ID == -1)
        {
            throw new System.Exception("gameobject not child of this board");
        }
        return ID;
    }

    private void ResetVisited()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Space>().visited = false;
        }
    }

    public void MarkAsNeighbours(Space a, Space b)
    {
        if (!a.neighbourList.Contains(b.gameObject)) { a.neighbourList.Add(b.gameObject); }
        if (!b.neighbourList.Contains(a.gameObject)) { b.neighbourList.Add(a.gameObject); }
    }
}