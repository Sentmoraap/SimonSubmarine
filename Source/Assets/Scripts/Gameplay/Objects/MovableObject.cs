using UnityEngine;
using System.Collections;

public class MovableObject : ActionObject {

    public Transform _lastParent;

    protected override void activateAction()
    {
        base.activateAction();

        if(transform.parent.name != "Character" )
        {
            _lastParent = transform.parent;
            transform.parent = GameObject.Find("Character").transform;
            rigidbody.isKinematic = true;
        }
    }

    protected override void disactivateAction()
    {
        base.disactivateAction();

        transform.parent = _lastParent;
        rigidbody.isKinematic = false;
    }
}
