using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRankMgr : Singleton<RobotRankMgr>
{
    private List<RobotRankData> rankDataList = new List<RobotRankData>();

    public List<RobotRankData> GetRankList()
    {
        RefreshRankList();
        return rankDataList;
    }

    public void RefreshRankList()
    {
        rankDataList.Clear();
        var userStarNum = RobotMgr.Instance.GetPlayerSocre;
        rankDataList.Add(new RobotRankData("user", "CN", userStarNum,-1 ,true));

        var allRobotData = RobotMgr.Instance.GetAllSelectRobotData();
        foreach (var robotData in allRobotData.RobotTimeAndStarList)
        {
            rankDataList.Add(new RobotRankData(robotData));
        }

        SortRankDataList();
    }

    private void SortRankDataList()
    {
        rankDataList.Sort((a, b) => b.starNum.CompareTo(a.starNum));
        var count = rankDataList.Count;
        var selfDataIndex = rankDataList.FindIndex(d => d.isSelf);
        var selfData = rankDataList[selfDataIndex];
        for (int i = 0; i < count; i++)
        {
            var curData = rankDataList[i];
            if (curData.starNum == selfData.starNum)
            {
                if (!curData.isSelf)
                {
                    rankDataList[i] = selfData;
                    rankDataList[selfDataIndex] = curData;
                }

                break;
            }
        }
    }

    public int GetSelfRankLevel(bool isRefresh)
    {
        if (isRefresh || rankDataList.Count == 0)
            RefreshRankList();
        var count = rankDataList.Count;
        for (int i = 0; i < count; i++)
        {
            if (rankDataList[i].isSelf)
                return i + 1;
        }

        return -1;
    }

    public (int ,RobotRankData) GetSelfRankData()
    {
        var count = rankDataList.Count;
        for (int i = 0; i < count; i++)
        {
            if (rankDataList[i].isSelf)
                return (i, rankDataList[i]);
        }

        return (-1, null);
    }
}



public class RobotRankData
{
    public string rankName;
    public string rankCountry;
    public int starNum;
    public bool isSelf;
    public int level;

    public RobotRankData(string _rankName, string _rankCountry, int _starNum,int  _level,bool _isSelf = false)
    {
        rankName = _rankName;
        rankCountry = _rankCountry;
        starNum = _starNum;
        level = _level;
        isSelf = _isSelf;
    }

    public RobotRankData(SelectRobotData robotData)
    {
        rankName = robotData.RobotName;
        rankCountry = robotData.RobotCountry;
        starNum = robotData.RobotScore;
        isSelf = false;
        level = robotData.RobotLevel;
    }
}