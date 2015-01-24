﻿using UnityEngine;
using System.Collections;

public abstract class ActionObject : MonoBehaviour
{
    #region publicAttributes
    public bool _isActivated;
    public bool _isMovable;
    #endregion

    
    #region MonoBehaviour
    void Start ()
    {
	    _isActivated=false;
        _isMovable=false;
	}
	
	void Update ()
    {
        doCommonStuff();
        if(_isActivated) doActivatedStuff(); else doDisactivatedStuff();
    }
    #endregion

    #region virtualMethods
    protected virtual void doCommonStuff() {}
    protected virtual void doActivatedStuff() {}
    protected virtual void doDisactivatedStuff(){}

    #endregion
}
