using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public bool IsWebGL;
    [SerializeField] AudioSource _Source1, _Source2;
    void Start()
    {
        if(!IsWebGL)
        {
            double awaketime = AudioSettings.dspTime + 0.5f;
            AudioClip intro = _Source1.clip;
            _Source1.PlayScheduled(awaketime);

            double clipDuration = (double)intro.samples / intro.frequency;
            double startsecondclip = awaketime + clipDuration;
            _Source2.PlayScheduled(startsecondclip);
        }
    }
    float _T, _clipL; bool _startfunc;
    void FixedUpdate()
    {
        if (!_startfunc && IsWebGL)
        {
            _Source1.Play();
            _clipL = _Source1.clip.length;
            _startfunc = true;
        }
        if(IsWebGL)
        {
            _T += Time.fixedDeltaTime;
            if(_T >= _clipL - 0.65f)
            {
                _Source2.Play();
                IsWebGL = false;
            }
        }
    }
}
