using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManagerInGame : MonoBehaviour
{
    float testprogress;
    #region publicMembers
    public List<Transform> UISorting;
    public Text GlobalTimeTxt;
    public Text MissionTimeTxt;
    public Text PrincipalMissionTxt;
    public Image LifeProgressBarImg;

    public Transform PlayerPos;
    public GameObject MiniPlayer;
    #endregion

    #region PrivateAttributes
    private float LifeProgress;
    private string PrincipalMission;
    private int GlobalTime;
    private int MissionTime;
    private Vector3 miniCr = new Vector3(0,0,0);
    #endregion

    #region Methods
    void DepthSort(List<Transform> sortedList)
    {
        for (int i = 0; i < sortedList.Count; i++)
        {
            if (sortedList[i] != null)
            {
                sortedList[i].SetSiblingIndex(i);
                Debug.Log(sortedList[i].name +" "+ i);
            }
            else continue;
        }
    }

    void UpdateGlobalTimeUI(int t)
    {
        if (t >= 0)
        {
            GlobalTimeTxt.text = t + " Seconds Left";
        }
        else
        {
            t = 0;
            GlobalTimeTxt.text = t + " Seconds Left";
            return;
        }
    }

    void UpdateMissionTimeUI(int t)
    {
        if (t >= 0)
        {
            MissionTimeTxt.text = t + "\n Seconds left For the mission";
        }
        else
        {
            t = 0;
            MissionTimeTxt.text = t + "\n Seconds left For the mission";
            return;
        }
        
    }

    void UpdateObjectifUI(string _Objectif)
    {
        PrincipalMissionTxt.text = _Objectif;
    }

    void UpdateLifeBar(float Life)
    {
        LifeProgressBarImg.fillAmount = Life;
        Mathf.Clamp(LifeProgressBarImg.fillAmount, 0f, 1f);
    }
    void UpdateMiniPlayer(Transform player)
    {
       // MiniPlayer.position.Set(player.position.x * 10, player.position.z * 10, 0f);
        if (MiniPlayer != null)
        {
            miniCr.Set(player.position.x / 100, player.position.z / 100, 0f);
            MiniPlayer.transform.localPosition = miniCr;
        }
    }

    # endregion

    #region MonoBehaviour
    void Awake () {
        //DepthSort(UISorting);
	}
	void Update () {
        testprogress = Time.time;
        UpdateGlobalTimeUI(50 - (int)testprogress);
        UpdateMissionTimeUI(10 - (int)testprogress);
        UpdateObjectifUI("Mission Obj");
        UpdateLifeBar(1-testprogress/20);
        UpdateMiniPlayer(PlayerPos);
    }
    #endregion
}
