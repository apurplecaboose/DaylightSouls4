using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBossShake : MonoBehaviour
{
    [SerializeField]int _ShakeLengthInFrames;
    public AnimationCurve IntensityCurve;
    [SerializeField] float _StartIntensity, _EndIntensity, _Amp, _ShakeFrequency = 60f;
    [SerializeField] bool _DontDestroyOnTimeUp, _StartOnAwake, _Trigger;
    //bool _Trigger;
    float _Dtime;

    void Awake()
    {
        if (_StartOnAwake) StartShake();
    }
    void Update()
    {
        if (_Trigger)
        {
            _Dtime += Time.deltaTime;
            float time = _Dtime / TicksToSeconds(_ShakeLengthInFrames);
            if (time >= 1 && !_DontDestroyOnTimeUp)
            {
                Destroy(this); //shake is finishied destroy script
                return;
            }
            _Amp = Mathf.Lerp(_StartIntensity, _EndIntensity, Mathf.Clamp01(IntensityCurve.Evaluate(time)));
            float x = Mathf.Sin(_Dtime * _ShakeFrequency) * _Amp;
            float y = Mathf.Cos(_Dtime * _ShakeFrequency) * _Amp;

            this.transform.position += new Vector3(x, y, 0);

            float TicksToSeconds(int ticks) // Don't Touch
            {
                float tickrate = 1f / 60f; // Assuming 60 fps
                float seconds = ticks * tickrate;
                return seconds;
            }
        }
    }
    public void StartShake()
    {
        _Trigger = true;
    }
    public void StopShake()
    {
        _Trigger = false;
    }
}
