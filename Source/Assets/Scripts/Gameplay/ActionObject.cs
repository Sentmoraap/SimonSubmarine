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
    void Start ()
    {
	    m_isActivated=false;
        _isMovable=false;
	}
	
	void Update ()
    {
        doCommonStuff();
        if(m_isActivated) doActivatedStuff(); else doDisactivatedStuff();
    }
    #endregion

    #region virtualMethods
    protected virtual void doCommonStuff() {}
    protected virtual void doActivatedStuff() {}
    protected virtual void doDisactivatedStuff(){}
    protected virtual void activateAction() { };
    protected virtual void disactivateAction() {};
    #endregion
}
