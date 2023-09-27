using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;

    private bool canCreateHotkeys = true;
    private bool cloneAttackReleased;
    public int amountOfAttack = 4;
    public float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    //get the list of all enemies that enter the trigger
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotkey = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.R))
        {
            DestroyHotkey();
            cloneAttackReleased = true;
            canCreateHotkeys = false;
        }

        if(cloneAttackTimer < 0 && cloneAttackReleased) {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range( 0, targets.Count );

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

            if( amountOfAttack <= 0 )
            {
                canShrink = true;
                cloneAttackReleased = false;                
            }
        }

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new  Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if(transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
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
