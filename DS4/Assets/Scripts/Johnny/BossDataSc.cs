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
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, Z2, Z3, Z4, Z5, Z6
    }
}
