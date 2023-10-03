using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool playerCanDisapear = true;

    private bool canCreateHotkeys = true;
    private bool cloneAttackReleased;
    private int amountOfAttack = 4;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    //get the list of all enemies that enter the trigger
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotkey = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    //setup for the skill manager
    public void SetupBlackhole(float _maxSize, float _growSpeed, float _srinkSpeed, int _numberOfAttack, float _cloneAttackCooldown, float _blackholeTimer)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _srinkSpeed;
        amountOfAttack = _numberOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeTimer;
    }

    // Update is called once per frame
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if(targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        
        if (targets.Count < 0)
        {
            Debug.Log("Number of target: "+targets.Count);
            return;
        }

        DestroyHotkey();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            PlayerManager.Instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttack > 0 && targets.Count > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            SkillManager.Instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttack--;

            if (amountOfAttack <= 0)
            {
                Invoke("FinishBackholeAbility", 1f);
            }
        }
        
    }

    private void FinishBackholeAbility()
    {
        DestroyHotkey();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotkey()
    {
        if(createHotkey.Count <= 0) { return; }

        for( int i = 0; i < createHotkey.Count; i++ )
        {
            Destroy(createHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(true);
            CreateHotkey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Enemy>() != null){
            other.GetComponent<Enemy>().FreezeTimer(false);
        }
    }

    //private void OnTriggerExit2D(Collider2D other) => other.GetComponent<Enemy>()?.FreezeTimer(false);

    private void CreateHotkey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0) {
            Debug.LogWarning("not enough hot keys in a key code list");
            return;
        }

        if (!canCreateHotkeys) { return; }

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0f, 2f), Quaternion.identity);
        createHotkey.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Controller>();


        newHotKeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
