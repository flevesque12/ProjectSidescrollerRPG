using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallModifierGravity : MonoBehaviour
{
    [SerializeField] private bool m_EnableGravity;
    public Rigidbody2D rb { get; private set; }
     //[Header("Fall modifier info")]
    [SerializeField] private float m_fallModifier = 2.5f;
    [SerializeField] private float m_LowJumpModifier = 2f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FallModifierGravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (m_fallModifier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (m_LowJumpModifier - 1) * Time.deltaTime;
        }
    }
}
