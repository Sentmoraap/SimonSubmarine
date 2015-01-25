using UnityEngine;
using System.Collections;

public class Pump : ActionObject
{

    #region constants
    private const float PUMP_SPEED = 0.05f; // in unit/s
    private const float PUMP_DURATION = 20; // s
    private const float PUMP_COOLDOWN = 30;
    #endregion

    #region private members
    private float m_activationTime=0;
    #endregion
    
    public  override void Start () {
	    base.Start();
	}
	
	public override void Update () {
        base.Update();
	    if(m_activationTime!=0)
        {
            if(m_activationTime<PUMP_DURATION)
            {
                foreach(Room r in Submarine._instance._rooms) r.WaterValue=Mathf.Max(0,r.WaterValue-PUMP_SPEED*Time.deltaTime);
            }
            m_activationTime+=Time.deltaTime;
            if(m_activationTime>PUMP_COOLDOWN) m_activationTime=0;
        }
	}


    public override void activateActionDown()
    {
        base.activateActionDown();
        Debug.Log("Pump action down");
        if (m_activationTime == 0) m_activationTime = 0.0001f;
    }
}
