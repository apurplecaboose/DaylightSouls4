using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_visual : MonoBehaviour
{
    float _Freq = 20f;
    float _Amp = 0.075f;

    public int ActiveFrames = 25;
    public float Offset;
    public Transform Target;

    void Update()
    {
        this.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y + Offset, 0); //dumb but it works
        float x = Mathf.Sin(Time.time * _Freq) * _Amp;
        float y = Mathf.Cos(Time.time * _Freq) * _Amp;
        this.transform.position += new Vector3(x, y, 0);
    }
    void FixedUpdate()
    {
        ActiveFrames -= 1;
        if(ActiveFrames <= 0) Destroy(gameObject);
    }
}
