using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct CharacterStats {
    public int strength;
    public int dexterity;
    public int constitution;
    public int wisdom;
    public int intelligence;
    public int charima;
}

[System.Serializable]
public class DieRoll
{
    public int number;
    public int die;

    public int roll()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        int sum = 0;
        for(int i = 0; i < number; i++)
        {
            sum += Random.Range(1, die);
        }

        return sum;
    }
}

public class Character : MonoBehaviour
{
    public string characterName;
    public int level = 1;
    public CharacterStats stats;
    public CharacterStats mods;

    public float maxHitpoint = 10;
    public float maxMana = 10;

    public float hitpoint = 10;
    public float mana = 10;

    public DieRoll attackDie;

    public Slider hitPointSlider;
    public Slider manaSlider;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI lvlText;

    private TextMeshProUGUI manaText;
    private TextMeshProUGUI hpText;

    private Animator animator;

    [FMODUnity.EventRef]
    public string attackEvent;
    [FMODUnity.EventRef]
    public string spellEvent;

    public FMOD.Studio.EventInstance attackAudioState;
    public FMOD.Studio.EventInstance spellAudioState;

    public Spell[] knownSpell;
    public List<StatusEffectSpell> activeStatus;

    public void Awake()
    {
        hitpoint = maxHitpoint;
        mana = maxMana;

        hitPointSlider.maxValue = maxHitpoint;
        hitPointSlider.value = hitpoint;

        manaSlider.maxValue = maxMana;
        manaSlider.value = mana;
        manaText = manaSlider.GetComponentInChildren<TextMeshProUGUI>();
        hpText = hitPointSlider.GetComponentInChildren<TextMeshProUGUI>();
        manaText.SetText("mana: " + mana);
        hpText.SetText("hp: " + hitpoint);

        nameText.SetText(characterName);
        lvlText.SetText("lvl " + level);

        animator = GetComponent<Animator>();

        attackAudioState = FMODUnity.RuntimeManager.CreateInstance(attackEvent);
        spellAudioState = FMODUnity.RuntimeManager.CreateInstance(spellEvent);

        UpdateMods();
    }

    private int getModifier(int val)
    {
        if (val <= 1)
        {
            return -5;
        }
        int mod = (val - 10)/ 2;
        return mod;
    }

    public void UpdateMods()
    {
        mods.strength = getModifier(stats.strength);
        mods.dexterity = getModifier(stats.dexterity);
        mods.constitution = getModifier(stats.constitution);
        mods.wisdom = getModifier(stats.wisdom);
        mods.intelligence = getModifier(stats.intelligence);
        mods.charima = getModifier(stats.charima);
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    public void TriggerSpellAnimation()
    {
        animator.SetTrigger("spell");
    }

    public void PlayAttackAudio()
    {
        attackAudioState.start();
    }

    public int Attack()
    {
        return attackDie.roll();
    }

    public void SetMana(float manaSpent)
    {
        if (mana - manaSpent < 0)
            mana = 0;
        else if (mana - manaSpent > maxMana)
            mana = maxMana;
        else
            mana -= manaSpent;
        manaSlider.value = mana;
        manaText.SetText("mana: " + mana);
    }

    public void TakeDamage(float hp)
    {
        if (hitpoint - hp < 0)
            hitpoint = 0;
        else if (hitpoint - hp > maxHitpoint)
            hitpoint = maxHitpoint;
        else
            hitpoint -= hp;
        hitPointSlider.value = hitpoint;
        hpText.SetText("hp: " + hitpoint);
    }

    public void OnValidate()
    {
        if (hitpoint >= maxHitpoint)
        {
            hitpoint = maxHitpoint;
        }else if (hitpoint <= 0)
        {
            hitpoint = 0;
        }

        if (mana >= maxMana)
        {
            mana = maxMana;
        }
        else if (mana <= 0)
        {
            mana = 0;
        }

        UpdateMods();
    }
}
