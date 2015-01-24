using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Vector3 _offset;
    public Transform _target;

	void Update ()
    {
        transform.position = _target.position + _offset;
	}
}
