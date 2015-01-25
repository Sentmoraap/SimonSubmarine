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
        //m_missions.Add(new Mission("MonkeyWrench", "Power"));
        //m_missions.Add(new Mission("AirLock",0,
        //    "Ahoy there, matey, I'm t' Captain Simon. This is my submarine and from now on, you have t' obey me ! First mission, take that ladder and open t' air lock ! Yaarg !"));
        m_missions.Add(new Mission("Pump",0,
            "Okay. Now, time t' use your tiny arms. Go start t' water pump ! You'll need it t' evacuate t' water on t' floor."));
        m_missions.Add(new Mission("Movie",0,"Focus, me bucko. the hour to learn. be off in th' projection room to watch a jolly barnacle-covered instructional movie !"));
        m_missions.Add(new Mission("BubblGum","Engine_Low","Oww, thar's a crack in t' engine room, take a chewin' gum 'n fix it !"));
        m_missions.Add(new Mission("Power",0,"Some nasty creatures be hangin' around out thar. Switch off th' power 'o th' submarine, quick !"));

        //level 2 missions
        m_missions.Add(new Mission("Aquarium",0,"Me poor fishes be sooo famished in th' aquarium. Please feed them..."));
        m_missions.Add(new Mission("Moray",0,"Ye'll need a moray because... Because err.. reasons. Be off catch it in th' aquarium !"));
        m_missions.Add(new Mission("Sponge", "oven","Ow ow oww. I forgot to brin' Mrs. Sponge in his own ship. I just can hold it in me hands... Please... Do it fer me !"));
        m_missions.Add(new Mission("Sponge", "BatteryRoom","Set the sails, scurvy pirate ! Cookin' needs lust like sailin' ! Be off cook a dirty meal !"));
        m_missions.Add(new Mission("Cook",0,"Set the sails, scurvy pirate ! Cookin' needs lust like sailin' ! Be off cook a dirty meal !"));

        //level 3 missions
        m_missions.Add(new Mission("CowBox", "WasteDisposal"));
        m_missions.Add(new Mission("Disposer",0,"Oh I be needin' him to start that dangerous thi.. Oh ye're thar ?  I have some... things to destroy, be off start th' disposer !"));
        m_missions.Add(new Mission("Sleep",0,"You look tired, sailor. Go hunt treasures for me in your dreams !"));
        m_missions.Add(new Mission("Hat",0,"Yaar what's be ye doin'? Don't ye spy wit' ye eye thar's transparent rum everywhere? Use a hat to stop that ! Now !"));
        m_missions.Add(new Mission("CowBox", "WasteDisposal","Thar's nasty creatures in th' waste disposal department. Be off get'em ! Wit' a mooing cow box ! Don't ask."));
        m_missions.Add(new Mission("Radar",0,"Th' kraken be somethere out thar... or maybe 'tis a giant whale. Be off and start th' radar, would ya ?"));

        m_missions.Add(new Mission("?", "?"));

        _currMission = m_missions[0];

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
