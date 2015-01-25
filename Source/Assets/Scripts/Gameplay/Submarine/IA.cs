using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IA : MonoBehaviour
{
    // TODO GD : set AI'S initial appreciation

#region Members

    public Mission _currMission;

    private List<Mission> m_missions;
    private float m_appreciation; // AI's appreciation in [0;1]
    private static IA m_instance;
#endregion

#region Singleton

    public static IA Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<IA>();
                if (m_instance == null)
                {
                    GameObject singleton = new GameObject();
                    singleton.name = "GenericPool";
                    m_instance = singleton.AddComponent<IA>();
                }
            }

            return m_instance;
        }
    }

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            m_instance.Init();
        }
        else
        {
            if (this != m_instance)
                Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        m_appreciation = 0.5f;

        m_missions = new List<Mission>();
        //level 1 missions
        m_missions.Add(new Mission("MonkeyWrench", "Power"));
        m_missions.Add(new Mission("AirLock"));
        m_missions.Add(new Mission("Pump"));
        m_missions.Add(new Mission("Movie"));
        m_missions.Add(new Mission("BubbleGum"));
        m_missions.Add(new Mission("Power"));

        //level 2 missions
        m_missions.Add(new Mission("Aquarium"));
        m_missions.Add(new Mission("Moray"));
        m_missions.Add(new Mission("Sponge", "oven"));
        m_missions.Add(new Mission("Sponge", "BatteryRoom"));
        m_missions.Add(new Mission("Cook"));

        //level 3 missions
        m_missions.Add(new Mission("CowBox", "WasteDisposal"));
        m_missions.Add(new Mission("Disposer"));
        m_missions.Add(new Mission("Sleep"));
        m_missions.Add(new Mission("Hat"));
        m_missions.Add(new Mission("CowBox", "WasteDisposal"));
        m_missions.Add(new Mission("Radar"));

        m_missions.Add(new Mission("?", "?"));

        _currMission = m_missions[0];
        _currMission._timing = 10;

    }

#endregion

#region MonoBehaviour
	
	void Update ()
    {
        _currMission.UpdateMission();

        //if (Input.GetKeyUp(KeyCode.Return))
        //{
        //    Debug.Log(m_missions.Count);
        //}
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
