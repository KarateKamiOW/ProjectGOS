using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestObject : ScriptableObject
{
    public string questTitle;
    public string questGiverName;
    public Sprite questGiverSprite;
    public MainOrSideQuest mainOrSideQuest;
    [TextArea(15, 10)]
    public string task;
    public NumOfSteps numOfSteps;
    public int solcReward;
    public List<ItemObject> itemRewards;
    public QuestDetails questDetails;
    public bool isActive;

}
public enum QuestType 
{ 
    None,
    Collect,
    Slay
}

public enum MainOrSideQuest 
{ 
    Main,
    Side
}

public enum NumOfSteps 
{ 
    _1,
    _2,
    _3
}