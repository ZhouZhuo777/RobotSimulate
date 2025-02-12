using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class AllSelectRobotData
{
    [SerializeField] private List<SelectRobotData> robotTimeAndStarList = new List<SelectRobotData>();
    [SerializeField] private int playerScore;

    public List<SelectRobotData> RobotTimeAndStarList
    {
        get => robotTimeAndStarList;
        set => robotTimeAndStarList = value;
    }

    public AllSelectRobotData()
    {
    }

    public void AddPlayerScore()
    {
        playerScore += 1;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }
}

[Serializable]
public class SelectRobotData
{
    [SerializeField] private string robotName;
    [SerializeField] private string robotCountry;
    [SerializeField] private int robotLevel;
    [SerializeField] private int robotScore;
    [SerializeField] private int curDayAddScoreNum;
    [SerializeField] private int dailyMaxScore;
    [SerializeField] private int dailyEndTimeStamp;
    [SerializeField] private List<RobotAutoAddScoreTIme> robotAutoAddScoreTimeList = new List<RobotAutoAddScoreTIme>();

    public string RobotName
    {
        get => robotName;
        set => robotName = value;
    }

    public string RobotCountry
    {
        get => robotCountry;
        set => robotCountry = value;
    }

    public int RobotLevel
    {
        get => robotLevel;
        set => robotLevel = value;
    }

    public int RobotScore
    {
        get => robotScore;
        set => robotScore = value;
    }
    public int CurDayAddScoreNum
    {
        get => curDayAddScoreNum;
        set => curDayAddScoreNum = value;
    }
    public int DailyMaxScore
    {
        get => dailyMaxScore;
        set => dailyMaxScore = value;
    }
    
    public int DailyEndTimeStamp
    {
        get => dailyEndTimeStamp;
        set => dailyEndTimeStamp = value;
    }

    public List<RobotAutoAddScoreTIme> RobotAutoAddScoreTimeList
    {
        get => robotAutoAddScoreTimeList;
        set => robotAutoAddScoreTimeList = value;
    }

    // public int GetRobotScore()
    // {
    //     return robotScore + GetCurTimeScoreNum();
    // }
    // public int GetCurTimeScoreNum()
    // {
    //     int scoreNum = 0;
    //     var curTimeStamp = TimeUtil.GetTimeStamp(StarShowdownCycleMgr.Instance.NowTime);
    //     foreach (var timeAndStar in robotTimeAndStarList)
    //     {
    //         if (timeAndStar.TimeStamp <= curTimeStamp)
    //             scoreNum = timeAndStar.StarNum;
    //         else
    //         {
    //             break;
    //         }
    //     }
    //
    //     return scoreNum;
    // }

    public void ResetCurDayAddScoreNum()
    {
        // if(robotLevel == 3) Debug.LogError(curDayAddScoreNum);
        curDayAddScoreNum = 0;
    }

    //玩家排名要实时计算
    public void AutoAddScore()
    {
        foreach (var st in robotAutoAddScoreTimeList)
        {
            var curTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now);
            if (st.TimeStamp > curTimeStamp)
            {
                return;
            }
            if (st.TimeStamp >= dailyEndTimeStamp)
            {
                ResetCurDayAddScoreNum();
                dailyEndTimeStamp += TimeUtil.DAY_TO_SECOND;
            }

            if (!st.IsAdd && st.TimeStamp <= curTimeStamp)
            {
                AddScore();
                st.IsAdd = true;
            }
        }
    }

    public void NormalAddScore()
    {
        var curTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now);
        if (curTimeStamp >= dailyEndTimeStamp)
        {
            dailyEndTimeStamp += TimeUtil.DAY_TO_SECOND;
            ResetCurDayAddScoreNum();
        }
        AddScore();
    }
    
    public void CycleAddScore(int timeStamp)
    {
        var curTimeStamp = timeStamp;
        AutoAddScore(timeStamp);
        if (curTimeStamp >= dailyEndTimeStamp)
        {
            ResetCurDayAddScoreNum();
            dailyEndTimeStamp += TimeUtil.DAY_TO_SECOND;
        }
        AddScore();
    }
    
    public void AutoAddScore(int timeStamp)
    {
        foreach (var st in robotAutoAddScoreTimeList)
        {
            var curTimeStamp = timeStamp;
            if (st.TimeStamp > curTimeStamp)
            {
                return;
            }
            if (st.TimeStamp >= dailyEndTimeStamp)
            {
                ResetCurDayAddScoreNum();
                dailyEndTimeStamp += TimeUtil.DAY_TO_SECOND;
            }

            if (!st.IsAdd && st.TimeStamp <= curTimeStamp)
            {
                AddScore();
                st.IsAdd = true;
            }
        }
    }

    private void AddScore()
    {
        var config = RobotConfigSO.Instance.RobotConfig;
        var robot = config.GetRobot(robotLevel);
        if (curDayAddScoreNum >= dailyMaxScore)
            return;
        var playerRankLevel = RobotRankMgr.Instance.GetSelfRankLevel(true);
        var probability = robot.GetScoringProbability(playerRankLevel);
        var randomValue = Random.value;
        if (robotLevel == 3)
            robot = config.GetRobot(robotLevel);
        if(randomValue > probability) return;
        var getScore = Random.Range(1, robot.OnceGetScoreRange + 1);
        if (getScore + curDayAddScoreNum > dailyMaxScore)
            getScore = dailyMaxScore - curDayAddScoreNum;
        robotScore += getScore;
        curDayAddScoreNum += getScore;
    }
}

[Serializable]
public class RobotAutoAddScoreTIme
{
    [SerializeField] private bool isAdd;
    [SerializeField] private int timeStamp;

    public bool IsAdd
    {
        get => isAdd;
        set => isAdd = value;
    }

    public int TimeStamp => timeStamp;

    public RobotAutoAddScoreTIme(bool _isAdd, int _timeStamp)
    {
        isAdd = _isAdd;
        timeStamp = _timeStamp;
    }
}

public class RobotData
{
    public int ID;
    public string Country;
    public string Name;
}