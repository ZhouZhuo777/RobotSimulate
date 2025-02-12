using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotCheatItem : MonoBehaviour
{
    [SerializeField] private Text indexText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text robotLevelText;
    [SerializeField] private Text starNumText;

    public void Init(int index, string name, int starNum, bool isSelf, int robotLevel = -1)
    {
        indexText.text = $"{index + 1}";
        nameText.text = name;
        starNumText.text = $"{starNum}";
        if (robotLevel >= 0) robotLevelText.text = $"{robotLevel}";
        else robotLevelText.text = "-";
        if (isSelf)
            indexText.color = Color.red;
        else
            indexText.color = Color.white;
    }
}
