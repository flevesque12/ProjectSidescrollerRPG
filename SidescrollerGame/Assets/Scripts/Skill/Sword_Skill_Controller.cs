using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private CircleCollider2D m_CircleCollider;
    private Player m_Player;

    private bool canRotate = true;
    private bool isReturning;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
        
    }

    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player)
    {
        m_Player = _player;
        m_Rigidbody.velocity = _direction;
        m_Rigidbody.gravityScale = _gravityScale;
        m_Animator.SetBool("Rotate", true);
    }

    public void ReturnSword()
    {
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //m_Rigidbody.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if(canRotate)
        {
            transform.right = m_Rigidbody.velocity;
            
        } 
        
        if(isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position, returnSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, m_Player.transform.position) < 1f)
            {
                m_Player.CatchTheSword();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isReturning)
            return;

        m_Animator.SetBool("Rotate", false);
        canRotate = false;
        
        m_CircleCollider.enabled = false;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
