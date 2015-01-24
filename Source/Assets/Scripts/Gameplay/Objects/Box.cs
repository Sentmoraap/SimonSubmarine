using UnityEngine;
using System.Collections;

public class Box : ActionObject {


    protected override void activateAction()
    {
        base.activateAction();

        if(transform.parent == null)
        {
            transform.parent = GameObject.Find("Character").transform;
            rigidbody.isKinematic = true;
        }
    }

    protected override void disactivateAction()
    {
        base.disactivateAction();

        transform.parent = null;
        rigidbody.isKinematic = false;
    }
}
