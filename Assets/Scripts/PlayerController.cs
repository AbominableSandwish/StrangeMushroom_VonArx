using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    

    #region Init Var
    private bool _play = false;
    public int Life = 25;
    [SerializeField] private GameObject lifePlayer;
    [SerializeField] private GameBattleManager manageBattle;
    [SerializeField] private MenuBattlePlayer manageMenuPlayer;
    [SerializeField] private PlayerAttack attackPlayer;
    private bool finish_choice;
    float cooldownAfterSwitchAttack;
    const float periodAfterAttack = 2.2f;

    [Header("SOUNDS")]
    [SerializeField]
    private SoundManagerPlayer manage_sound_player;

    [Header("Pause")]
    [SerializeField]
    private PauseManager pause_manager;

    enum State
    {
        IN_WAIT,
        MENU,
        CHOICE_ENEMY,
        EVENT_ATTACK,
        ATTACK,
        WAIT,
        STUN,
        WIN,
        DEAD
    }
    State state = State.MENU;

    [SerializeField] public GameObject SpawnDamagePrefab;
    #endregion

    void Start()
    {
        Text[] listText = lifePlayer.GetComponentsInChildren<Text>();
        listText[0].text = Life + "";
        listText[1].text = Life + " /";
    }

    void Update()
    {
        if (!(pause_manager.GetIsInPause()))
        {
            switch (state)
            {
                //When a player is in the menu
                case State.MENU:
                    finish_choice = manageMenuPlayer.ManageMenu();
                    if (finish_choice)
                    {
                        state = State.CHOICE_ENEMY;
                    }
                    break;
                //When a player Choice who after the menu
                case State.CHOICE_ENEMY:
                    switch (manageMenuPlayer.AttackForChoiceEnemys(manageMenuPlayer.GetChoiceMenuBattle()))
                    {
                        case -1:
                            manageMenuPlayer.ShowScroolMenuBattle();
                            state = State.MENU;
                            break;
                        case 0:
                            state = State.CHOICE_ENEMY;
                            break;
                        case 1:
                            state = State.EVENT_ATTACK;
                            break;
                    }
                    break;
                //When a player play
                case State.EVENT_ATTACK:
                    switch (attackPlayer.ChoiceMenu(manageMenuPlayer.GetChoiceMenuBattle()))
                    {
                        case 0:
                            state = State.EVENT_ATTACK;
                            break;
                        case 1:
                            state = State.ATTACK;
                            break;
                        case 2:
                            state = State.WAIT;
                            break;
                    }
                    break;
                //When the player finish his attack
                case State.ATTACK:
                    attackPlayer.AttackSuccess(manageMenuPlayer.GetChoiceMenuBattle());
                    state = State.WAIT;
                    break;
                //When the player wait his turn for play
                case State.WAIT:

                    cooldownAfterSwitchAttack += Time.deltaTime;
                    if (cooldownAfterSwitchAttack >= periodAfterAttack)
                    {
                        manageBattle.finishPlay();
                        state = State.IN_WAIT;
                    }
                    break;
            }
        }
    }

    //Init Turn the player and Show his menu
    public void Init()
    {
        if (state == State.IN_WAIT)
        {
            cooldownAfterSwitchAttack = 0.0f;
            manageMenuPlayer.ShowScroolMenuBattle();
            state = State.MENU;
        }
    }

    private void ChoiceEnemyAttack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            int damage = 2;//tmp
            Life -= damage;
            if (Life <= 0)
            {
                manageBattle.GameOver();
            }
            manage_sound_player.PlaySound(0);
            manageBattle.TakeDamage(damage);
            Text[] listText = lifePlayer.GetComponentsInChildren<Text>();
            listText[1].text = Life + " /";
        }
    }

    public void HealPlayer(int heal)
    {
        Life += heal;
        if (Life > 25)
        {
            Life = 25;
        }
        Text[] listText = lifePlayer.GetComponentsInChildren<Text>();
        listText[1].text = Life + " /";
    }
}
