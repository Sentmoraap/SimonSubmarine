using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{

#region members

    public float _actionOffset;
    public float _actionRange = 3;
    public Animator _anim;

	private int m_currentLifePoint;

    private float m_moveSpeed = 0.25f;
    private float m_horizontalValue;
	private float m_verticalValue;

    private List<ActionObject> m_objects;
    private string m_currRoom;

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

    }

#endregion 

#region Character Functions

    public void UpdateMoveSpeed()
    {
        //TODO calculate move speed considering environment
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
            m_currRoom = other.name;
        }
    }

#endregion

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position + transform.right * _actionOffset, _actionRange);
    }

}
