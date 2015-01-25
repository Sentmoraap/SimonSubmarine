using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmbiantSoundPlayer : MonoBehaviour
{
    #region constants
    private const int NB_REPEAT_LOOP = 3;
    #endregion

    #region public members
    public List<AudioClip> _musicLoops;
    public static AmbiantSoundPlayer instance;
    #endregion

    #region private members
    private int m_currentLoopRepeat = 0;
    private int m_currentLoopIndex = 0;
    private AudioSource m_audioSource;
    #endregion

    #region Mono
    void Start ()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_audioSource.clip = _musicLoops[0];
	}
	
	void Update ()
    {
        if(!m_audioSource.isPlaying)
        {
            m_currentLoopRepeat++;
            if(m_currentLoopRepeat>=NB_REPEAT_LOOP)
            {
                m_currentLoopRepeat = 0;
                m_currentLoopIndex = (m_currentLoopIndex + 1) % _musicLoops.Count;
            }
            m_audioSource.clip = _musicLoops[m_currentLoopIndex];
            m_audioSource.Play();
        }
    }
    #endregion

}
