using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : ActionObject {

#region members

    public Room[] _rooms;
    public float _lockDelay;
    public Animator _anim;

    private DoorState m_doorState;
    private float m_pressure;
    private float m_leak;

    private Timer m_timer;
    private bool m_isLocking;
    private bool m_ignoreUp;

#endregion

#region Properties

    public DoorState DoorState { get { return m_doorState; } set { m_doorState = value; } }

    public float Pressure { get { return m_pressure; } }

    public float Leak { get { return m_leak; } }


#endregion

#region Mono Functions

    public override void Start()
    {
        base.Start();

        DoorState = DoorState.Unreachable;
        m_timer = new Timer();
        m_isLocking = false;
        m_ignoreUp = false;
    }

    public override void Update()
    {
        base.Update();

        if(DoorState != DoorState.Locked)
        {
            UpdatePressure();
        }

        if((m_doorState == DoorState.Closed || m_doorState == DoorState.Locked) && m_isLocking && m_timer.IsElapsedLoop)
        {
            m_doorState = m_doorState == DoorState.Locked ? DoorState.Closed : DoorState.Locked ;
            m_isLocking = false;
            m_ignoreUp = true;
        }
    }

#endregion

#region Class Functions

    private void UpdatePressure()
    {
        //TODO Update room water or heat value considering the leak 
    }

#endregion

#region Overrided functions

    public override void activateActionUp()
    {
        if(m_ignoreUp)
        {
            m_ignoreUp = false;
            return;
        }


        Debug.Log(m_doorState);

        base.activateActionUp();

        switch (m_doorState)
        {
            case DoorState.Open:
                m_doorState = DoorState.Closed;
                Debug.Log(name + "close");
                _anim.SetTrigger("Close");
                m_isLocking = false;
                break;


            case DoorState.Closed:
                m_doorState = DoorState.Open;
                Debug.Log(name + "open");
                _anim.SetTrigger("Open");
                break;

        }
    }

    public override void activateActionDown()
    {
        base.activateActionDown();

        Debug.Log(m_doorState);

        switch (m_doorState)
        {

            case DoorState.Closed:
                _anim.SetTrigger("Lock");
                Debug.Log(name + "lock");
                m_timer.Reset(_lockDelay);
                m_isLocking = true;
                break;

            case DoorState.Locked :
                Debug.Log(name + "unlock");
                _anim.SetTrigger("Unlock");
                m_isLocking = true;
                break;
        }

    }

#endregion 

}

public enum DoorState
{
    Open,
    Closed,
    Locked,
    Unreachable
}