using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBattlePlayer : MonoBehaviour
{
    [SerializeField] private GameBattleManager manage_battle_cs;
    #region VarForMenuBattlePlayer
    [SerializeField] private Transform all_menu_transform;
    [SerializeField] private Transform[] menu_transform_list;
    public List<Vector3> InitDestinationList;
    public List<Vector3> DestinationList;
    private int min_scroll_menu = 0;
    private int max_scroll_menu;
    private int choice_menu = 0;
    private bool change_choice = true;
    private float velocity_scrool_menu = 4.0f;
    private bool finish_choice = false;
    #endregion

    #region VarForChoiceEnemysAttackBeforeAttack
    [SerializeField] private Transform cursor_attack_enemy;
    [SerializeField] private GameObject zone_attack_sword;
    [SerializeField] private GameObject msg_attack_sword;
    [SerializeField] private GameObject msg_attack_bow;
    [SerializeField] private Transform cursor_heal_player;
    private int enemyAttack = -1;
    #endregion

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Animation());
        max_scroll_menu = menu_transform_list.Length - 1;
        if (InitDestinationList.Count == 0)
            InitFirstPositionDestination();
        for (int i = min_scroll_menu; i <= max_scroll_menu; i++)
        {
            DestinationList.Add(InitDestinationList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        IncrementPosition();
    }

    #region ChoiceEnemysAttackBeforeAttack

    public int AttackForChoiceEnemys(int attack)
    {
        int tmp = 0;
        switch (attack)
        {
            case 0:
                tmp = ChoiceEnemyAttackWithBow(manage_battle_cs.GetEnemyCount());
                break;
            case 1:
                tmp = ChoicePlayerHeal();
                break;
            case 2:
                tmp = ChoiceEnemysAttackWithSword(manage_battle_cs.GetEnemyCount());
                break;
        }
        return tmp;
    }

    // Ask Player for his choice for Heal
    public int ChoicePlayerHeal()
    {
        cursor_heal_player.gameObject.SetActive(true);
        // Return in menuBattle player
        if (Input.GetKeyDown("escape") || Input.GetButtonDown("Fire2"))
        {
            //  msg_attack_sword.SetActive(false);
            cursor_heal_player.gameObject.SetActive(false);
            return -1;
        }
        // Confirm his Choice
        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
        {
            //msg_attack_sword.SetActive(false);
            cursor_heal_player.gameObject.SetActive(false);
            return 1;
        }
        return 0;
    }
    // Ask Player for his choice for attack Sword
    public int ChoiceEnemysAttackWithSword(int countEnemy)
    {

        GameObject[] tmp = manage_battle_cs.GetListEnemy();
        msg_attack_sword.SetActive(true);
        zone_attack_sword.SetActive(true);
        // Return in menuBattle player
        if (Input.GetKeyDown("escape") || Input.GetButtonDown("Fire2"))
        {
            msg_attack_sword.SetActive(false);
            zone_attack_sword.SetActive(false);
            enemyAttack = 0;
            return -1;
        }
        // Confirm his Choice
        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
        {
            msg_attack_sword.SetActive(false);
            zone_attack_sword.SetActive(false);
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i].GetComponent<Collider2D>().enabled = true;
            }
            //enemyAttack = -1;
            return 1;
        }
        return 0;
    }

    public int ChoiceEnemyAttackWithBow(int countEnemy)
    {
        GameObject[] tmp = manage_battle_cs.GetListEnemy();
        msg_attack_bow.SetActive(true);
        cursor_attack_enemy.gameObject.SetActive(true);
        // Return in menuBattle player
        if (Input.GetKeyDown("escape") || Input.GetButtonDown("Fire2"))
        {
            msg_attack_bow.SetActive(false);
            cursor_attack_enemy.gameObject.SetActive(false);
            enemyAttack = 0;
            return -1;
        }
        else
        {
            // Confirm his Choice
            if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
            {
                msg_attack_bow.SetActive(false);
                cursor_attack_enemy.gameObject.SetActive(false);
                tmp[enemyAttack].GetComponent<Collider2D>().enabled = true;
                enemyAttack = -1;
                return 1;
            }
            else
            {
                // Change Enemy choice for attack
                if (Input.GetKeyDown("a") || Input.GetAxis("Horizontal") < 0)
                {
                    if (enemyAttack > 0)
                    {
                        enemyAttack--;
                    }
                }
                // Change Enemy choice for attack
                if (Input.GetKeyDown("d") || Input.GetAxis("Horizontal") > 0)
                {
                    if (enemyAttack < countEnemy - 1)
                    {
                        enemyAttack++;
                    }
                }

                if (enemyAttack == -1)
                    enemyAttack = 0;
                if (enemyAttack != -1)
                {
                    cursor_attack_enemy.transform.position = new Vector3(tmp[enemyAttack].transform.position.x, cursor_attack_enemy.transform.position.y, cursor_attack_enemy.transform.position.z);
                }
            }
        }
        return 0;
    }
    #endregion

    #region MenuBattlePlayer
    public bool ManageMenu()
    {
        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i <= max_scroll_menu; i++)
            {
                DestinationList[i] = all_menu_transform.position;
            }
            finish_choice = true;
            return true;
        }
        else
        {
            ScroolMenuBattle(Input.GetAxis("Horizontal"));
            return false;
        }
    }

    public int GetChoiceMenuBattle()
    {
        return choice_menu;
    }

    private void ScroolMenuBattle(float horizontal)
    {
        if (horizontal != 0 && change_choice)
        {
            if (horizontal < 0)
            {
                // Set the destination to be the object's position so it will not start off moving
                SetDestination(1);
                choice_menu++;
                if (choice_menu > max_scroll_menu)
                {
                    choice_menu = min_scroll_menu;
                }
                change_choice = false;
            }
            else if (horizontal > min_scroll_menu)
            {
                // Set the destination to be the object's position so it will not start off moving
                SetDestination(-1);
                choice_menu--;
                if (choice_menu < min_scroll_menu)
                {
                    choice_menu = max_scroll_menu;
                }
                change_choice = false;
            }
            Debug.Log(choice_menu);
        }

        for (int i = min_scroll_menu; i <= max_scroll_menu; i++)
        {
            // If the object is not at the target destination
            if (DestinationList[i] == menu_transform_list[i].transform.position)
            {
                change_choice = true;
            }
        }
    }

    private void IncrementPosition()
    {
        for (int i = min_scroll_menu; i <= max_scroll_menu; i++)
        {
            // Calculate the next position
            float delta = velocity_scrool_menu * Time.deltaTime;
            Vector3 currentPosition = menu_transform_list[i].gameObject.transform.position;
            Vector3 nextPosition = Vector3.MoveTowards(currentPosition, DestinationList[i], delta);

            // Move the object to the next position
            menu_transform_list[i].gameObject.transform.position = nextPosition;
        }
    }
    // Set the destination to cause the object to smoothly glide to the specified location
    private void InitFirstPositionDestination()
    {
        for (int i = min_scroll_menu; i <= max_scroll_menu; i++)
        {
            InitDestinationList.Add(menu_transform_list[i].transform.position);
        }
    }


    // Set the destination to cause the object to smoothly glide to the specified location
    private void SetDestination(int value)
    {

        if (value < 0)
        {
            Vector3 destination_tmp;
            destination_tmp = DestinationList[min_scroll_menu];

            for (int i = min_scroll_menu; i <= max_scroll_menu - 1; i++)
            {
                DestinationList[i] = DestinationList[i + 1];
            }

            DestinationList[max_scroll_menu] = destination_tmp;

        }
        else if (value > 0)
        {
            Vector3 destination_tmp;
            destination_tmp = DestinationList[max_scroll_menu];

            for (int i = max_scroll_menu; i >= min_scroll_menu + 1; i--)
            {
                DestinationList[i] = DestinationList[i - 1];
            }
            DestinationList[min_scroll_menu] = destination_tmp;
        }

    }
    //Hide menubattle player After selected his attack
    public void HideScroolMenuBattle()
    {
        if (DestinationList[min_scroll_menu] != menu_transform_list[min_scroll_menu].transform.position)
        {
            IncrementPosition();
        }
        else
        {
            all_menu_transform.gameObject.SetActive(false);
            finish_choice = false;
        }
    }
    //Show menubattle Player
    public void ShowScroolMenuBattle()
    {
        choice_menu = 0;
        all_menu_transform.gameObject.SetActive(true);
        for (int i = min_scroll_menu; i <= max_scroll_menu; i++)
        {
            DestinationList[i] = InitDestinationList[i];
        }
    }
    #endregion

    private IEnumerator Animation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
            if (finish_choice)
            {
                HideScroolMenuBattle();

            }
        }
    }
}
