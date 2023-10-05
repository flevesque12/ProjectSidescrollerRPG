using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Main stats")]
    public Stat strength;       // 1 pt increase dmg by 1 and crit pow by 1
    public Stat agility;        // 1 pt increase evasion(eva) by 1% and crit chance by 1%
    public Stat intelligence;   // 1 pt increase mag dmg by 1 and mag res by 3
    public Stat vitality;       // 1 pt increase health by 3 or 5 points

    [Header("Offense stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critDamage;     //default value 150%

    [Header("Defense stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;      //dmg over time
    public bool isChilled;      //armor -20%
    public bool isShocked;      //acc -20%

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCoolDown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public int currentHealth;

    public System.Action onHealthChanged;
    protected bool isDead;

    // Start is called before the first frame update
    protected virtual void Start()
    { 
        critDamage.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue;
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.time;
        chilledTimer -= Time.time;
        shockedTimer -= Time.time;

        igniteDamageTimer -= Time.time;

        if(ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if(chilledTimer < 0)
        {
            isChilled = false;
        }

        if(shockedTimer < 0)
        {
            isShocked = false;
        }

        if(isIgnited)
        {

        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetAvoidAttackChance(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue + strength.GetValue;

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmorStat(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        if(currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue + agility.GetValue;

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critDamage.GetValue + strength.GetValue) * .01f;
        float TotalcritDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(TotalcritDamage);
    }


    private int CheckTargetArmorStat(CharacterStats _targetStats, int _totalDamage)
    {
        //apply armor stat
        _totalDamage -= _targetStats.armor.GetValue;
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    private bool TargetAvoidAttackChance(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue + _targetStats.agility.GetValue;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
}
