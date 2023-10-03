using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{

    [SerializeField] private int numberOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller currentBackhole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, numberOfAttacks, cloneCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeFinished()
    {

        if (!currentBackhole)
        {
            return false;
        }

        if(currentBackhole.playerCanExitState)
        {
            currentBackhole = null;
            return true;
        }

        return false;
    }
}
