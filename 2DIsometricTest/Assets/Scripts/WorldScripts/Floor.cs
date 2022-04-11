using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : WorldObject
{
    protected const float m_xOffset = 0.5f;
    //Overrides to set name based on World Positon and then adds itself to World Dictionairy
    protected override void SetName()
    {
        name = m_name + "(X:" + m_worldPosition.x + ",Y:" + m_worldPosition.y + ")";
        //m_world.RegisterNewFloor(this);
    }
}
