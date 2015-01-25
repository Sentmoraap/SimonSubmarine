using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
#region constants
    private const float BASE_MOVE_SPEED = 0.25f;

    private const float SLOWDOWN_WATER_LEVEL = 0.1f;
    private const float SLOWDOWN_SPEED_MULT = 0.5f;
    private const float DROWN_WATER_LEVEL = 0.5f;
    private const float OXYGEN_DECREASE_RATE = 0.3f; // /s
    private const float OXYGEN_INCREASE_RATE = 1; // /s
    private const float HEALTH_DROWN_DECREASE_RATE = 0.1f; // /s
    private const float STEAM_HURT_THRESHOLD = 1f;
    private const float STEAM_HURT_RATE = 0.1f; // /s
    private const float ELECTRICITY_HURT_DIST = 5;
    private const float ELECTRICITY_THRESHOLD = 1;
    private const float ELECTRICITY_WATER_MULT = 50; // value multiplied by water level and added to electricity value
    private const float ELECTRICITY_HURT_DAMAGE = 0.2f;

#endregion

#region members

    public float _actionOffset;
    public float _actionRange = 3;
    public Animator _anim;

	private float m_health=1;// in [0;1]
    private float m_oxygen=1;// in [0;1]
    private float m_distLastElectroshock = 0;

    private float m_moveSpeed = 0.25f;
    private float m_horizontalValue;
	private float m_verticalValue;

    private bool m_waterSlowdown = false;
    private List<ActionObject> m_objects;
    private Room m_currRoom;

#endregion

#region Properties

    public float MoveSpeed
    {
        get { return m_moveSpeed; }
        set { m_moveSpeed = value; }
    }

	public float HorizontalValue 
	{
		get{return m_horizontalValue;}
		set { m_horizontalValue = value; }
	}

	public float VerticalValue
	{ 
		get{return m_verticalValue;}
		set { m_verticalValue = value; }
	}

#endregion

#region Mono Functions

    void Start()
	{
        m_objects = new List<ActionObject>();

		KeyBinder.Instance.DefineActions("Horizontal", new AxisActionConfig(KeyType.Movement, 0, value => { HorizontalValue = value; } ));
		KeyBinder.Instance.DefineActions("Vertical", new AxisActionConfig(KeyType.Movement, 0, value => { VerticalValue = value; } ));
        KeyBinder.Instance.DefineActions("ButtonA", new KeyActionConfig(KeyType.Action, 0, ActionDown, ActionUp));
	}

	void Update()
	{
        UpdateCloseActionObject();

        // Check drowning and water slowdown
        float waterLevel = m_currRoom.WaterValue;
        if(waterLevel>=DROWN_WATER_LEVEL)
        {
            if(m_oxygen<=0)
            {
                m_health = Mathf.Max(0, m_health - HEALTH_DROWN_DECREASE_RATE * Time.deltaTime);
            }
            else
            {
                m_oxygen = Mathf.Max(m_oxygen-OXYGEN_DECREASE_RATE * Time.deltaTime,0);
            }
        }
        else
        {
            m_oxygen = Mathf.Min(m_oxygen+OXYGEN_INCREASE_RATE * Time.deltaTime,1);
        }
        m_waterSlowdown = waterLevel >= SLOWDOWN_WATER_LEVEL;

        // Check steam
        float heatLevel = m_currRoom.HeatValue;
        if(heatLevel>=STEAM_HURT_THRESHOLD)
        {
            m_health = Mathf.Max(0, m_health - STEAM_HURT_RATE * Time.deltaTime);
        }

        // Check electricity
        float electricityLevel = m_currRoom.ElectricityValue;
        electricityLevel += electricityLevel * waterLevel * ELECTRICITY_WATER_MULT;
        if(electricityLevel>ELECTRICITY_THRESHOLD)
        {
            if(m_distLastElectroshock>=ELECTRICITY_HURT_DIST)
            {
                m_distLastElectroshock -= ELECTRICITY_HURT_DIST;
                m_health = Mathf.Max(0, m_health - ELECTRICITY_HURT_DAMAGE);
                // TODO : hurt animation and stun
            }
        }
        else
        {
            m_distLastElectroshock = 0;
        }
	}

    void FixedUpdate()
    {
        UpdateMoveSpeed();
        if (HorizontalValue != 0f || VerticalValue != 0f)
        {
            _anim.SetBool("Walk", true);
            ApplyMove(MoveSpeed);
        }
        else
        {
            _anim.SetBool("Walk", false);
        }

        if(Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log(m_currRoom.gameObject.name);
        }
    }

#endregion 

#region Character Functions

    public void UpdateMoveSpeed()
    {
        m_moveSpeed = BASE_MOVE_SPEED;
        if (m_waterSlowdown)
        {
            _anim.SetBool("Water", true);
            m_moveSpeed *= SLOWDOWN_SPEED_MULT;
        }
        else
        {
            _anim.SetBool("Water", false);
        }
    }

    public void UpdateCloseActionObject()
    {
        //TODO add closed actionobject to the list to be able to interact with use sphere collision to detect and getcomponent<ActionObject>

        m_objects.Clear();
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.right * _actionOffset, _actionRange, ~(1 << LayerMask.NameToLayer("Player")));
        
        foreach(Collider col in cols)
        {
            ActionObject aObj = col.GetComponent<ActionObject>();
            if(aObj != null )
            {
                m_objects.Add(aObj);
            }
        }
        //return cols.Length > 0;
    }

	void ApplyMove(float moveSpeed)
	{
        var direction = new Vector3(HorizontalValue, 0f, VerticalValue).normalized;
		transform.position += direction * moveSpeed;
		transform.rotation = Quaternion.Euler(new Vector3(0f, (Mathf.Atan2(-VerticalValue, HorizontalValue) * 180 / Mathf.PI), 0f));
        rigidbody.angularVelocity = Vector3.zero;
        m_distLastElectroshock += moveSpeed;
    }

    void ActionDown()
    {
        if(m_objects.Count > 0)
        {
            m_objects[0].activateActionDown();
        }
    }

    void ActionUp()
    {
        if(m_objects.Count > 0)
        {
            if(m_objects[0]._isMovable)
            {
                _anim.SetBool("Carry", m_objects[0].IsActivated ? false : true);
            }

            m_objects[0].IsActivated = m_objects[0].IsActivated ? false : true;
            m_objects[0].activateActionUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            m_currRoom = other.gameObject.GetComponent<Room>();
        }
    }

#endregion

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position + transform.right * _actionOffset, _actionRange);
    }

}
