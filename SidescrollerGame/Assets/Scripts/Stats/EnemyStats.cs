using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDropSystem;

    [Header("Level detail")]
    [SerializeField] private int level = 1;

    //each level have a modifier percentage between 0% and 100% that give it to stats
    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
        itemDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        //apply modify to stats
        ModifyStatPerLevel(strength);
        ModifyStatPerLevel(agility);
        ModifyStatPerLevel(intelligence);
        ModifyStatPerLevel(vitality);


        ModifyStatPerLevel(damage);
        ModifyStatPerLevel(critChance);
        ModifyStatPerLevel(critDamage);

        ModifyStatPerLevel(maxHealth);
        ModifyStatPerLevel(armor);
        ModifyStatPerLevel(evasion);
        ModifyStatPerLevel(magicResistance);

        ModifyStatPerLevel(fireDamage);
        ModifyStatPerLevel(iceDamage);
        ModifyStatPerLevel(lightingDamage);
    }

    private void ModifyStatPerLevel(Stat _stat)
    {
        for (int i = 0; i < level; i++)
        {
            //each level add the percentage modify for each level
            float modifierValue = _stat.GetValue * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifierValue));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        itemDropSystem.GenerateDrop();
    }
}
