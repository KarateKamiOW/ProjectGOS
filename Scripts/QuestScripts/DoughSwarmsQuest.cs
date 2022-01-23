using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughSwarmsQuest : QuestDetails
{
    int doughbilesSlain = 0;
    public override void CheckQuestProgress()
    {
        throw new System.NotImplementedException();
    }

    public override string QuestStepDetailed(int step)
    {
        return "Slay " + doughbilesSlain + "/5 Doughbiles";
    }
}
