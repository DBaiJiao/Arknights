using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeCheck : MonoBehaviour
{
    public int maxAttackNum;
    public int currAttackNum;
    void Start()
    {
        maxAttackNum = gameObject.transform.parent.GetComponent<Character>().maxAttackNum;
    }

    void Update()
    {
        currAttackNum = gameObject.transform.parent.GetComponent<Character>().attackedEnemy.Count;
    }
    private void OnTriggerStay(Collider other)
    {
        GameObject enemy = other.gameObject.transform.parent.gameObject;
        if (other.tag == "Enemy" && currAttackNum < maxAttackNum && !gameObject.transform.parent.GetComponent<Character>().attackedEnemy.Contains(enemy) && !enemy.GetComponent<Enemy>().dead)
        {
            gameObject.transform.parent.GetComponent<Character>().attackedEnemy.Add(enemy);
        }
        if (currAttackNum > maxAttackNum)
        {
            for(int outIndex = maxAttackNum; outIndex < currAttackNum; outIndex++)
            {
                gameObject.transform.parent.GetComponent<Character>().attackedEnemy.Remove(gameObject.transform.parent.GetComponent<Character>().attackedEnemy[outIndex]);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Enemy")
        {
            gameObject.transform.parent.GetComponent<Character>().attackedEnemy.Remove(other.gameObject.transform.parent.gameObject);
        }
    }
}
