using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Submarine : MonoBehaviour
{
    #region constants
    private const float EVENT_PERIOD=120;
    private const float EVENT_DURATION = 10; 
    #endregion

    #region publicTYpes
    public enum EventState
    {
        NO_EVENT = 0,
        VOLCANO = 1,
        KRAKEN =2,
        MAX_EVENT=KRAKEN
    };
    #endregion

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

    public float TimeLeft
    {
        get
        {
            return m_startTime + m_timeLimit - Time.time;
        }
    }
    #endregion

    #region privateAttributes
    private int m_unlockedRooms=0;
    private int m_phase = 0;
    private float m_health; // Global health in [0;1]
    private List<int> m_phaseRoomUnlocks;
    private float m_startTime;
    private float m_timeLimit;
    private EventState eventState=EventState.NO_EVENT;
    #endregion

    #region monobehaviour

    public void Start()
    {
        m_phaseRoomUnlocks=new List<int>();
        m_phaseRoomUnlocks.Add(0);
        m_phaseRoomUnlocks.Add(4);
        m_phaseRoomUnlocks.Add(6);
        m_phaseRoomUnlocks.Add(10);
        m_startTime = Time.time;
        m_timeLimit = 600;
        for (int i = 0; i < _rooms.Count; i++) _rooms[i]._id = i;
        UnlockNextPhase();
    }

    public void Update()
    {
        if(Time.time>m_startTime+m_timeLimit) return; //TODO : game over eent
        bool isInEvent = (Time.time - m_startTime) % EVENT_PERIOD < EVENT_DURATION && Time.time - m_startTime >= EVENT_PERIOD;
        if(isInEvent && eventState==EventState.NO_EVENT)
        {
            eventState = (EventState) Random.Range(1, (int)EventState.MAX_EVENT + 1);

            // TODO : do actual things instead of just printing
            if (eventState == EventState.VOLCANO) Debug.Log("Underwater volcano eruption");
            if (eventState == EventState.KRAKEN) Debug.Log("Kraken");
        }
        if (!isInEvent) eventState = EventState.NO_EVENT;
    }
    #endregion

    #region methods
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
        updateRoomDisplays();
        updateDoorLocks();
    }

    private void updateRoomDisplays()
    {
        for(int i=0;i<_rooms.Count;i++)
            _rooms[i].IsVisible=i<m_unlockedRooms;
    }

    private void updateDoorLocks()
    {
        foreach(Room r in _rooms)
            foreach(Door d in r._doors)
            {
                bool unreachable = d._rooms[0]._id>=m_unlockedRooms || d._rooms[1]._id>=m_unlockedRooms;
                if (unreachable) d.DoorState = DoorState.Unreachable;
                if (!unreachable && d.DoorState == DoorState.Unreachable) d.DoorState = DoorState.Locked;
            }
    }

    public void AddTime(float seconds)
    {
        m_timeLimit += seconds;
    }
    #endregion
}
