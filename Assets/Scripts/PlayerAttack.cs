using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameBattleManager manage_battle_cs;
    [SerializeField] private Image square_battle;
    [SerializeField] private GameObject Sword;
    [SerializeField] private SpriteRenderer pngSword;

    [Header("Arrow")]
    [SerializeField]
    private GameObject arrow_prefab;
    [SerializeField] private Transform bow_transform;
    private float Arrow_velocity = 10;

    [SerializeField] private GameObject imgHealEmpty;
    [SerializeField] private GameObject imgHeal;

    int timeEvent = 0;
    int timeToFire = 1;
    float last_time_fire = 0.0f;
    float TimeSword = 5.0f;
    float TimeSpell = 5.0f;
    float timeEvents = 0.0f;
    int countSpace = 0;
    int rotSwordAxeZ = -50;
    int resultAttackSword = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Attack animation and damage
    public void AttackSuccess(int choice)
    {
        switch (choice)
        {
            case 0:
                GameObject Arrow = Instantiate(arrow_prefab, bow_transform.position, bow_transform.rotation);
                Arrow.GetComponent<Rigidbody2D>().velocity = bow_transform.right * Arrow_velocity;
                Destroy(Arrow, 5);
                break;
            case 1:
                GameObject tmp2 = manage_battle_cs.GetInfoPlayer();
                tmp2.GetComponent<PlayerController>().HealPlayer(5);
                break;
            case 2:
                GameObject[] tmp = manage_battle_cs.GetListEnemy();
                for (int i = 0; i < resultAttackSword; i++)
                {
                    if (i < tmp.Length)
                    {
                        tmp[i].GetComponent<EnemyController>().TakeDamage(2 + resultAttackSword - i);
                    }
                }
                break;
        }

    }

    //Function for Check What attack a play has shoose
    public int ChoiceMenu(int choice)
    {
        int attackComplet = 0;
        switch (choice)
        {
            case 0:
                attackComplet = AttackBow();
                break;
            case 1:
                attackComplet = SpellHeal();
                break;
            case 2:
                attackComplet = AttackSword();
                break;
        }
        return attackComplet;
    }


    public int AttackBow()
    {
        square_battle.gameObject.SetActive(true);
        if (timeEvent == 0)
        {
            square_battle.color = Color.red;
        }
        if (Time.realtimeSinceStartup - last_time_fire > timeToFire)
        {
            last_time_fire = Time.realtimeSinceStartup;
            ++timeEvent;
        }
        else
        {
            if (timeEvent == 1)
                square_battle.color = Color.yellow;
            if (timeEvent >= 2.0f && timeEvent < 3.0f)
            {
                square_battle.color = Color.green;
                if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
                {

                    timeEvent = 0;
                    square_battle.gameObject.SetActive(false);

                    return 1;
                    //_play = false;
                }
            }
            else
            {
                if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
                {
                    square_battle.color = Color.red;
                    timeEvent = 0;
                    return 2;
                    // _play = false;
                }

            }
            if (timeEvent == 3.0f)
            {
                square_battle.color = Color.red;
                timeEvent = 0;
                return 2;
                //_play = false;
            }
        }
        return 0;
    }

    public int AttackSword()
    {
        Sword.SetActive(true);
        if (timeEvents > TimeSword)
        {
            Sword.SetActive(false);
            timeEvents = 0.0f;
            rotSwordAxeZ = -50;
            return 1;
        }
        else
        {
            timeEvents += Time.deltaTime;
            if (Time.realtimeSinceStartup - last_time_fire > 0.1f)
            {
                last_time_fire = Time.realtimeSinceStartup;
                if (rotSwordAxeZ > -50)
                    rotSwordAxeZ -= 1;
                if (rotSwordAxeZ > -40)
                {

                    if (rotSwordAxeZ > -20)
                    {
                        if (rotSwordAxeZ > 0)
                        {
                            resultAttackSword = 3;
                            pngSword.color = Color.blue;
                        }
                        else
                        {
                            resultAttackSword = 2;
                            pngSword.color = Color.green;
                        }
                    }
                    else
                    {
                        resultAttackSword = 1;
                        pngSword.color = Color.yellow;
                    }
                }
                else
                {
                    resultAttackSword = 0;
                    pngSword.color = Color.red;
                }
            }
            if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
            {
                countSpace++;
                Debug.Log(countSpace);
                if (rotSwordAxeZ < 35)
                {
                    rotSwordAxeZ += 4;
                }
            }
            Sword.transform.rotation = Quaternion.Euler(0, 0, rotSwordAxeZ);

        }
        return 0;
    }

    public int SpellHeal()
    {
        imgHealEmpty.SetActive(true);
        imgHeal.SetActive(true);
        if (timeEvents > TimeSpell)
        {

            imgHealEmpty.SetActive(false);
            imgHeal.SetActive(false);
            timeEvents = 0.0f;

            if (imgHeal.GetComponent<Image>().fillAmount < 0.8f && imgHeal.GetComponent<Image>().fillAmount > 0.7f)
            {
                imgHeal.GetComponent<Image>().fillAmount = 1;
                return 1;
            }
            else
            {
                imgHeal.GetComponent<Image>().fillAmount = 1;
                return 2;
            }
        }
        else
        {
            timeEvents += Time.deltaTime;
            if (Time.realtimeSinceStartup - last_time_fire > 0.1f)
            {
                last_time_fire = Time.realtimeSinceStartup;
                if (imgHeal.GetComponent<Image>().fillAmount > 0)
                    imgHeal.GetComponent<Image>().fillAmount -= 0.025f;
                if (imgHeal.GetComponent<Image>().fillAmount < 0.8f && imgHeal.GetComponent<Image>().fillAmount > 0.7f)
                {
                    resultAttackSword = 0;
                    imgHeal.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    imgHeal.GetComponent<Image>().color = Color.red;
                }
            }
            if (Input.GetKeyDown("space") || Input.GetButton("Fire1"))
            {
                countSpace++;
                Debug.Log(countSpace);
                if (imgHeal.GetComponent<Image>().fillAmount < 1)
                {
                    imgHeal.GetComponent<Image>().fillAmount += 0.01f;
                }
            }


        }
        return 0;
    }
}
