using UnityEngine;
using System.Collections;

public abstract class ActionObject : MonoBehaviour
{
    #region publicAttributes
    public bool _isMovable;
    #endregion

    #region properties
    public bool IsActivated
    {
        get
        {
            return m_isActivated;
        }
        set
        {
            if(value==m_isActivated) return;
            if(value) activateAction(); else disactivateAction();
            m_isActivated=value;
        }
    }
    #endregion

    #region privateAttributes
        bool m_isActivated;
    #endregion
    
    #region MonoBehaviour
    public virtual void Start ()
    {
	    m_isActivated=false;
        _isMovable=false;
	}

    public virtual void Update()
    {
        doCommonStuff();
        if(m_isActivated) doActivatedStuff(); else doDisactivatedStuff();
    }
    #endregion

    #region virtualMethods

    /// <summary>
    /// Called each update
    /// </summary>
    protected virtual void doCommonStuff() {}

    /// <summary>
    /// Called each update if the object is activated
    /// </summary>
    protected virtual void doActivatedStuff() {}

    /// <summary>
    /// Called each update if the object is disactivated
    /// </summary>
    protected virtual void doDisactivatedStuff(){}

    /// <summary>
    /// Called one time when activating the object
    /// </summary>
    protected virtual void activateAction() { }

    /// <summary>
    /// Called one time when activating button is down the object
    /// </summary>
    protected virtual void activateActionDown() { }

    /// <summary>
    /// Called one time when activating button is up the object
    /// </summary>
    protected virtual void activateActionUp() { }

    /// <summary>
    /// Called one time when disactivating the object
    /// </summary>
    protected virtual void disactivateAction() {}
    #endregion
}
