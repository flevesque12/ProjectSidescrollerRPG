using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float returnSpeed;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private CircleCollider2D m_CircleCollider;
    private Player m_Player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;

    //Pierce info
    private int pierceAmount;

    //Bounce Info
    private float bounceSpeed;
    private bool isBouncing;
    private int numberOfBounce;
    private List<Transform> enemyTarget;
    private int targetIndex;

    //Spin info
    private float maxTravelDistance;

    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
    }

    private void DestroySword()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        m_Player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        m_Rigidbody.velocity = _direction;
        m_Rigidbody.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            m_Animator.SetBool("Rotate", true);

        spinDirection = Mathf.Clamp(m_Rigidbody.velocity.x, -1f, 1f);

        Invoke("DestroySword", 7f);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        numberOfBounce = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
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
        if (canRotate)
        {
            transform.right = m_Rigidbody.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, m_Player.transform.position) < 1f)
            {
                m_Player.CatchTheSword();
            }
        }

        BounceLogic();

        SpinLogic();
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(m_Player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                //move a little bit to the enemy
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            //between target
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                //go next target
                targetIndex++;
                numberOfBounce--;

                if (numberOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                //reset
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetForBounce(collision);

        StuckIntoTarget(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckIntoTarget(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;

        m_CircleCollider.enabled = false;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        m_Animator.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}