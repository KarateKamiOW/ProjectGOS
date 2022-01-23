using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCasterSpellSet : MonoBehaviour
{
    [SerializeField] List<SpellsScriptableObject> rohkanSpells;
    [SerializeField] List<SpellsScriptableObject> paperiousSpells;
    [SerializeField] List<SpellsScriptableObject> scissoraSpells;
    [SerializeField] SpellsScriptableObject blockAsSpell;
    public List<Spells> TotalEnemyListOfSpells = new List<Spells>();
    [SerializeField] Spells RohkanSpell1 { get; set; }
    [SerializeField] Spells RohkanSpell2 { get; set; }
    [SerializeField] Spells PaperiousSpell1 { get; set; }
    [SerializeField] Spells PaperiousSpell2 { get; set; }
    [SerializeField] Spells ScissoraSpell1 { get; set; }
    [SerializeField] Spells ScissoraSpell2 { get; set; }

    [SerializeField] Spells BlockAsSpell { get; set; }

    #region Expose Those Spells!
    public Spells RohkanSpell1Exposed { get { return RohkanSpell1; } }
    public Spells RohkanSpell2Exposed { get { return RohkanSpell2; } }
    public Spells PaperiousSpell1Exposed { get { return PaperiousSpell1; } }
    public Spells PaperiousSpell2Exposed { get { return PaperiousSpell2; } }
    public Spells ScissoraSpell1Exposed { get { return ScissoraSpell1; } }
    public Spells ScissoraSpell2Exposed { get { return ScissoraSpell2; } }
    #endregion

    public void SetSpellData()
    {
        RohkanSpell1 = new Spells(rohkanSpells[0]);
        RohkanSpell2 = new Spells(rohkanSpells[1]);

        PaperiousSpell1 = new Spells(paperiousSpells[0]);
        PaperiousSpell2 = new Spells(paperiousSpells[1]);

        ScissoraSpell1 = new Spells(scissoraSpells[0]);
        ScissoraSpell2 = new Spells(scissoraSpells[1]);

        BlockAsSpell = new Spells(blockAsSpell);

        //Add all elements to a single list, for the BattleSystem to refer to.
        TotalEnemyListOfSpells.Add(new Spells(rohkanSpells[0]));
        TotalEnemyListOfSpells.Add(new Spells(rohkanSpells[1]));
        TotalEnemyListOfSpells.Add(new Spells(paperiousSpells[0]));
        TotalEnemyListOfSpells.Add(new Spells(paperiousSpells[1]));
        TotalEnemyListOfSpells.Add(new Spells(scissoraSpells[0]));
        TotalEnemyListOfSpells.Add(new Spells(scissoraSpells[1]));
        TotalEnemyListOfSpells.Add(new Spells(blockAsSpell));
    }
}
