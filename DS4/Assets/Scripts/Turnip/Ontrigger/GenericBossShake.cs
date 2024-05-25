using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBossShake : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Shake value will be VERY small start around 0.0005f")]
    [SerializeField] float _StartIntensity, _EndIntensity;
    [SerializeField] float _ShakeFrequency = 60f;
    float _Amp, _Dtime;
    public AnimationCurve IntensityCurve;
    bool _Trigger;
    [Tooltip("Makes it possible to trigger on awake without the need of event")]
    public bool StartOnAwake;
    void Awake()
    {
        if (StartOnAwake) StartShake();
    }
    void Update()
    {   
        if(_Trigger) Shake();
    }
    void Shake()
    {
        _Dtime += Time.deltaTime;
        float time = _Dtime / 2.25f;
        _Amp = Mathf.Lerp(_StartIntensity, _EndIntensity, Mathf.Clamp01(IntensityCurve.Evaluate(time)));
        float x = Mathf.Sin(_Dtime * _ShakeFrequency) * _Amp;
        float y = Mathf.Cos(_Dtime * _ShakeFrequency) * _Amp;

        this.transform.position += new Vector3(x, y, 0);
    }
    /// <summary>
    /// Trigger with event
    /// </summary>
    public void StartShake()
    {
        _Trigger = true;
    }
}
