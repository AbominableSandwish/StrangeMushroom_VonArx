using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBattleManager : MonoBehaviour
{
    // 0 = Wait, 1 = Player , 2 = Monster
    private int CharactersPlay = 0;
    // Use this for initialization
    private bool WaitPlay = false;
    [SerializeField] PlayerController Player;

    [SerializeField] GameObject ObjectDamagePrefab;
    private int EnemyCount;
    private GameObject[] enemyList;

    [SerializeField] private MenuManager menu_manager;

    void Start()
    {
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaitPlay)
        {
            enemyList = null;
            EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            if (CharactersPlay != 0)
            {
                int n = 1;
                //Search enemy Playing
                foreach (GameObject enemy in enemyList)
                {
                    if (n == CharactersPlay)
                    {
                        EnemyController scriptEnemy = enemy.GetComponent<EnemyController>();
                        scriptEnemy.Init();
                    }
                    n++;
                }
            }
            else
            {
                Player.Init();
            }
            WaitPlay = true;
        }
    }

    //Change turn Play
    public void finishPlay()
    {
        CharactersPlay++;
        if (CharactersPlay > EnemyCount)
            CharactersPlay = 0;
        WaitPlay = false;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            menu_manager.GameWin();
        }
    }

    //Show Damage in the view 
    public void TakeDamage(int _damage, GameObject enemy)
    {
        EnemyController scriptEnemy = enemy.GetComponent<EnemyController>();
        GameObject Damage = Instantiate(ObjectDamagePrefab, scriptEnemy.SpawnDamagePrefab.transform.position, scriptEnemy.SpawnDamagePrefab.transform.rotation);
        Damage.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        Damage.transform.position = new Vector3(enemy.transform.position.x + 1, 0, scriptEnemy.SpawnDamagePrefab.transform.position.z);
        Damage.GetComponentInChildren<Text>().text = _damage.ToString();
        Destroy(Damage, 2);
    }
    //Show Damage in the view
    public void TakeDamage(int _damage)
    {
        GameObject Damage = Instantiate(ObjectDamagePrefab, Player.SpawnDamagePrefab.transform.position, Player.SpawnDamagePrefab.transform.rotation);
        Damage.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        Damage.transform.localPosition.Set(Player.SpawnDamagePrefab.transform.position.x, Player.SpawnDamagePrefab.transform.position.y, Player.SpawnDamagePrefab.transform.position.z);
        Damage.GetComponentInChildren<Text>().text = _damage.ToString();
        Destroy(Damage, 2);
    }

    public void FinishTurn()
    {
        finishPlay();
    }

    public GameObject[] GetListEnemy()
    {
        return enemyList;
    }

    public GameObject GetInfoPlayer()
    {
        return Player.gameObject;
    }

    public int GetEnemyCount()
    {
        return EnemyCount;
    }

    public void GameOver()
    {
        menu_manager.GameOver();
    }
}
