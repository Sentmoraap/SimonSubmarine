using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonTest : MonoBehaviour {
    public GameObject UiPanel;
    public Text BtnText;
    public Image bar;

    float progress;
    string dialogueText;
    Text dialogue;
    bool hit;

    public void Next()
    {
        BtnText.text = "Next";
    }
    public void Ok()
    {
        BtnText.text = "Ok";
    }
    void reduceLife(bool _hit)
    {
        float _progress = bar.fillAmount;
        if (_hit)
        {
            progress = Mathf.Lerp(bar.fillAmount, 0, Time.deltaTime * 0.5f);
            bar.fillAmount = progress;
            //_progress = progress;
        }
        else
        {
            _progress = bar.fillAmount;
            //bar.fillAmount = progress;
        }
    
    }
    public void Raise()
    {
        UiPanel.GetComponent<Animator>().SetBool("Rise", true);
        dialogue.text = "panel" + UiPanel.transform.GetSiblingIndex().ToString() + " this" + this.transform.GetSiblingIndex().ToString();

    }
    public void Reduce()
    {
        hit = true;
    }

    public void Stop()
    {
        hit = false;
    }
    private void Init()
    {
        UiPanel.GetComponent<Animator>().SetBool("Rise", false);
        UiPanel.transform.SetSiblingIndex(this.transform.GetSiblingIndex() - 1);
        dialogue = UiPanel.transform.GetComponentInChildren<Text>();
        Debug.Log(dialogue!=null);
    }

	// Use this for initialization
	void Start () {

        Init();

	}
	
	// Update is called once per frame
	void Update () {
        //progress = Mathf.Lerp(bar.fillAmount, 0, Time.deltaTime * 0.5f);
        reduceLife(hit);
        Debug.Log(hit);
	}
}
