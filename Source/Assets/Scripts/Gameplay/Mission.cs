using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission
{
    #region publicTypes
    // TODO : define generic cases and handle them
    public enum ResultType
    {
        TIME_BONUS,
        IA_BONUS,
        SPECIAL
    }
    
    public class Params
    {
    }

    public class FloatParam : Params
    {
        public float value;
    }

    public struct Result
    {
        public ResultType type;
        public Params parameters;
    }

    public struct CompleteCondition
    {
        public bool _useObject;
        public string _object;
        public string _room;
        public Params parameters;
    }
    #endregion

    #region publicAttributes
    public float _timing;
    #endregion

    #region privateAttributes
    private float m_startTime=-1;
    private bool m_finished = false;
    private bool m_completed = false;
    private List<Result> m_positiveResults;
    private List<Result> m_negativeResults;
    private List<Result> m_denyResults;
    private CompleteCondition m_completeCondition;
    private Submarine m_submarine;
    #endregion

    #region MonoBehaviour
    public Mission(string obj, string room)
    {
        m_startTime = Time.time;
        m_positiveResults = new List<Result>();
        m_negativeResults = new List<Result>();
        m_denyResults = new List<Result>();
        m_completeCondition._useObject = true;
        m_completeCondition._object = obj;
        m_completeCondition._room = room;
    }

    public Mission(string obj)
    {
        m_startTime = Time.time;
        m_positiveResults = new List<Result>();
        m_negativeResults = new List<Result>();
        m_denyResults = new List<Result>();
        m_completeCondition._useObject = false;
        m_completeCondition._object = obj;
    }

    public void UpdateMission()
    {
        if(!m_finished)
        {
            if(isEllapsed())
            {
                m_finished = true;
                applyNegativeResults();
            }
            else if(m_completed)
            {
                m_finished = true;
                applyPositiveResults();
            }
        }
    }
    #endregion

    #region publicMethods
    public void setDefaultGoodMission()
    {
        Result pos;
        pos.type=ResultType.TIME_BONUS;
        FloatParam fp=new FloatParam();
        fp.value=20;
        pos.parameters=fp;
        m_positiveResults.Add(pos);

        Result neg;
        neg.type=ResultType.TIME_BONUS;
        fp=new FloatParam();
        fp.value=-20;
        neg.parameters=fp;
        m_negativeResults.Add(neg);
    }

    public void setDefaultBadMission()
    {
        Result pos;
        pos.type=ResultType.TIME_BONUS;
        FloatParam fp=new FloatParam();
        fp.value=-10;
        pos.parameters=fp;
        m_positiveResults.Add(pos);
        
        Result den;
        den.type=ResultType.TIME_BONUS;
        fp=new FloatParam();
        fp.value=10;
        den.parameters=fp;
        m_denyResults.Add(den);
    }

    public float getTimeLeft()
    {
        if (m_startTime == -1) return -1;
        else return m_startTime - Time.time + _timing;
    }

    public bool isEllapsed()
    {
        return m_startTime!=-1 && Time.time > m_startTime + _timing;
    }

    public void Deny()
    {
        if(!m_finished)
        {
            m_finished = true;
            applyDenyResults();
        }
    }
    #endregion

    #region privateMethods
    private void applyNegativeResults()
    {
        applyResults(m_negativeResults);        
    }

    private void applyPositiveResults()
    {
        applyResults(m_positiveResults);
    }
    private void applyDenyResults()
    {
        applyResults(m_denyResults);
    }

    private void applyResults(List<Result> results)
    {
        foreach(Result r in results)
        {
            switch(r.type)
            {
                case ResultType.TIME_BONUS:
                    FloatParam param = (FloatParam)r.parameters;
                    m_submarine.AddTime(param.value);
                    break;
                case ResultType.IA_BONUS :
                    FloatParam fparam = (FloatParam)r.parameters;
                    IA.Instance.ModifyAppreciation(fparam.value);
                    break;
                case ResultType.SPECIAL:
                    applySpecialResult(r.parameters);
                    break;
                default:
                    Debug.LogError("Unhandled mission result type");
                    break;
            }
        }
    }

    public void checkIfCompleted(string objName, string room)
    {
        if (!m_completeCondition._useObject && m_completeCondition._object == objName && m_completeCondition._room == room)
        {
            m_completed = true;
        }
    }

    public void checkIfCompleted(string objName)
    {
        if (m_completeCondition._useObject && m_completeCondition._object == objName)
        {
            m_completed = true;
        }
    }
    #endregion

    // Methods to override for special cases
    #region virtualMethods 
    protected virtual bool checkSpecialConditions(Params parameters) {return false;}
    protected virtual void applySpecialResult(Params parameters) {}
    #endregion
}
