using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyCalculator : MonoBehaviour
{
    //[Inject(InjectFrom.Anywhere)]
    //public World m_world;
    
    ////Values used for detection of potentially blocked paths
    //public float _DetectRadius;//How sensitive the detection is
    //public float _RadiusOffset;//How big the area detected is

    ////Vectors used for calc
    //protected Vector2 vector;//vector of origin to target
    //protected Vector2 direction;//normalized vector
    //protected Vector2 leftOfTarget;//Vector of origin to left of target
    //protected Vector2 rightOfTarget;//Vector of origin to right of target
    //protected Vector2 leftNormal;//Direction of vector at 90 degrees to leftOfTarget
    //protected Vector2 rightNormal;//Direction of vector at 90 degrees to rightOfTarget

    ////Range of view that is blocked
    //protected float leftMin = 0f;
    //protected float leftMax = 0f;
    //protected float rightMin = 1f;
    //protected float rightMax = 1f;

    ////Use to get accuracy reduction of attack
    //public float Accuracy
    //{
    //    get
    //    {
    //        return (rightMin < leftMax) ? rightMax - leftMin : (leftMax - leftMin) + (rightMax - rightMin);
    //    }
    //}

    //protected void SetValues(Vector2 origin, Vector2 target)
    //{
    //    //Setup Variables
    //    vector = target - origin;
    //    direction = vector.normalized;
    //    leftOfTarget = target + new Vector2(-direction.y, direction.x) * 0.5f;
    //    rightOfTarget = target + new Vector2(direction.y, -direction.x) * 0.5f;

    //    //Reset List used for Gizmos to show field of view
    //    //targets = new List<Vector2>
    //    //{
    //    //    origin,
    //    //    target,
    //    //    leftOfTarget,
    //    //    rightOfTarget
    //    //};

    //    //Get nodes in vision
    //    leftNormal = (leftOfTarget - origin).normalized;
    //    leftNormal = origin + new Vector2(leftNormal.y, -leftNormal.x);

    //    rightNormal = (rightOfTarget - origin).normalized;
    //    rightNormal = origin + new Vector2(-rightNormal.y, rightNormal.x);

    //    //Reset Gizmos Lists for locations that block field of view
    //    //rightBlocks = new List<Vector2>();
    //    //leftBlocks = new List<Vector2>();
    //    //walls = new List<Vector4>();

    //    //Reset MinMax values
    //    leftMin = 0f;
    //    leftMax = 0f;
    //    rightMin = 1f;
    //    rightMax = 1f;
    //}

    //public float GetVision(WorldPosition origin, WorldPosition target)
    //{
    //    //Get Node position to make calculations easier
    //    Vector2 originV = m_world.TileSpaces[origin].PNode._NodePosition;
    //    Vector2 targetV = m_world.TileSpaces[target].PNode._NodePosition;

    //    //Setup Vectors
    //    SetValues(originV, targetV);

    //    //Get the +/- of value
    //    float xn = (vector.x == 0f) ? 1f : Mathf.Abs(vector.x) / vector.x;
    //    float yn = (vector.y == 0f) ? 1f : Mathf.Abs(vector.y) / vector.y;

    //    //% values for debug
    //    //float left = 0f;
    //    //float right = 0f;

    //    //Loop through all nodes in path to target position
    //    for (int y = 0; y <= Mathf.Abs(Mathf.RoundToInt(vector.y)); y++)
    //    {
    //        for (int x = 0; x <= Mathf.Abs(Mathf.RoundToInt(vector.x)); x++)
    //        {
    //            //Get Coords of target location
    //            Vector2 temp = new Vector2(x * xn, y * yn) + originV;
    //            WorldPosition wp = m_world.PGrid.Grid[Mathf.FloorToInt(temp.x), Mathf.FloorToInt(temp.y)].TSpace.Position;

    //            //Check Location exists
    //            if (!m_world.TileSpaces.ContainsKey(wp)) continue;
    //            if (!m_world.TileSpaces[wp].IsBlockedByObject() && m_world.TileSpaces[wp].Walls.Count == 0) continue;

    //            //Calculate Dot products of both Left and Right view line
    //            float dotLeft = Vector2.Dot(leftNormal - originV, new Vector2(x * xn, y * yn));
    //            float dotRight = Vector2.Dot(rightNormal - originV, new Vector2(x * xn, y * yn));

    //            //If both values are 0 continue to stop divide by zero error
    //            if (dotLeft == 0 && dotRight == 0) continue;

    //            //Anything of more than 0.71f away is definately not in range
    //            if (dotLeft <= -0.71f || dotRight <= -0.71f) continue;

    //            //Calculate leftoftarget equation
    //            Vector2 l = leftOfTarget - originV;
    //            Vector2 c = targetV - originV;
    //            Vector2 r = rightOfTarget - originV;
    //            Debug.Log("Left y = (" + l.x + " / " + l.y + " ) * x");
    //            Debug.Log("Center y = (" + c.x + " / " + c.y + " ) * x");
    //            Debug.Log("Right y = (" + r.x + " / " + r.y + " ) * x");

    //            //float targetX = 0f;
    //            //float targetY = 0f;

    //            TileSpace ts = m_world.TileSpaces[wp];

    //            if (ts.Walls.Count > 0)
    //            {
    //                Vector2 closestPoint = new Vector2(0f, 0f);
    //                Vector2 furthestPoint = new Vector2(0f, 0f);
    //                Vector2 leftMostPoint = new Vector2(0f, 0f);
    //                Vector2 rightMostPoint = new Vector2(0f, 0f);

    //                List<Vector2> wallPoints = ts.GetNPointsOfWalls();

    //                foreach (Vector2 v in wallPoints)
    //                {
    //                    Vector2 rv = v - originV; //+ new Vector2(x * xn, y * yn);
    //                    //Find the closest point
    //                    if (closestPoint == new Vector2(0f, 0f))
    //                        closestPoint = rv;
    //                    else if (Vector2.Distance(originV, rv) < Vector2.Distance(originV, closestPoint))
    //                        closestPoint = rv;

    //                    //Get furthest point
    //                    if (Vector2.Distance(originV, rv) > Vector2.Distance(originV, furthestPoint))
    //                        furthestPoint = rv;

    //                    float a = Vector2.Dot(leftOfTarget - targetV, rv);
    //                    float b = Vector2.Dot(leftOfTarget - targetV, leftMostPoint);
    //                    float d = Vector2.Dot(leftOfTarget - targetV, rightMostPoint);

    //                    //Find the left most point
    //                    if (a > b)
    //                        leftMostPoint = rv;
    //                    //Find the right most point
    //                    if (a < d)
    //                        rightMostPoint = rv;
    //                }
    //            }
    //        }
    //    }
    //    return 1f - Accuracy;
    //}

    //protected float CheckTarget(Vector2 origin, Vector2 target, int x, int y)
    //{
    //    return 0f;
    //}

    //protected void CheckForWalls(Vector2 target)
    //{

    //}

    //protected List<Vector2> targets = new List<Vector2>();
    //protected List<Vector2> leftBlocks = new List<Vector2>();
    //protected List<Vector2> rightBlocks = new List<Vector2>();
    //protected List<Vector4> walls = new List<Vector4>();
    //protected List<Vector2> points = new List<Vector2>();
    //private void OnDrawGizmos()
    //{
    //    if (rightBlocks.Count > 0)
    //    {
    //        foreach (Vector2 v in rightBlocks)
    //        {
    //            Gizmos.color = Color.blue;
    //            Gizmos.DrawSphere(v, _RadiusOffset);
    //        }
    //    }
    //    if (leftBlocks.Count > 0)
    //    {
    //        foreach (Vector2 v in leftBlocks)
    //        {
    //            Gizmos.color = Color.cyan;
    //            Gizmos.DrawSphere(v, _RadiusOffset);
    //        }
    //    }
    //    if (targets.Count > 1)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawLine(targets[0], targets[1]);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(targets[0], targets[2]);//Left
    //        Gizmos.DrawLine(targets[0], targets[3]);//Right
    //        Gizmos.DrawLine(targets[2], targets[3]);
    //    }
    //    if(walls.Count > 0)
    //    {
    //        Gizmos.color = Color.yellow;
    //        foreach(Vector4 v in walls)
    //        {
    //            Vector2 v1 = m_world.GetComponent<PathingGrid>().ConvertToNodePosition(new WorldPosition(v.x,v.y,0,0));
    //            Vector2 v2 = m_world.GetComponent<PathingGrid>().ConvertToNodePosition(new WorldPosition(v.z, v.w, 0, 0));
    //            Gizmos.DrawLine(v1, v2);
    //        }           
    //    }
    //    if(points.Count > 0)
    //    {
    //        Gizmos.color = Color.red;
    //        foreach(Vector2 v in points)
    //        {
    //            Gizmos.DrawSphere(v, 0.1f);
    //        }
    //    }
    //}
}