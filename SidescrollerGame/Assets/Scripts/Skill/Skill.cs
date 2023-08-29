using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float cooldownTimer;

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            //use skill
            UseSkill();
            cooldownTimer = coolDown;
            return true;
        }

        return false;
        Debug.Log("Skill on cooldown");
    }

    public virtual void UseSkill()
    {
        //do skill
    }
}
