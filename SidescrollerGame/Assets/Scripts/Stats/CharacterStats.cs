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
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("take burn damage " + igniteDamage);

            DecreaseHealthBy(igniteDamage);
            
            if (currentHealth < 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCoolDown;
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

        //here
        _targetStats.TakeDamage(totalDamage);

        MagicalDamage(_targetStats);
    }

    public virtual void MagicalDamage(CharacterStats _targetStats)
    {
        int _fireDmg = fireDamage.GetValue;
        int _iceDmg = iceDamage.GetValue;
        int _lightingDmg = lightingDamage.GetValue;

        int totalMagicDmg = _fireDmg + _iceDmg + _lightingDmg + intelligence.GetValue;
        totalMagicDmg = CheckTargetResistance(_targetStats, totalMagicDmg);

        _targetStats.TakeDamage(totalMagicDmg);

        if (Mathf.Max(_lightingDmg, _fireDmg, _iceDmg) <= 0) { return; }

        bool canApplyIgnite = _fireDmg > _iceDmg && _fireDmg > _lightingDmg;
        bool canApplyChill = _iceDmg > _fireDmg && _iceDmg > _lightingDmg;
        bool canApplyShock = _lightingDmg > _fireDmg && _lightingDmg > _iceDmg;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.5f && _fireDmg > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _iceDmg > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightingDmg > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDmg * 0.2f));
        }

        _targetStats.ApplyAilment(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDmg)
    {
        totalMagicDmg -= _targetStats.magicResistance.GetValue + (_targetStats.intelligence.GetValue * 3);
        totalMagicDmg = Mathf.Clamp(totalMagicDmg, 0, int.MaxValue);
        return totalMagicDmg;
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked) { return; }

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFx(ailmentsDuration);
        }

        if (_chill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;

            float slowPercentage = 0.2f;

            GetComponent<Entity>().SlowEntity(slowPercentage, ailmentsDuration);
            fx.ChillFx(ailmentsDuration);
        }

        if (_shock)
        {
            shockedTimer = ailmentsDuration;
            isShocked = _shock;

            fx.ShockedFx(ailmentsDuration);
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        Debug.Log("Total damage: " + _damage);

        if (currentHealth < 0 && !isDead)
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
        float totalCritPower = (critDamage.GetValue + strength.GetValue) * 0.01f;
        float TotalcritDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(TotalcritDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue + vitality.GetValue * 5;
    }

    private int CheckTargetArmorStat(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isChilled)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue * 0.8f);
        }
        else
        {
            _totalDamage -= _targetStats.armor.GetValue;
        }

        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    private bool TargetAvoidAttackChance(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue + _targetStats.agility.GetValue;

        if (isShocked)
        { totalEvasion += 20; }

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
}