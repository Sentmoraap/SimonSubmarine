using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

#region members

    public int _floor;

    private float m_health;
    //private List<Object> m_objects;
    private List<Door> m_doors;
    
    private float m_waterValue;
    private float m_heatValue;

    private bool m_isVisible;

#endregion 

#region Properties

    public float Health { get { return m_health; } set { m_health = value; } }

    public List<Door> Doors { get { return m_doors; } }

    public float WaterValue { get { return m_waterValue; } }

    public float HeatValue { get { return m_heatValue; } }

    public bool IsVisible
    { 
        get { return m_isVisible; } 
        
        set
        { 
            m_isVisible = value;
            //TODO active a black plane when not visible
        }
    }

#endregion 

#region Mono Functions

    void Start ()
    {
	
	}
	
	void Update ()
    {
        //TODO each of them might be update only considering a certain state of the room
        UpdateHealth();
        UpdateWaterValue();
        UpdateHeatValue();
	}

#endregion 

#region Class Functions

    private void UpdateHealth()
    {
        //TODO Update room health considering water and heat value
    }

    private void UpdateWaterValue()
    {
        //TODO Update water value considering water propagation
    }

    private void UpdateHeatValue()
    {
        //TODO Update Heat value considering Heat propagation
    }

#endregion

}

public enum RoomState
{
    //TODO must be define with game designers
}