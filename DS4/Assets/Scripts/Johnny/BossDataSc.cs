using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "BossAsset")]
public class BossDataSc : ScriptableObject
{
    public List<float> possibility = new List<float>();
    public List<ComboType> comboTypes = new List<ComboType>();

    public enum ComboType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
        I = 8,
        J = 9,
        K = 10,
        L = 11,
        M = 12,
        N = 13,
        O = 14,
        P = 15,
        Q = 16,
        R = 17,
        S = 18,
        T = 19,
        U = 20,
        V = 21,
        W = 22,
        X = 23,
        Y = 24,
        Z = 25,

        B1__Slam_Attack = 0,
        B1__Punch_Attack = 1,
        B1__Death_by_Covid = 2,

        B2__Runny_nose = 0,
        B2__Headache = 1,
        B2__Death_by_Covid = 2

    }
    // convert combo name into its relative boss combo scriptable object
}
