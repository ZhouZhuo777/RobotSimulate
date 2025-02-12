using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotConfigSO", menuName = "Custom/RobotConfigSO", order = 3)]
public class RobotConfigSO : ScriptableObject
{
    public RobotConfig RobotConfig;
    public RobotGearConfig RobotGearConfig;

    private static RobotConfigSO _instance;

    public static RobotConfigSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<RobotConfigSO>("RobotConfigSO");
            }

            return _instance;
        }
    }

    public void RefreshInstance()
    {
        _instance = Resources.Load<RobotConfigSO>("RobotConfigSO");
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }
}