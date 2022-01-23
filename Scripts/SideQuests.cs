using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create New Quest", menuName = "Quests/Side Quest")]
public class SideQuests : QuestObject
{
    private void Awake()
    {
        mainOrSideQuest = MainOrSideQuest.Side;
    }
}
