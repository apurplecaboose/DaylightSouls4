using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenericBossShake : MonoBehaviour
{
    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(GenericBossShake))]
    public class GenericBossShakeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GenericBossShake script = (GenericBossShake)target;
            EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Shake Length In Frames", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 3));
            script._ShakeLengthInFrames = EditorGUILayout.IntField(script._ShakeLengthInFrames, GUILayout.MaxWidth(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Shake Frequency", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 3));
            script._ShakeFrequency = EditorGUILayout.FloatField(script._ShakeFrequency, GUILayout.MaxWidth(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Intensity", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 4f));
            EditorGUILayout.LabelField("Start Intensity", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 8));
            script._StartIntensity = EditorGUILayout.FloatField(script._StartIntensity, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 8));
            EditorGUILayout.LabelField("End Intensity", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 8));
            script._EndIntensity = EditorGUILayout.FloatField(script._EndIntensity, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 8));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Intensity Curve", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 6f));
            script._IntensityCurve = EditorGUILayout.CurveField(script._IntensityCurve, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 1.3f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Start On Awake", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 4));
            script._StartOnAwake = EditorGUILayout.Toggle(script._StartOnAwake, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 4), GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField("Dont Destroy On Time Up", GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 4));
            script._DontDestroyOnTimeUp = EditorGUILayout.Toggle(script._DontDestroyOnTimeUp, GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth / 4), GUILayout.ExpandWidth(false));
            EditorGUILayout.EndHorizontal();
        }
        public static void TightLabel(string labelStr)
        {
            GUIContent label = new GUIContent(labelStr);
            //This is the important bit, we set the width to the calculated width of the content in the GUIStyle of the control
            EditorGUILayout.LabelField(label, GUILayout.Width(GUI.skin.label.CalcSize(label).x));
        }
    }
#endif
    #endregion
    int _ShakeLengthInFrames;
    AnimationCurve _IntensityCurve;
    float _StartIntensity, _EndIntensity, _Amp, _Dtime, _ShakeFrequency = 60f;
    bool _DontDestroyOnTimeUp, _StartOnAwake, _Trigger;

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
            _Amp = Mathf.Lerp(_StartIntensity, _EndIntensity, Mathf.Clamp01(_IntensityCurve.Evaluate(time)));
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
}
