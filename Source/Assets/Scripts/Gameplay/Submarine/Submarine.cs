using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Submarine
{
    #region publicAttributes
    public List<Room> _rooms;
    #endregion

    #region properties
    public float Health
    {
        get 
        {
            return m_health;
        }
    }
    #endregion

    #region privateAttributes
    private float m_health; // Global health in [0;1]
    #endregion

    #region methods
    public void UpdateHealth()
    {
        //TODO when we have rooms
    }
    #endregion
}
