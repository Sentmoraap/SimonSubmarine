using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Submarine
{
    #region publicAttributes
    // The rooms must be ordered in unlock order
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

    public int UnlockedRooms
    {
        get
        {
            return m_unlockedRooms;
        }
    }
    #endregion

    #region privateAttributes
    private int m_unlockedRooms=0;
    private int m_phase = 0;
    private float m_health; // Global health in [0;1]
    private List<int> m_phaseRoomUnlocks;
    #endregion

    #region methods

    public Submarine()
    {
        m_phaseRoomUnlocks=new List<int>();
        m_phaseRoomUnlocks.Add(0);
        m_phaseRoomUnlocks.Add(3);
        m_phaseRoomUnlocks.Add(6);
        m_phaseRoomUnlocks.Add(10);
    }

    public void UpdateHealth()
    {
        //TODO when we have rooms
    }

    // Can be used with a negative number to lock rooms
    /*public void UnlockRooms(int nbRooms)
    {
        _unlockedRooms += nbRooms;
        if(_unlockedRooms<0) _unlockedRooms=0;
        if (_unlockedRooms > _rooms.Count - 1) _unlockedRooms = _rooms.Count - 1;
    }*/

    public void UnlockNextPhase()
    {
        m_phase++;
        m_unlockedRooms = m_phaseRoomUnlocks[m_phase];
    }

    #endregion
}
