using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class Pathfinding : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public PathingGrid _Grid;

    //This Function is now defunct
    //Function used to find path through the current level
    //public List<Vector2> FindPath (WorldPosition startPos, WorldPosition targetPos)
    //{
    //    if (!m_world.TileSpaces.ContainsKey(startPos) || m_world.TileSpaces[startPos].PNode == null)
    //    {
    //        Debug.LogError("Cannot find starting node position");
    //        return new List<Vector2>();
    //    }
    //    if (!m_world.TileSpaces.ContainsKey(targetPos) || m_world.TileSpaces[startPos].PNode == null)
    //    {
    //        Debug.LogError("Cannot find target node position");
    //        return new List<Vector2>();
    //    }

    //    PathingNode startNode = m_world.TileSpaces[startPos].PNode;
    //    PathingNode targetNode = m_world.TileSpaces[targetPos].PNode;

    //    List<PathingNode> openSet = new List<PathingNode>();
    //    HashSet<PathingNode> closedSet = new HashSet<PathingNode>();

    //    openSet.Add(startNode);

    //    //Main Pathifnding Loop
    //    while(openSet.Count > 0)
    //    {
    //        //Remember to convert to heap
    //        PathingNode currentNode = openSet[0];
    //        for(int i = 1; i < openSet.Count; i++)
    //        {
    //            if (openSet[i]._FCost < currentNode._FCost || openSet[i]._FCost == currentNode._FCost && openSet[i]._HCost < currentNode._HCost)
    //            {
    //                currentNode = openSet[i];
    //            }
    //        }

    //        openSet.Remove(currentNode);
    //        closedSet.Add(currentNode);

    //        if (currentNode == targetNode)
    //        {
    //            return RetracePath(startNode, targetNode);
    //        }

    //        //Checks currentNodes neighbours for suitable node to add to path
    //        foreach(PathingNode neighbour in _Grid.GetNeighbours(currentNode))
    //        {
    //            SpriteType angleTo = GetAngle(currentNode,neighbour);
    //            SpriteType angleFrom = GetAngle(neighbour,currentNode);

    //            //Debug tools to identify which walls are halting Path
    //            //if(_Grid.CheckWalls(neighbour, angleTo))
    //            //    _Grid.angles1.Add(neighbour);
    //            //if (_Grid.CheckWalls(currentNode, angleFrom))
    //            //    _Grid.angles2.Add(currentNode);

    //            //Checks to see if anything blocks the path, i.e. non-walkable tiles, already in closed list or has a blocking wall
    //            if(!neighbour._Walkable || closedSet.Contains(neighbour) || _Grid.CheckWalls(neighbour, angleTo) || _Grid.CheckWalls(currentNode, angleFrom))
    //                continue;

    //            //Checks the movement cost to neighbour
    //            int newMovementCostToNeighbour = currentNode._GCost + GetDistance(currentNode, neighbour);
    //            //If lower than neighbours G cost or not already in openset then neighbour becomes new currentNode
    //            if(newMovementCostToNeighbour < neighbour._GCost || !openSet.Contains(neighbour))
    //            {
    //                neighbour._GCost = newMovementCostToNeighbour;
    //                neighbour._HCost = GetDistance(neighbour,targetNode);
    //                neighbour._Parent = currentNode;
    //                //Add neighbour to openSet if not already
    //                if (!openSet.Contains(neighbour))
    //                    openSet.Add(neighbour);
    //            }
    //        }
    //    }
        
    //    return new List<Vector2>();
    //}

    //
    public List<Vector2> FindPath(PathingNode startNode, PathingNode targetNode)
    {
        if (startNode == null || targetNode == null) 
        {
            Debug.LogError("Target nodes are null");
            return new List<Vector2>();
        }

        List<PathingNode> openSet = new List<PathingNode>();
        HashSet<PathingNode> closedSet = new HashSet<PathingNode>();

        openSet.Add(startNode);

        //Main Pathifnding Loop
        while (openSet.Count > 0)
        {
            //Remember to convert to heap
            PathingNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i]._FCost < currentNode._FCost || openSet[i]._FCost == currentNode._FCost && openSet[i]._HCost < currentNode._HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            //Checks currentNodes neighbours for suitable node to add to path
            foreach (PathingNode neighbour in _Grid.GetNeighbours(currentNode))
            {
                SpriteType angleTo = GetAngle(currentNode, neighbour);
                SpriteType angleFrom = GetAngle(neighbour, currentNode);

                //Checks to see if anything blocks the path, i.e. non-walkable tiles, already in closed list or has a blocking wall
                if (!neighbour._Walkable || closedSet.Contains(neighbour) || _Grid.CheckWalls(neighbour, angleTo) || _Grid.CheckWalls(currentNode, angleFrom))
                    continue;

                //Checks the movement cost to neighbour
                int newMovementCostToNeighbour = currentNode._GCost + GetDistance(currentNode, neighbour);
                //If lower than neighbours G cost or not already in openset then neighbour becomes new currentNode
                if (newMovementCostToNeighbour < neighbour._GCost || !openSet.Contains(neighbour))
                {
                    neighbour._GCost = newMovementCostToNeighbour;
                    neighbour._HCost = GetDistance(neighbour, targetNode);
                    neighbour._Parent = currentNode;
                    //Add neighbour to openSet if not already
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Vector2>();
    }

    //Gets angle of direction from endNode to startNode, (1,0) to (0,0) would equal 7, (4,6) to (5,5) would equal 2
    SpriteType GetAngle(PathingNode startNode, PathingNode endNode)
    {
        Vector2 vector = endNode._NodePosition - startNode._NodePosition;
        //Normalizes vector for simplicitity
        Vector2 v2 = vector.normalized;
        //f1 is either -0.1f or 0.1f
        float f1 = Mathf.Round(Mathf.Abs(v2.y + 0.1f) / (v2.y + 0.1f)) * 0.1f;
        //Uses vector2 to get angle (180, 135, 90, 45, 0 , -45, -90, -135), f1 is added to the y vector in order to shorten the radius of 90 and -90 degrees otherwise angles 135, 45, -45 and -135 will not select correctly on the map
        float angle = Vector2.SignedAngle(new Vector2(-1.0f, 0.0f), new Vector2(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y + f1)));
        //Returns WorldPosition angle
        return (SpriteType)(Mathf.RoundToInt(angle / 45.0f) + 3);
        //0 - South, 1 - SouthEast, 2 - East, 3 - NorthEast, 4 - North, 5 - NorthWest, 6 - West, 7 - SouthWest
    }

    //Retraces path using parents of endNode until it reaches the startNode
    private List<Vector2> RetracePath(PathingNode startNode, PathingNode endNode)
    {
        //List<PathingNode> path = new List<PathingNode>();
        List<Vector2> worldPath = new List<Vector2>();
        PathingNode currentNode = endNode;

        while(currentNode != startNode)
        {
            //path.Add(currentNode);
            worldPath.Add(currentNode.TSpace.Position);
            currentNode = currentNode._Parent;
        }

        //path.Reverse();
        worldPath.Reverse();

        return worldPath;
    }

    //Gets the distance from nodeA to nodeB
    int GetDistance (PathingNode nodeA, PathingNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA._GridX - nodeB._GridX);
        int dstY = Mathf.Abs(nodeA._GridY - nodeB._GridY);
        
        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
