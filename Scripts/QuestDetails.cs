using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestDetails : MonoBehaviour
{
    public abstract void CheckQuestProgress();

    public abstract string QuestStepDetailed(int step);
    
}
