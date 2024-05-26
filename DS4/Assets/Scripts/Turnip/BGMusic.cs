using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] AudioSource _Source1, _Source2;
    void Start()
    {
        double awaketime = AudioSettings.dspTime + 0.5f;
        AudioClip intro = _Source1.clip;
        _Source1.PlayScheduled(awaketime);

        double clipDuration = (double)intro.samples / intro.frequency;
        double startsecondclip = awaketime + clipDuration;
        _Source2.PlayScheduled(startsecondclip);
    }

}
