using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    #region publicMembers
    public List<GameObject> BtnPrefab = new List<GameObject>();
    public GameObject UIRoot;
    public GameObject CreditsUI;
    public string[] BtnName;
    #endregion

    #region PrivateAttributes
    private List<GameObject> BtnInScene = new List<GameObject>();
    private bool endMove =false;
    private bool forward = true;
    List<Action> BtnMethods = new List<Action>();

    #endregion

    #region methods
    public void startMove()
    {
        endMove = false;
    }

    public void showCredits(bool On)
    {
        if (On)
            CreditsUI.SetActive(true);
        else
            CreditsUI.SetActive(false);
    }
    public void test1()
    {
        Application.LoadLevel("TestMathilda");
    }

    public void test2()
    {
        Debug.Log("clicked2");
        showCredits(true);
    }

    public void test3()
    {
        Debug.Log("clicked3");
        Application.Quit();
    }
    void ShowButton(float offset)
    {
        for (int i = 0; i < BtnPrefab.Count; i++)
        {
            if (BtnPrefab[i] != null)
            {
                GameObject Btn = Instantiate(BtnPrefab[i], UIRoot.transform.position, Quaternion.identity)as GameObject;
                Btn.transform.SetParent(UIRoot.transform);
                Btn.name = i.ToString();
                //Btn.GetComponent<Button>().onClick.AddListener(() => { BtnMethods[i](); });
                Btn.GetComponentInChildren<Text>().text = BtnName[i];
                Vector3 offpos = new Vector3(0, -offset * i, 0);
                Btn.transform.GetComponent<RectTransform>().Translate(offpos);
                BtnInScene.Add(Btn);
            }
        }
    }
    void addMethods()
    {
        BtnMethods.Add(() => test1());
        BtnMethods.Add(() => test2());
        BtnMethods.Add(() => test3());
        BtnInScene[0].GetComponent<Button>().onClick.AddListener(() => { BtnMethods[0](); });
        BtnInScene[1].GetComponent<Button>().onClick.AddListener(() => { BtnMethods[1](); });
        BtnInScene[2].GetComponent<Button>().onClick.AddListener(() => { BtnMethods[2](); });
        //for (int i = 0; i < BtnInScene.Count; i++)
        //{
        //    BtnInScene[i].GetComponent<Button>().onClick.AddListener(() => { BtnMethods[i](); });
        //}
    }
    void MoveBtn(float speed,float goal)
    {
        if (endMove) return;
        float start =  0f;
            UIRoot.transform.Translate(speed * Time.deltaTime,0,0);
            for (int i = 0; i < BtnPrefab.Count; i++)
            {
                BtnInScene[i].transform.GetComponent<RectTransform>().Translate(i * speed * Time.deltaTime, 0f, 0f);
            }
            start = BtnInScene[BtnInScene.Count-1].transform.GetComponent<RectTransform>().localPosition.x;
            if (start >= goal && forward)
            {
                endMove = true;
            }
            else if (!forward && start <= goal)
            {
                endMove = true;
            }
    }

    void MoveForward(bool _forward, float _speed, float _goal)
    {
        forward = _forward;
        if (_forward)
        { 
            MoveBtn(_speed, _goal);
        }
        else MoveBtn(-_speed, -_goal);
    }

    #endregion
    #region MonoBehaviour
    // Use this for initialization
	void Awake () {
        ShowButton(50f);
        addMethods();
        CreditsUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        //if(Input.getKey)
        MoveForward(true, 150, 200);
    }
    #endregion
}
