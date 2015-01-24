using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission : MonoBehaviour
{
    #region publicTypes
    // TODO : define generic cases and handle them
    public enum ResultType
    {
        SPECIAL
    }
    
    public class ResultParams
    {
    }

    public struct Result
    {
        public ResultType type;
        public ResultParams parameters;
    }

    // TODO : define generic conditons and handle them
    public enum CompleteConditionType
    {
        SPECIAL
    }

    public class CompleteConditionParams
    {
    }

    public struct CompleteCondition
    {
        public CompleteConditionType type;
        public CompleteConditionParams parameters;
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
    private List<CompleteCondition> m_completeConditions;
    #endregion

    #region MonoBehaviour
    public virtual void Start()
    {
        m_positiveResults = new List<Result>();
        m_negativeResults = new List<Result>();
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
    public void start()
    {
        m_startTime = Time.time;
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

    private void applyResults(List<Result> results)
    {
        foreach(Result r in results)
        {
            switch(r.type)
            {
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
    protected virtual bool checkSpecialConditions(CompleteConditionParams parameters) {return false;}
    protected virtual void applySpecialResult(ResultParams parameters) {}
    #endregion
}
