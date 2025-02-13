using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RobotCheatView : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text cycleText;
    [SerializeField] private Text testTimeText;
    [SerializeField] private Text gearText;
    [SerializeField] private Button playBtn;
    [SerializeField] private Text curTestTime;
    [SerializeField] private Button add1Btn;
    [SerializeField] private Button play1Btn;

    [SerializeField] private GameObject robotPanel;
    [SerializeField] private GameObject robotCheatItem;
    [SerializeField] private Transform rankBg;
    [SerializeField] private Transform itemContent;

    protected void Awake()
    {
        playBtn.onClick.AddListener(OnBtnPlayClicked);
        play1Btn.onClick.AddListener(OnBtnPlay1Clicked);
        add1Btn.onClick.AddListener(OnBtnAdd1Clicked);
    }

    protected void Start()
    {
        robotPanel.UpdateActive(false);
        RobotMgr.Instance.Initialize(this);
    }

    protected void OnClose()
    {
    }

    private void OnBtnAdd1Clicked()
    {
        RobotConfigSO.Instance.RefreshInstance();
        RobotMgr.Instance.AddPlayerScore(TimeUtil.GetTimeStamp(DateTime.Now));
        robotPanel.UpdateActive(true);
        RefreshRankList();
    }
    private void RefreshRankList()
    {
        var CurDataList = RobotRankMgr.Instance.GetRankList();
        var robotGOList = transform.GetComponentsInChildren<RobotCheatItem>();
        var GOCount = robotGOList.Length;
        var dataCount = CurDataList.Count;
        for (int i = 0; i < dataCount; i++)
        {
            var data = CurDataList[i];
            if (i < GOCount)
            {
                robotGOList[i].Init(i, data.rankName, data.starNum, data.isSelf,data.level);
            }
            else
            {
                var robot = Instantiate(robotCheatItem, itemContent).GetComponent<RobotCheatItem>();
                robot.Init(i, data.rankName, data.starNum, data.isSelf,data.level);
            }
        }
    }

    private int testTime = 1;
    private int cycle = 3;
    // private int playerScoreTime = 10;
    private int gear = 3;
    private int[] playerScoreTimeArr;

    private int playerAverageRank = 0;
    private int playerRank1 = 0;
    private void OnBtnPlayClicked()
    {
        curTestTime.text = "当前0";
        RobotConfigSO.Instance.RefreshInstance();
        // int.TryParse(playerScoreText.text, out playerScoreTime);
        int.TryParse(cycleText.text, out cycle);
        int.TryParse(testTimeText.text, out testTime);
        int.TryParse(gearText.text, out gear);

        var playerScoreTimeArrStr = playerScoreText.text.Split('-');
     
        try
        {
            playerScoreTimeArr = Array.ConvertAll(playerScoreTimeArrStr, int.Parse);
        }
        catch (Exception e)
        {
            Debug.LogError("输入的玩家得分格式不对");
            return;
        }
      
        
        playerAverageRank = 0;
        playerRank1 = 0;
        if (testTime <= 0) testTime = 1;
        if (cycle <= 0) cycle = 3;
        // if (playerScoreTime <= 0) playerScoreTime = 10;
        if (gear < 0 || gear > 4) gear = 3;
        if (playerScoreTimeArrStr.Length != cycle)
        {
            Debug.LogError("玩家得分数量和活动周期对不上");
            return;
        }
        StartCoroutine(TestTimeCycle());
    }

    private void OnBtnPlay1Clicked()
    {
        RobotMgr.Instance.RobotSimulate(cycle, gear);
        robotPanel.UpdateActive(true);
        RefreshRankList();
    }

    private Dictionary<int, int> rankLevelProbabilityDic = new Dictionary<int, int>();
    IEnumerator TestTimeCycle()
    {
        ResetDic();
        SetViewClickable(false);
        for (int i = 0; i < testTime; i++)
        {
            curTestTime.text = $"当前{i + 1}";
            var level = RobotMgr.Instance.RobotSimulate(cycle, playerScoreTimeArr, gear);
           // Debug.LogError(level);
           Add2Dic(level);
           if (level == 1) playerRank1 += 1;
           playerAverageRank += level;
           yield return null;
            // yield return new WaitForSeconds(0.01f);
            robotPanel.UpdateActive(true);
            RefreshRankList();
        }
        Debug.LogError($"平均排名 {(float)playerAverageRank / testTime}");
        StringBuilder stringBuilder = new StringBuilder();
        // string str = "各个名次的概率";
        stringBuilder.Append("各个名次的概率");
        foreach (var levelKV in rankLevelProbabilityDic)
        {
            if (levelKV.Value != 0)
            {
                var probability = $"{((float)(levelKV.Value * 100) / testTime).ToString("0.##")}%";
                stringBuilder.Append($"\n{levelKV.Key} : {probability}");
            }
        }
        stringBuilder.Append("\n其他名次概率为0");
        Debug.LogError(stringBuilder);
        // Debug.LogError($"排名第一的概率 {((float)(playerRank1 * 100) / testTime).ToString("0.##")}%");
        yield return null;
        SetViewClickable(true);
    }

    private void ResetDic()
    {
        rankLevelProbabilityDic.Clear();
        for (int i = 1; i <= 50; i++)
        {
            rankLevelProbabilityDic[i] = 0;
        }
    }

    private void Add2Dic(int level)
    {
        if (!rankLevelProbabilityDic.ContainsKey(level))
            rankLevelProbabilityDic[level] = 0;
        else
            rankLevelProbabilityDic[level] += 1;
    }

    private void SetViewClickable(bool isClickable)
    {
        cg.blocksRaycasts = isClickable;
        cg.interactable = isClickable;
    }
}