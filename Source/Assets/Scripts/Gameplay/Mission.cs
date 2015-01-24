using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission : MonoBehaviour
{
    #region publicTypes
    // TODO : define generic cases and handle them
    public enum ResultType
    {
        TIME_BONUS,SPECIAL
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

    // TODO : define generic conditons and handle them
    public enum CompleteConditionType
    {
        SPECIAL
    }

    public struct CompleteCondition
    {
        public CompleteConditionType type;
        public Params parameters;
    }
    #endregion

    #region publicAttributes
    public float _timing;
    #endregion

    #region privateAttributes
    private float m_startTime=-1;
    private bool m_finished = false;
    private List<Result> m_positiveResults;
    private List<Result> m_negativeResults;
    private List<Result> m_denyResults;
    private List<CompleteCondition> m_completeConditions;
    private Submarine m_submarine;
    #endregion

    #region MonoBehaviour
    public virtual void Start()
    {
        m_startTime = Time.time;
        m_positiveResults = new List<Result>();
        m_negativeResults = new List<Result>();
        m_denyResults = new List<Result>();
        m_completeConditions = new List<CompleteCondition>();
    }

    public virtual void Update()
    {
        if(!m_finished)
        {
            if(isEllapsed())
            {
                m_finished = true;
                applyNegativeResults();
            }
            else if(checkIfCompleted())
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
                case ResultType.SPECIAL:
                    applySpecialResult(r.parameters);
                    break;
                default:
                    Debug.LogError("Unhandled mission result type");
                    break;
            }
        }
    }

    private bool checkIfCompleted()
    {
        bool ret = true;
        foreach(CompleteCondition c in m_completeConditions)
        {
            switch(c.type)
            {
                case CompleteConditionType.SPECIAL:
                    ret &= checkSpecialConditions(c.parameters);
                    break;
                default:
                    Debug.LogError("Unhandled mission condition type");
                    break;
            }
        }
        return ret;
    }
    #endregion

    // Methods to override for special cases
    #region virtualMethods 
    protected virtual bool checkSpecialConditions(Params parameters) {return false;}
    protected virtual void applySpecialResult(Params parameters) {}
    #endregion
}
