using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{

#region constants
    private const float ROOM_HEIGHT=10f;
    private const float CRACK_FILL_SPEED = 0.5f;
#endregion

#region members

    public int _id;
    public List<Door> _doors;
    public float _area; // _area in unity unit²
    public int id;

    private float m_health;
    
    private float m_waterValue=1; // in [0;1]
    private float m_heatValue=0; // in [0;1]
    private float m_electricityValue=0; // in [0;1]

    private Transform m_waterPlane;
    private Transform m_cachePlane;

    private bool m_isVisible;

    private bool m_hasCrack;
#endregion 

#region Properties

    public float Health { get { return m_health; } set { m_health = value; } }

    public float WaterValue { get { return m_waterValue; } set { m_waterValue = value; } }

    public float HeatValue { get { return m_heatValue; } }
    
    public float ElectricityValue { get { return m_electricityValue; } }

    public bool IsVisible
    { 
        get { return m_isVisible; } 
        
        set
        { 
            m_isVisible = value;
            if (m_cachePlane == null) Start();
            Vector3 pos = m_cachePlane.localPosition;
            pos.y=value ? 0 : ROOM_HEIGHT;
            m_cachePlane.localPosition = pos;
        }
    }

    public bool HasCrack
    {
        get { return m_hasCrack;}
        set { m_hasCrack = value; }
    }

#endregion 

#region Mono Functions

    void Start ()
    {
        m_waterPlane = transform.Find("Water");
        m_cachePlane = transform.Find("Cache");
	}
	
	public void _update ()
    {
        UpdateWaterValue();
        UpdateHeatValue();
        UpdateElectricityValue();
        UpdateHealth();
	}

#endregion 

#region Class Functions

    private void UpdateHealth()
    {
        m_health = (1f - m_waterValue) * (1f - m_heatValue) * (1f - m_electricityValue);
    }

    private void UpdateWaterValue()
    {
        // For every door, half-update the water value of both rooms
        foreach(Door d in _doors)
        {
            Room otherRoom=d._rooms[0];
            if(otherRoom==this) otherRoom=d._rooms[1];
            propagate(d.DoorState, ref m_waterValue, ref otherRoom.m_waterValue, otherRoom._area, d.WaterLeak);
            
        }
        if(m_hasCrack)
        {
            m_waterValue = Mathf.Max(1, m_waterValue + CRACK_FILL_SPEED / _area);
        }
        Vector3 planePos=m_waterPlane.localPosition;
        planePos.y = Mathf.Lerp(0, ROOM_HEIGHT, m_waterValue);
        m_waterPlane.localPosition = planePos;
    }

    private void UpdateHeatValue()
    {
        foreach(Door d in _doors)
        {
            Room otherRoom=d._rooms[0];
            if(otherRoom==this) otherRoom=d._rooms[1];
            propagate(d.DoorState, ref m_heatValue, ref otherRoom.m_heatValue, otherRoom._area, d.HeatLeak);
        }
        //TODO : graphic effect
    }

    private void UpdateElectricityValue()
    {
        foreach(Door d in _doors)
        {
            Room otherRoom=d._rooms[0];
            if(otherRoom==this) otherRoom=d._rooms[1];
            propagate(d.DoorState, ref m_electricityValue, ref otherRoom.m_electricityValue, otherRoom._area, d.ElectricityLeak);
        }
        //TODO : graphic effect
    }

    private void propagate(DoorState doorState, ref float ownValue, ref float otherValue, float otherArea, float leak)
    {
        switch(doorState)
        {
            case DoorState.Open:
                {
                    float meanValue = (ownValue * _area + otherValue * otherArea) / (_area + otherArea);
                    ownValue = meanValue;
                    otherValue = meanValue;
                }
                break;
            case DoorState.Closed:
                {
                    int redoTimes = 4;
                    bool mustRedo = false;
                    float diffTime = leak * Time.deltaTime / 2;
                    do // Repeat multiple times to adjust in case there is too many water moved
                    {
                        mustRedo = false;
                        redoTimes--;
                        if (otherValue > ownValue)
                        {
                            if (diffTime / _area + ownValue > 1) diffTime = (1 - ownValue) * _area;
                            if (otherValue - diffTime / otherArea < 0) diffTime = otherValue * otherArea;
                            ownValue += diffTime / _area;
                            ownValue -= diffTime / otherArea;
                            if (ownValue > otherValue) mustRedo = true;
                        }
                        else
                        {
                            if (diffTime / otherArea + otherValue > 1) diffTime = (1 - otherValue) * otherArea;
                            if (ownValue - diffTime / _area < 0) diffTime = ownValue * _area;
                            ownValue -= diffTime / _area;
                            otherValue += diffTime / otherArea;
                            if (ownValue < otherValue) mustRedo = true;
                        }
                        diffTime /= 2;
                    } while (mustRedo && redoTimes > 0);
                }
                break;
            case DoorState.Locked:
            case DoorState.Unreachable:
                break;
            default:
                Debug.LogError("Unhandled DoorState");
                break;
        }
    }

#endregion

}

public enum RoomState
{
    //TODO must be define with game designers
}