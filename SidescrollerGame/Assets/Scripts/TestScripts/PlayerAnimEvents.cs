using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts
{
    public class PlayerAnimEvents : MonoBehaviour
    {

        private TestPlayerScript player;
        void Start()
        {
            player = GetComponentInParent<TestPlayerScript>();
        }

        private void AnimationTrigger(){
            player.AttackOver();
        }
    }
}
