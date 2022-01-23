using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create New Quest", menuName = "Quests/MainQuest")]
public class MainQuests : QuestObject
{
    private void Awake()
    {
        mainOrSideQuest = MainOrSideQuest.Main;
    }
}
