using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeTest : MonoBehaviour
{
    void Start()
    {
        float invokeTimeCount = 0;
        invokeTimeCount += TicksToSeconds(10); // time you want to wait before invoking
        Invoke("Printer1", invokeTimeCount);// Attk 1
        invokeTimeCount += TicksToSeconds(60);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(50, 100)); // length of gap
        Invoke("Printer2", invokeTimeCount);// Attk 2
        invokeTimeCount += TicksToSeconds(60);//length of combo
        invokeTimeCount += TicksToSeconds(UnityEngine.Random.Range(20, 30)); // length of gap
        Invoke("Printer3", invokeTimeCount);// Attk 3
    }
    float TicksToSeconds(int ticks)
    {
        float tickrate = 1f / 60f; // Assuming 60 fps
        float seconds = ticks * tickrate;
        return seconds;
    }
    void Printer1()
    {
        print("Invoke 1. Time: " + Time.time);
    }
    void Printer2()
    {
        print("Invoke 2. Time: " + Time.time);
    }
    void Printer3()
    {
        print("Invoke 3. Time: " + Time.time);
    }
}
