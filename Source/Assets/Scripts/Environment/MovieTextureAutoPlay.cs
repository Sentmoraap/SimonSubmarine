using UnityEngine;
using System.Collections;

public class MovieTextureAutoPlay : ActionObject {


    public Material[] _movietextures;

    private int m_current;
    private MovieTexture m_mt;

	public override void Start ()
    {

        m_current = 1;
        m_mt = (MovieTexture)renderer.material.mainTexture;
        m_mt.Play();

	}

    protected override void activateAction()
    {
        base.activateAction();

        if(m_current == _movietextures.Length-1)
        {
            m_current = 0;
        }
        else
        {
            m_current ++;
        }

        renderer.material = _movietextures[m_current];
        m_mt = (MovieTexture)renderer.material.mainTexture;
		if(m_mt != null)
	        m_mt.Play();
        
    }

}
