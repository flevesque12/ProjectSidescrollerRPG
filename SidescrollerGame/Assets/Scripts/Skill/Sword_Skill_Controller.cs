using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private CircleCollider2D m_CircleCollider;
    private Player m_Player;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
        
    }

    public void SetupSword(Vector2 _direction, float _gravityScale)
    {
        m_Rigidbody.velocity = _direction;
        m_Rigidbody.gravityScale = _gravityScale;
    }
}
