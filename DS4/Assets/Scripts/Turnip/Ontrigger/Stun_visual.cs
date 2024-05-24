using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_visual : MonoBehaviour
{
    float _Freq = 60f;
    float _Amp = 0.005f, _Dtime;

    public int ActiveFrames = 25;
    public float Offset;
    public Transform Target;

    void Update()
    {
        this.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y + Offset, 0); //dumb but it works

        _Dtime += Time.deltaTime;
        float time = _Dtime / 2.25f;
        float x = Mathf.Sin(_Dtime * _Freq) * _Amp;
        float y = Mathf.Cos(_Dtime * _Freq) * _Amp;
        this.transform.position += new Vector3(x, y, 0);
    }
    void FixedUpdate()
    {
        ActiveFrames -= 1;
        if(ActiveFrames <= 0) Destroy(gameObject);
    }
}
