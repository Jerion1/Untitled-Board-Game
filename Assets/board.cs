using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class board : MonoBehaviour
{
    Character character;
    private int currentSpace;
    private int targetSpace;
    private List<int> route;

    public int maxIterations = 10;
    private int iterations = 0;



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

    }

    public void MoveToSpace(string childString)
    {
        print("--------------");
        ResetVisited();
        targetSpace = GetChildID(childString);
        List<int> ReturnList = new List<int>();
        ReturnList = GetBestNeighbour(currentSpace);
        if (ReturnList[0] < 0)
        {
            print("Target Too Far Away: " + ReturnList[0]);
        }
        else
        {
            ReturnList.Remove(0);
            route = Enumerable.Reverse(ReturnList).ToList();
            print("Route found: " + route[0]);
        }


    }

    private List<int> GetBestNeighbour(int from)
    {
        int BestDepth = -1;
        List<int> BestDepthList = new List<int>();
        if (!transform.GetChild(from).GetComponent<Space>().visited)
        {
            transform.GetChild(from).GetComponent<Space>().visited = true;

            string[] neighbourNames = transform.GetChild(from).GetComponent<Space>().neighbours;

            for (int i = 0; i < neighbourNames.Length; i++)
            {
                int id = GetChildID(neighbourNames[i]);
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
                    List<int> temp = GetBestNeighbour(id);
                    int tempDepth = temp[0];
                    if (tempDepth != -2)
                    {
                        if (BestDepth == -1)
                        {
                            BestDepth = tempDepth;
                            BestDepthList = temp;
                        }
                        else if (BestDepth > tempDepth)
                        {
                            BestDepth = tempDepth;
                            BestDepthList = temp;
                        }
                    }
                    //GetBestNeighbour(id);
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
            //BestDepthList.Add(id);

        }
        else
        {
            BestDepthList[0] += 1;
            BestDepthList.Add(from);
        }

        return BestDepthList;
    }
    /*
    private List<int> GetNeighbours(int from)
    {

        int BestDepth = -1;
        List<int> BestDepthList = new List<int>();

        string[] neighbourNames = transform.GetChild(from).GetComponent<Space>().neighbours;

        iterations++;
        if (iterations >= maxIterations)
        {
            BestDepthList.Clear();
            BestDepthList.Add(-2);
            //route.Add(targetSpace);
            return BestDepthList;
        }

        //foreach (string name in neighbourNames)
        for (int i = 0; i < 2; i++)
        {
            int id = GetChildID(neighbourNames[i]);
            //print("id: " + id + ", target: " + targetSpace);
            if (id == targetSpace)
            {
                //print("hit: ");
                BestDepthList.Clear();
                BestDepthList.Add(1);
                BestDepthList.Add(id);
                print("BestDepth: "+BestDepthList[0]);
                //route.Add(targetSpace);
                return BestDepthList;
            }
            else
            {
                //route.Add(id);

                if (BestDepth < 0)
                {
                    BestDepthList = GetNeighbours(id);
                    BestDepth = BestDepthList[0];
                }
                else
                {
                    List<int> temp = GetNeighbours(id);
                    if (temp[0] != -2)
                    {

                        if (temp[0] < BestDepth)
                        {
                            BestDepthList = temp;
                        }
                    }

                }
            }
        }
        if (BestDepthList[0] < 0)
        {
            iterations--;
        }
        else
        {
            BestDepthList[0] += 1;
            BestDepthList.Add(from);
            print(BestDepthList[0]);
        }
        return BestDepthList;
    }*/

    private int GetChildID(string spaceName)
    {
        int ID = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            //print("searchedForName: " + spaceName + ", name: " + transform.GetChild(i).name);
            if (spaceName == transform.GetChild(i).name)
            {
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
    }

    private void ResetVisited()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Space>().visited = false;
        }
    }
}
