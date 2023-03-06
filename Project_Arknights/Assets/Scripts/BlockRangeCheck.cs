using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRangeCheck : MonoBehaviour
{
    public int maxBlockNum;
    public int currBlockNum;

    void Start()
    {
        maxBlockNum = gameObject.transform.parent.GetComponent<Character>().maxBlockNum;
    }

    void Update()
    {
        currBlockNum = gameObject.transform.parent.GetComponent<Character>().blockedEnemy.Count;
    }
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        GameObject enemy = other.gameObject.transform.parent.gameObject;
        if (other.tag == "Enemy" && currBlockNum < maxBlockNum && !gameObject.transform.parent.GetComponent<Character>().blockedEnemy.Contains(enemy) && !enemy.GetComponent<Enemy>().dead)
        {
            enemy.GetComponent<Enemy>().is_blocked = true;
            gameObject.transform.parent.GetComponent<Character>().blockedEnemy.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            gameObject.transform.parent.GetComponent<Character>().blockedEnemy.Remove(other.gameObject.transform.parent.gameObject);
        }
    }
}
