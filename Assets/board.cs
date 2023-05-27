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
    private int stepsLeft = 0;



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
        if(stepsLeft > 0)
        {
            if (!character.gameObject.GetComponent<Character>().moving)
            {
                Vector3 pos = transform.GetChild(route[0]).position;
                currentSpace = route[0];
                route.RemoveAt(0);
                stepsLeft--;
                character.MovePlayer(pos); 
            }
        }
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
        if (!transform.GetChild(from).GetComponent<Space>().visited)
        {


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
    }

    private void ResetVisited()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Space>().visited = false;
        }
    }
}
