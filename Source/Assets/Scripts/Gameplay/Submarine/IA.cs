using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour
{
    // TODO GD : set AI'S initial appreciation

#region PrivateAttributes
    private float m_appreciation; // AI's appreciation in [0;1]
#endregion 

    #region MonoBehaviour
    void Start ()
    {
        m_appreciation = 0.5f;    
	}
	
	void Update ()
    {

    }
    #endregion

    #region methods
    public void SimonSays()
    {
        // TODO GD : how to generate orders
        // TODO : generate orders
    }

    public void ModifyAppreciation(float diff)
    {
        m_appreciation = Mathf.Clamp(m_appreciation+diff, 0, 1);
    }
    #endregion
}
