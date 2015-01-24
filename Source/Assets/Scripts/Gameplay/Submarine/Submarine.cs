using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Submarine
{
    #region publicAttributes
    // The rooms must be ordered in unlock order
    public List<Room> _rooms;
    public int _unlockedRooms=0; // If unlockedRooms is x, the player can go in rooms [0;x-1]
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

    // Can be used with a negative number to lock rooms
    public void UnlockRooms(int nbRooms)
    {
        _unlockedRooms += nbRooms;
        if(_unlockedRooms<0) _unlockedRooms=0;
        if (_unlockedRooms > _rooms.Count - 1) _unlockedRooms = _rooms.Count - 1;
    }
    #endregion
}
