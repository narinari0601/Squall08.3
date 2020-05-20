using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageData
{
    private static List<GameObject> stageList = new List<GameObject>();

    private static int stageNum = 0;

    private static string[] ranks = { " ☆ ☆ ☆ ", " ★ ☆ ☆ ", " ★ ★ ☆ ", " ★ ★ ★ " };

    private static Dictionary<int, bool> stagePlayFlags = new Dictionary<int, bool>();

    private static Dictionary<int, float> highScores = new Dictionary<int, float>();

    private static bool isDataInitialize = false;

    public static List<GameObject> StageList { get => stageList; set => stageList = value; }
    public static int StageNum { get => stageNum; set => stageNum = value; }
    public static string[] Ranks { get => ranks; }
    public static Dictionary<int, bool> StagePlayFlags { get => stagePlayFlags; set => stagePlayFlags = value; }
    public static Dictionary<int, float> HighScores { get => highScores; set => highScores = value; }

    public static void DataInitialize()
    {
        if (isDataInitialize)
            return;

        for (int i = 0; i < stageList.Count; i++)
        {
            stagePlayFlags[i] = false;
            highScores[i] = 0;
        }

        stagePlayFlags[0] = true;

        isDataInitialize = true;
    }

    public static void SetStagePrefab(GameObject[] stages)
    {
        if (stageList.Count == 0)
        {
            for (int i = 0; i < stages.Length; i++)
            {
                stageList.Add(stages[i]);
            }
        }
    }

    public static int releasedStageValue()
    {
        int value = 0;

        for (int i = 0; i < stageList.Count; i++)
        {
            if (stagePlayFlags[i])
            {
                value++;
            }
        }

        return value;
    }

    public static void StageClear(int num)
    {
        if (num > stageList.Count)
            return;

        stagePlayFlags[num] = true;
    }

    public static string StageRank(int stageNum)
    {
        var stage = stageList[stageNum].GetComponent<Stage>();
        var highScore = highScores[stageNum];
        var threeStarsScore = stage.ThreeStarsScore;
        var twoStartsScore = stage.TwoStarsScore;

        if (highScore >= threeStarsScore)
        {
            return ranks[3];
        }

        else if (highScore >= twoStartsScore)
        {
            return ranks[2];
        }

        else if (highScore < twoStartsScore && highScore > 0)
        {
            return ranks[1];
        }

        return ranks[0];
    }

    public static void ReleaseAllStage()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            StageClear(i);
        }
    }
}
