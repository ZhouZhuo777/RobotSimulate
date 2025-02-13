using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RobotConfig
{
    public Robot[] RobotArr;

    public RobotConfig()
    {
        RobotProbability[] robotProbabilityArr0 = new RobotProbability[] { new RobotProbability(1,0.1f), new RobotProbability(4,0f), new RobotProbability(11,0f), };
        RobotProbability[] robotProbabilityArr1 = new RobotProbability[] { new RobotProbability(1,0.2f), new RobotProbability(4,0.1f), new RobotProbability(11,0f), };
        RobotProbability[] robotProbabilityArr2 = new RobotProbability[] { new RobotProbability(1,0.4f), new RobotProbability(4,0.2f), new RobotProbability(11,0.1f), };
        RobotProbability[] robotProbabilityArr3 = new RobotProbability[] { new RobotProbability(1,0.6f), new RobotProbability(4,0.4f), new RobotProbability(11,0.2f), };
        RobotArr = new Robot[]
        {
            new Robot(0, 15, robotProbabilityArr0, 1, new Interval(4, 6), 5),
            new Robot(1, 15, robotProbabilityArr1, 2, new Interval(5, 15), 8),
            new Robot(2, 10, robotProbabilityArr2, 3, new Interval(10, 20), 10),
            new Robot(3, 5, robotProbabilityArr3, 5, new Interval(30, 50), 15),
        };
    }

    public Robot GetRobot(int robotLevel)
    {
        if (robotLevel < 0 || robotLevel > RobotArr.Length)
            return new Robot(robotLevel,15, new RobotProbability[] { new RobotProbability(1,0.1f), new RobotProbability(4,0f), new RobotProbability(10,0f), }, 1, new Interval(4,6), 5);
        return RobotArr[robotLevel];
    }
}

[Serializable]
public class Robot
{
    [SerializeField] private int robotLevel;
    [SerializeField] private int startScoreRange;
    [SerializeField] private RobotProbability[] scoringProbabilityArr;
    [SerializeField] private int onceGetScoreRange;
    [SerializeField] private Interval dailyMaxScoreInterval;
    [SerializeField] private int dailyAutoGetScoreNum;

    public int RobotLevel
    {
        get => robotLevel;
        set => robotLevel = value;
    }

    public int StartScoreRange
    {
        get => startScoreRange;
        set => startScoreRange = value;
    }

    public RobotProbability[] ScoringProbabilityDic
    {
        get => scoringProbabilityArr;
        set => scoringProbabilityArr = value;
    }

    public int OnceGetScoreRange
    {
        get => onceGetScoreRange;
        set => onceGetScoreRange = value;
    }

    public Interval DailyMaxScoreInterval
    {
        get => dailyMaxScoreInterval;
        set => dailyMaxScoreInterval = value;
    }

    public int DailyAutoGetScoreNum
    {
        get => dailyAutoGetScoreNum;
        set => dailyAutoGetScoreNum = value;
    }

    public Robot(int _robotLevel, int _startScoreRange, RobotProbability[] _scoringProbabilityArr,
        int _onceGetScoreRange,
        Interval _dailyMaxScoreInterval, int _dailyAutoGetScoreNum)
    {
        robotLevel = _robotLevel;
        startScoreRange = _startScoreRange;
        scoringProbabilityArr = _scoringProbabilityArr;
        onceGetScoreRange = _onceGetScoreRange;
        dailyMaxScoreInterval = _dailyMaxScoreInterval;
        dailyAutoGetScoreNum = _dailyAutoGetScoreNum;
    }

    public float GetScoringProbability(int playerRankLevel)
    {
        var probability = 0f;
        foreach (var kv in scoringProbabilityArr)
        {
            if (playerRankLevel >= kv.left)
                probability = kv.probability;
        }

        return probability;
    }
}


[Serializable]
public class RobotGearConfig
{
    [SerializeField] private RobotProbability[] gearScoreArr;
    [SerializeField] private RobotGear[] robotGearDicArr;

    public RobotGear[] RobotGearArr
    {
        get => robotGearDicArr;
        set => robotGearDicArr = value;
    }

    public RobotProbability[] GearScoreArr
    {
        get => gearScoreArr;
        set => gearScoreArr = value;
    }

    public RobotGearConfig()
    {
        robotGearDicArr = new RobotGear[]
        {
            new RobotGear(-2, new int[] { 34, 15, 0, 0 }),
            new RobotGear(-1, new int[] { 33, 15, 1, 0 }),
            new RobotGear(-2, new int[] { 32, 15, 2, 0 }),
            new RobotGear(1, new int[] { 31, 15, 2, 1 }),
            new RobotGear(2, new int[] { 31, 15, 1, 2 }),
        };

        GearScoreArr = new RobotProbability[]
        {
            new RobotProbability(1,0.5f),
            new RobotProbability(2,0),
            new RobotProbability(3,0),
            new RobotProbability(4,-1),
            new RobotProbability(11,-1f),
        };
    }
}

[Serializable]
public class Interval
{
    public int left;
    public int right;

    public Interval(int _left,int _right)
    {
        left = _left;
        right = _right;
    }
}

[Serializable]
public class RobotProbability
{
    public int left;
    public float probability;

    public RobotProbability(int _left, float _probability)
    {
        left = _left;
        probability = _probability;
    }
}

[Serializable]
public class RobotGear
{
    public int level;
    public int[] gearArr;

    public RobotGear(int _level, int[] _gearArr)
    {
        level = _level;
        gearArr = _gearArr;
    }
}