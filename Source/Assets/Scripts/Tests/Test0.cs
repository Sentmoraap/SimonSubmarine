using UnityEngine;
using System.Collections;

public class Test0 : MonoBehaviour
{

	void Start ()
    {
        // Submarine test
        Submarine submarine = new Submarine();
        submarine.UnlockNextPhase();
        if (submarine.UnlockedRooms != 3) Debug.LogError("Plop");
        submarine.UnlockNextPhase();
        submarine.UnlockNextPhase();
        if (submarine.UnlockedRooms != 10) Debug.LogError("Plop");

        // Mission test
        Mission m = gameObject.AddComponent<Mission>();
        m._timing = 0.1f;
        if (m.isEllapsed()) Debug.LogError("Plop");
        m.Start();
        if (m.isEllapsed()) Debug.LogError("Plop");
	}
	
	void Update ()
    {
	
	}
}
