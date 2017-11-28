using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EnemyController : MonoBehaviour {
    private bool Play = false;
    public int life_monster = 15;
    [SerializeField] GameBattleManager managerBattle;
    private Vector3 Position_monster_destination;

    [SerializeField] public GameObject SpawnDamagePrefab;

    float cooldownAfterSwitchAttack;
    const float periodAfterAttack= 2.2f;

    [SerializeField] private Text lifeEnemy;

    enum State
    {
        IN_WAIT,
        ATTACK,

        WAIT,
        
        STUN,
        DEAD
    }
    State state = State.IN_WAIT;

    [Header("SOUNDS")]
    [SerializeField]
    private SoundManagerMonsters manageSound;

    void Start () {
        lifeEnemy.text = life_monster.ToString();
        Position_monster_destination = transform.position;
        SoundManagerMonsters manageSound = GetComponent<SoundManagerMonsters>();
    }

    void Update()
    {

        switch (state)
        {
            case State.IN_WAIT:
                break;

            case State.ATTACK:
                GetComponent<Collider2D>().enabled = true;
                Attack();
              
                break;

            case State.WAIT:
                cooldownAfterSwitchAttack += Time.deltaTime;
                if (cooldownAfterSwitchAttack >= periodAfterAttack)
                {
                    GetComponent<Collider2D>().enabled = false;
                    managerBattle.finishPlay();
                    state = State.IN_WAIT;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Arrows")
        {
            GetComponent<Collider2D>().enabled = false;
            TakeDamage(8);
            
            Destroy(collision.gameObject);
            
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position = Position_monster_destination;
        Play = false;
        state = State.WAIT;
    }

    public void Init()
    {
        cooldownAfterSwitchAttack =0.0f;
        Play = true;
        state = State.ATTACK;
    }

    private void Attack()
    {
        manageSound.PlaySound(1);
        transform.position = new Vector3(transform.position.x-0.24f, transform.position.y, transform.position.z);
    }

    public void TakeDamage(int damage)
    {
        managerBattle.TakeDamage(damage, this.gameObject);
        life_monster -= damage;
        manageSound.PlaySound(1);
        lifeEnemy.text = life_monster.ToString();
        if (life_monster <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
