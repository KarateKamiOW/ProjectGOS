using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedAllyHUD : MonoBehaviour
{
    public SummonedUnitData[] allActiveAllies { get; set; } = new SummonedUnitData[3];
    public SummonedAllyData[] unitInfoListUI;
    GameObject[] allyGO = new GameObject[3];

    public IEnumerator SummonUnit(SummonedUnit summonedUnit)  
    {
        //
        for (int i = 0; i < allActiveAllies.Length; i++) 
        {
            if (allActiveAllies[i] == null) 
            {
                allActiveAllies[i] = new SummonedUnitData(summonedUnit);
                //GameObject allySpellGameObject
                allyGO[i] = Instantiate(allActiveAllies[i].Base.SummonedUnitsGOAAbility, unitInfoListUI[i].SummonedUnitAnim.transform.position, unitInfoListUI[i].SummonedUnitAnim.transform.rotation);
                allActiveAllies[i].CurrentHealth = allActiveAllies[i].Base.MaxpHP;
                unitInfoListUI[i].HPBar.SetHealth(allActiveAllies[i].Base.MaxpHP);
                unitInfoListUI[i].SummonedUnitAnim.runtimeAnimatorController = allActiveAllies[i].Base.UnitAnimator;
                unitInfoListUI[i].HPBarGameObject.SetActive(true);
                break;
            }
            if (i == allActiveAllies.Length - 1) 
            {
                Debug.Log("Field Full!");
                break;
            }
        }
        yield return new WaitForSeconds(.35f);
    }

    public void CheckStatusOfSummonedUnits() 
    {
        Debug.Log("Ally List Count = " + allActiveAllies.Length);
        for (int i = 0; i < allActiveAllies.Length; i++) 
        {
            if (allActiveAllies[i] != null)
            {
                if (allActiveAllies[i].CurrentHealth <= 0)
                {
                    allActiveAllies[i].CurrentHealth = 0;
                    unitInfoListUI[i].SummonedUnitAnim.runtimeAnimatorController = null;
                    unitInfoListUI[i].HPBarGameObject.SetActive(false);
                    allActiveAllies[i] = null;
                    allyGO[i] = null;
                }
            }
        }
    }

    public IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit) 
    {
        for (int i = 0; i < allActiveAllies.Length; i++) 
        {
            if (allActiveAllies[i] != null) 
            {
                yield return allyGO[i].GetComponent<AbstractAllyAbility>().BasicAbility(sourceUnit, targetUnit, unitInfoListUI[i].SummonedUnitAnim);
                //GameObject allySpellGameObject = Instantiate(allActiveAllies[i].Base.SummonedUnitsGOAAbility, unitInfoListUI[i].SummonedUnitAnim.transform.position, unitInfoListUI[i].SummonedUnitAnim.transform.rotation);
                //yield return allySpellGameObject.GetComponent<AbstractAllyAbility>().BasicAbility(sourceUnit, targetUnit, unitInfoListUI[i].SummonedUnitAnim);
                
                yield return new WaitForSeconds(.6f);
            }
        }
    }

    public IEnumerator TakeDamageForAll(int damage) 
    {
        for (int i = 0; i < allActiveAllies.Length; i++) 
        {
            if (allActiveAllies[i] != null) 
            {
                if (allActiveAllies[i].Base.MaxpHP >= 10000)
                {
                    //Ally is assumed to be indestrucbile
                    //Forgoe Damaging
                    //Leaving blank area here for now
                }
                else
                {
                    allActiveAllies[i].CurrentHealth -= damage;
                    if (allActiveAllies[i].CurrentHealth <= 0)
                        allActiveAllies[i].CurrentHealth = 0;
                    yield return unitInfoListUI[i].HPBar.SetHPSmoothened((float)allActiveAllies[i].CurrentHealth / allActiveAllies[i].Base.MaxpHP);
                    if (allActiveAllies[i].CurrentHealth <= 0)
                        ClearAllyPosition(i);
                }
            }
        }
        //CheckStatusOfSummonedUnits();
    }

    public IEnumerator TakeDamageIndividual(int damage, int position) 
    {
        allActiveAllies[position].CurrentHealth -= damage;
        yield return unitInfoListUI[position].HPBar.SetHPSmoothened((float)allActiveAllies[position].CurrentHealth / allActiveAllies[position].Base.MaxpHP);
    }
    void ClearAllyPosition(int position) 
    {
        unitInfoListUI[position].SummonedUnitAnim.runtimeAnimatorController = null;
        unitInfoListUI[position].HPBarGameObject.SetActive(false);
        allActiveAllies[position] = null;
        allyGO[position] = null;
    }

    public void NullifyAll() 
    {
        for (int i = 0; i < allActiveAllies.Length; i++)
        {
            unitInfoListUI[i].SummonedUnitAnim.runtimeAnimatorController = null;
            unitInfoListUI[i].HPBarGameObject.SetActive(false);
            allActiveAllies[i] = null;
            allyGO[i] = null;
        }
    }
}
[System.Serializable]
public class SummonedAllyData
{
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject hpBarGameObject;
    [SerializeField] Animator summonedUnitAnim;
    public SummonedUnitData UnitSummonedSOData {get; set;}

    public HPBar HPBar 
    { get { return hpBar; } }

    public GameObject HPBarGameObject 
    { get { return hpBarGameObject; } }

    public Animator SummonedUnitAnim 
    { get { return summonedUnitAnim; } }


}
