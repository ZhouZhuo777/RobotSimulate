using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RobotMgr : Singleton<RobotMgr>
{
    private string localPath = $"{Application.persistentDataPath}/Robot/RobotData.json";

    private AllSelectRobotData allSelectRobotData = new AllSelectRobotData();
    
    public AllSelectRobotData GetAllSelectRobotData()
    {
        if (allSelectRobotData != null && allSelectRobotData.RobotTimeAndStarList.Count != 0)
            return allSelectRobotData;
        Init();
        if (allSelectRobotData != null && allSelectRobotData.RobotTimeAndStarList.Count != 0)
            return allSelectRobotData;
        SelectRobot(3, robotGear);
        return allSelectRobotData;
    }

    public void Init()
    {
        ReadData();
        var RobotTextAsset = Resources.Load<TextAsset>("Configs/RobotData");
        robotDataList = SerializeTools.JsonConvertStringToObj1<List<RobotData>>(RobotTextAsset.text);
    }

    public void SaveAllSelectRobotData()
    {
        FileTools.WriteTextFile(localPath, SerializeTools.JsonUtilityObjToString(allSelectRobotData));
    }

    
    private RobotCheatView robotCheatView;

    public void Initialize(RobotCheatView view)
    {
        robotCheatView = view;
    }
    private IEnumerator DelayedSave()
    {
        yield return new WaitForSeconds(1f);
        SaveAllSelectRobotData();
        isSvaing = false;
    }

    private bool isSvaing = false;
    private void WaitSave()
    {
        if(isSvaing) return;
        isSvaing = true;
        robotCheatView.StartCoroutine(DelayedSave());
    }
    
    private void ReadData()
    {
        if (!File.Exists(localPath))
        {
            // SelectRobot();
            return;
        }

        try
        {
            var str = FileTools.ReadTextFile(localPath);
            allSelectRobotData =
                SerializeTools.JsonUtilityStringToObj<AllSelectRobotData>(str);
        }
        catch (Exception e)
        {
            Debug.LogError($"AllStarShowdownSelectRobotData Read Persistent File Exception ___ {e}");
        }
    }

    private int robotGear = 3;
    public void SelectRobot(int oneCycle,int gear)
    {
        if (robotDataList==null || robotDataList.Count == 0)
            robotDataList = GetRobotListData();
        allSelectRobotData = new AllSelectRobotData();
        var soConfig = RobotConfigSO.Instance;
        var robotConfig = soConfig.RobotConfig;
        var allTypeRobot = robotConfig.RobotArr;
        int allRobotCount = 49;
        int curRobotCount = 0;
        robotGear = gear;
        
        var robotGearConfig = soConfig.RobotGearConfig;
        
        for (int i = 0; i < allTypeRobot.Length; i++)
        {
            var least = robotGearConfig.RobotGearArr[gear].gearArr[i];
            for (int j = 0; j < least; j++)
            {
                allSelectRobotData.RobotTimeAndStarList.Add(GetRobotData(allTypeRobot[i], oneCycle));
                curRobotCount += 1;
            }
        }

        WaitSave();
    }
    
    private SelectRobotData GetRobotData(Robot robotConfig, int oneCycle)
    {
        var selectRobot = new SelectRobotData();
        var robotInfo = GenerateAINameAndCountry();
        selectRobot.RobotLevel = robotConfig.RobotLevel;
        selectRobot.RobotName = robotInfo.Item1;
        selectRobot.RobotCountry = robotInfo.Item2;
        selectRobot.RobotScore = Random.Range(1, robotConfig.StartScoreRange + 1);
        selectRobot.DailyMaxScore =
            Random.Range(robotConfig.DailyMaxScoreInterval.left, robotConfig.DailyMaxScoreInterval.right + 1);
        selectRobot.DailyEndTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddDays(1));

        List<int> allTimeStamp = new List<int>();
        for (int j = 0; j < oneCycle; j++)
        {
            var curDayAddStarNum = robotConfig.DailyAutoGetScoreNum;
            var startTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddDays(j));
            var endTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddDays(j + 1));
            for (int k = 0; k < curDayAddStarNum; k++)
            {
                allTimeStamp.Add(Random.Range(startTimeStamp, endTimeStamp));
            }
        }

        allTimeStamp.Sort();
        foreach (var timeStamp in allTimeStamp)
        {
            selectRobot.RobotAutoAddScoreTimeList.Add(new RobotAutoAddScoreTIme(false, timeStamp));
        }

        return selectRobot;
    }
    private List<RobotData> GetRobotListData()
    {
        var RobotTextAsset = Resources.Load<TextAsset>("RobotData");
        return SerializeTools.JsonConvertStringToObj1<List<RobotData>>(RobotTextAsset.text);
    }
    private List<RobotData> robotDataList = new List<RobotData>();
    private (string, string) GenerateAINameAndCountry()
    {
        string aiName;
        string aiCountry;
        if (null != robotDataList && robotDataList.Count > 0)
        {
            int randomIndex = RandomInt(0, robotDataList.Count);
            aiName = robotDataList[randomIndex].Name;
            aiCountry = robotDataList[randomIndex].Country;
            robotDataList.RemoveAt(randomIndex);
        }
        else
        {
            aiName = RandomInt(1, 10000).ToString();
            aiCountry = "us";
        }

        return (aiName, aiCountry);
    }
    public int RandomInt(int minValue, int maxValue)
    {
        InitRandom();
        return _randomInst.Next(minValue, maxValue);
    }
    private static System.Random _randomInst = null;
    private void InitRandom()
    {
        if (null == _randomInst)
        {
            var dateTimeOffset = DateTimeOffset.UtcNow;
            long unixTimeSeconds = dateTimeOffset.ToUnixTimeSeconds();
            _randomInst = new System.Random((int)unixTimeSeconds);
        }
    }

    public void RobotSimulate(int oneCycle,int gear)
    {
        SelectRobot(oneCycle, gear);
    }
    public int RobotSimulate(int oneCycle, int[] dailyScoreTimes,int gear)
    {
        SelectRobot(oneCycle, gear);
        List<int> allTimeStamp = new List<int>();
        for (int i = 0; i < oneCycle; i++)
        {
            var startTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddDays(i));
            var endTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddDays(i + 1));
            if (i == oneCycle - 1)
                endTimeStamp = TimeUtil.GetTimeStamp(DateTime.Now.AddHours(i * 24 + 18));
  
            for (int k = 0; k < dailyScoreTimes[i]; k++)
            {
                allTimeStamp.Add(Random.Range(startTimeStamp, endTimeStamp));
            }
        }
        allTimeStamp.Sort();
        foreach (var timeStamp in allTimeStamp)
        {
            AddPlayerScore(timeStamp);
        }
        AutoAddRobotScore(TimeUtil.GetTimeStamp(DateTime.Now.AddHours((oneCycle - 1) * 24 + 19)));
        return RobotRankMgr.Instance.GetSelfRankLevel(true);
    }

    public void AddPlayerScore(int timeStamp)
    {
        allSelectRobotData.AddPlayerScore();
        AddScore(timeStamp);
    }

    public int GetPlayerSocre => allSelectRobotData.GetPlayerScore();
    
    public void AddScore(int timeStamp)
    {
        if (allSelectRobotData.RobotTimeAndStarList == null || allSelectRobotData.RobotTimeAndStarList.Count == 0)
        {
            Debug.LogError("没有机器人数据");
            return;
        }

        foreach (var robotData in allSelectRobotData.RobotTimeAndStarList)
        {
            robotData.CycleAddScore(timeStamp);
        }
        WaitSave();
    }

    public void AutoAddRobotScore(int timeStamp)
    {
        foreach (var robotData in allSelectRobotData.RobotTimeAndStarList)
        {
            robotData.AutoAddScore(timeStamp);
        }

        WaitSave();
    }
}


public class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> _lazy = new Lazy<T>(() => new T());
    public static T Instance => _lazy.Value;
        
    protected Singleton()
    {
        Init();
    }

    /// <summary>
    /// 仅仅用来初始化单例成员变量，不要用来做初始化操作【多次在Init里写其他操作又调回本单例，导致循环调用引发程序崩溃】
    /// 建议每个单例初始化操作另写函数
    /// </summary>
    protected virtual void Init()
    {
    }
        
    /// <summary>
    /// 提供一个创建单例接口
    /// </summary>
    public void Touch()
    {
    }
}