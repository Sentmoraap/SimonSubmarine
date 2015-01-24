using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{

#region constants
    private const float ROOM_LOW_LEVEL = 0.49f;
    private const float ROOM_HIGH_LEVEL = 4.8f;
#endregion

#region members

    public int _floor;
    public List<Door> _doors;
    public float _area; // _area in unity unit²
    private float m_health;
    //private List<Object> m_objects;
    
    public/*private*/ float m_waterValue=0; // in [0;1]
    private float m_heatValue=0; // in [0;1]

    private Transform m_waterPlane;
    private Transform m_cachePlane;

    private bool m_isVisible;

#endregion 

#region Properties

    public float Health { get { return m_health; } set { m_health = value; } }

    public float WaterValue { get { return m_waterValue; } }

    public float HeatValue { get { return m_heatValue; } }

    public bool IsVisible
    { 
        get { return m_isVisible; } 
        
        set
        { 
            m_isVisible = value;
            Vector3 pos = m_cachePlane.position;
            pos.y=value ? ROOM_LOW_LEVEL : ROOM_HIGH_LEVEL;
            m_cachePlane.position = pos;
            //TODO active a black plane when not visible
        }
    }

#endregion 

#region Mono Functions

    void Start ()
    {
        m_waterPlane = transform.Find("Water");
        m_cachePlane = transform.Find("Cache");
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
        m_health = (1 - m_waterValue) * (1 - m_heatValue);
    }

    private void UpdateWaterValue()
    {
        // For every door, half-update the water value of both rooms
        foreach(Door d in _doors)
        {
            Room otherRoom=d._rooms[0];
            if(otherRoom==this) otherRoom=d._rooms[1];
            propagate(d.DoorState, ref m_waterValue, ref otherRoom.m_waterValue, otherRoom._area, d.Leak);
            
        }
        Vector3 planePos=m_waterPlane.position;
        planePos.y = Mathf.Lerp(ROOM_LOW_LEVEL, ROOM_HIGH_LEVEL, m_waterValue);
        m_waterPlane.position = planePos;
    }

    private void UpdateHeatValue()
    {
        foreach(Door d in _doors)
        {
            Room otherRoom=d._rooms[0];
            if(otherRoom==this) otherRoom=d._rooms[1];
            propagate(d.DoorState, ref m_heatValue, ref otherRoom.m_heatValue, otherRoom._area, d.Pressure);
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