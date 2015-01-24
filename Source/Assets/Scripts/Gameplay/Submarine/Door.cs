using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

#region members

    public Room[] m_rooms;

    private DoorState m_doorState;
    private float m_pressure;
    private float m_leak;

#endregion

#region Properties

    public DoorState DoorState { get { return m_doorState; } set { m_doorState = value; } }

    public float Pressure { get { return m_pressure; } }

    public float Leak { get { return m_leak; } }


#endregion

#region Mono Functions

    void Start()
    {
        DoorState = DoorState.Locked;
    }

    void Update()
    {
        if(DoorState == DoorState.Locked)
        {
            UpdatePressure();
        }
    }

#endregion

#region Class Functions

    private void UpdatePressure()
    {
        //TODO Update room water or heat value considering the leak 
    }

#endregion

}

public enum DoorState
{
    Open,
    Closed,
    Locked
}