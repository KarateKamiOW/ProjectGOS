using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaster : MonoBehaviour
{
    [SerializeField] CastersScriptableObject playerCaster;
    [SerializeField] List<SpellsScriptableObject> rohkanSpells;
    [SerializeField] List<SpellsScriptableObject> paperiousSpells;
    [SerializeField] List<SpellsScriptableObject> scissoraSpells;
    [SerializeField] SpellsScriptableObject blockAsSpell;
    public Animator overworldAnim;
    public List<Spells> totalListOfSpells = new List<Spells>();
    public List<Spells> TotalListOfSpells { get { return totalListOfSpells; } set { totalListOfSpells = value; } }
    public Spells RohkanSpell1 { get; set; }
    public Spells RohkanSpell2 { get; set; }
    public Spells PaperiousSpell1 { get; set; }
    public Spells PaperiousSpell2 { get; set; }
    public Spells ScissoraSpell1 { get; set; }
    public Spells ScissoraSpell2 { get; set; }

    public Spells BlockAsSpell { get; set; }

    #region Expose Those Spells!
    public Spells RohkanSpell1Exposed { get { return RohkanSpell1; } }
    public Spells RohkanSpell2Exposed { get { return RohkanSpell2; } }
    public Spells PaperiousSpell1Exposed { get { return PaperiousSpell1; } }
    public Spells PaperiousSpell2Exposed { get { return PaperiousSpell2; } }
    public Spells ScissoraSpell1Exposed { get { return ScissoraSpell1; } }
    public Spells ScissoraSpell2Exposed { get { return ScissoraSpell2; } }

    public Spells BlockAsSpellExposed { get { return BlockAsSpell; } }
    #endregion

    public void Start()
    {
        SetSpellData();

        if (PlayerPrefs.HasKey("PlayerCaster"))
            Debug.Log("Must delete each session");

        //overworldAnim.runtimeAnimatorController = playerCaster.OverworldAnimator;
    }

    public void SetSpellData()
    {
        RohkanSpell1 = new Spells(rohkanSpells[0]);
        RohkanSpell2 = new Spells(rohkanSpells[1]);

        PaperiousSpell1 = new Spells(paperiousSpells[0]);
        PaperiousSpell2 = new Spells(paperiousSpells[1]);

        ScissoraSpell1 = new Spells(scissoraSpells[0]);
        ScissoraSpell2 = new Spells(scissoraSpells[1]);

        BlockAsSpell = new Spells(blockAsSpell);
    }

    public void SetSpellDataForTotalList()
    {
        RohkanSpell1 = new Spells(rohkanSpells[0]);
        RohkanSpell2 = new Spells(rohkanSpells[1]);

        PaperiousSpell1 = new Spells(paperiousSpells[0]);
        PaperiousSpell2 = new Spells(paperiousSpells[1]);

        ScissoraSpell1 = new Spells(scissoraSpells[0]);
        ScissoraSpell2 = new Spells(scissoraSpells[1]);

        BlockAsSpell = new Spells(blockAsSpell);

        TotalListOfSpells.Add(new Spells(rohkanSpells[0]));
        TotalListOfSpells.Add(new Spells(rohkanSpells[1]));
        TotalListOfSpells.Add(new Spells(paperiousSpells[0]));
        TotalListOfSpells.Add(new Spells(paperiousSpells[1]));
        TotalListOfSpells.Add(new Spells(scissoraSpells[0]));
        TotalListOfSpells.Add(new Spells(scissoraSpells[1]));
        TotalListOfSpells.Add(new Spells(blockAsSpell));

    }

    public void SavePlayerData() 
    {
        PlayerPrefs.SetString("PlayerCaster", playerCaster.CasterName);

        PlayerPrefs.SetString("RohkanSpell1", RohkanSpell1.Base.SpellName);
        PlayerPrefs.SetString("RohkanSpell2", RohkanSpell2.Base.SpellName);
        PlayerPrefs.SetString("PaperiousSpell1", PaperiousSpell1.Base.SpellName);
        PlayerPrefs.SetString("PaperiousSpell2", PaperiousSpell2.Base.SpellName);
        PlayerPrefs.SetString("ScissoraSpell1", ScissoraSpell1.Base.SpellName);
        PlayerPrefs.SetString("ScissoraSpell2", ScissoraSpell2.Base.SpellName);

        Debug.Log("Player Data Saved!");
    }

    public void OnApplicationQuit()
    {
        ClearAllUserData();
    }

    void ClearAllUserData() 
    {
        //For Now, all user data will reset from memory each time you quit the application
        //PlayerPrefs.DeleteKey("PlayerCaster");
        PlayerPrefs.DeleteAll();
    }




}
