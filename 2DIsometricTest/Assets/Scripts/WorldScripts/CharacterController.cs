using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This controls the characters various forms of movement, Moving to another tile, Rotating etc
public class CharacterController : MonoBehaviour
{
    private Character m_charRef = null;

    #region Movement Vars
    
    [SerializeField] protected float m_moveSpeed = 5f;
    public float MoveSpeed { get { return m_moveSpeed; } }
    
    #endregion

    public void Initialize(Character cr)
    {
        if(cr == null) { Debug.LogError("Passed a null character"); return; }
        m_charRef = cr;
    }

    protected int GetDirection(Vector2 target)//Gets the WorldPosition Angle of current position and target
    {
        Vector2 vector = target - m_charRef.WorldPosition;
        //Normalizes vector for simplicitity
        Vector2 v2 = vector.normalized;
        //f1 is either -0.1f or 0.1f
        float f1 = Mathf.Round(Mathf.Abs(v2.y + 0.1f) / (v2.y + 0.1f)) * 0.1f;
        //Uses vector2 to get angle (180, 135, 90, 45, 0 , -45, -90, -135), f1 is added to the y vector in order to shorten the radius of 90 and -90 degrees otherwise angles 135, 45, -45 and -135 will not select correctly on the map
        float angle = Vector2.SignedAngle(new Vector2(1.0f, 1.0f), new Vector2(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y + f1)));
        //Returns WorldPosition angle
        return Mathf.RoundToInt(angle / 45.0f) + 3;
        //0 - South, 1 - SouthEast, 2 - East, 3 - NorthEast, 4 - North, 5 - NorthWest, 6 - West, 7 - SouthWest
    }

    //Primitive move functions >NOT FINAL<
    public void RotateCharacter(WorldPosition target)//Rotates Characters Sprite to face selected WorldPosition
    {
        WorldPosition wp = new WorldPosition(m_charRef.WorldPosition);
        wp.SetAngle(GetDirection(target));
        m_charRef.UpdateWorldPosition(wp);
    }

    public void StartMove(List<Vector2> path)
    {
        StartCoroutine(Move(path));
    }

    private IEnumerator Move(List<Vector2> targets)
    {
        foreach (Vector2 v in targets)
        {
            float t = 0.0f;
            WorldPosition wp = new WorldPosition(m_charRef.WorldPosition);
            Vector2 start = m_charRef.transform.position;
            RotateCharacter(new WorldPosition(v));
            while (t < 1.0f)
            {
                t += Time.deltaTime * m_moveSpeed;
                wp = m_charRef.WorldPosition;
                //Vector2 pos = Vector2.Lerp(start, v, t);

                wp.x = Mathf.Lerp(start.x, v.x, t);// pos.x;
                wp.y = Mathf.Lerp(start.y, v.y, t);// pos.y;

                //float p = 8f / 256f;

                //wp.x = Mathf.Floor(wp.x / p) * p;
                //wp.y = Mathf.Floor(wp.y / p) * p;

                m_charRef.UpdateWorldPosition(wp);
                yield return null;
            }
            wp = m_charRef.WorldPosition;
            wp.x = v.x;
            wp.y = v.y;
            m_charRef.UpdateWorldPosition(new WorldPosition(wp));

            //yield return null;
        }
    }
    //End of primitive functions
}
