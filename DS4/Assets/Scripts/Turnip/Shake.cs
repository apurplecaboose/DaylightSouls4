using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Shake : MonoBehaviour
{
    float _Freq = 60f;
    float _Amp, _Dtime;
    public AnimationCurve C;
    public GameObject Particle;
    GameObject _P;
    void Update()
    {
        _Dtime += Time.deltaTime;
        float time = _Dtime / 2.25f;
        _Amp = Mathf.Lerp(0.0005f, 0.0075f, Mathf.Clamp01(C.Evaluate(time)));  
        float x = Mathf.Sin(_Dtime * _Freq) * _Amp;
        float y = Mathf.Cos(_Dtime * _Freq) * _Amp;

        this.transform.position += new Vector3(x, y, 0);
        if (_P != null) return;
        if(_Dtime >= 0.25f)
        {
            _P = Instantiate(Particle,this.transform.parent);
            _P.transform.localPosition = new Vector3(-1.705f, -0.844f, 0);
            Vector3 randomRot = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            _P.transform.up = randomRot;
        }
    }

}
