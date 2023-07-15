using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{

    private TestPlayerScript player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<TestPlayerScript>();
    }

    private void AnimationTrigger(){
        player.AttackOver();
    }
}
