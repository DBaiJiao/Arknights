using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Character : MonoBehaviour
{
    public string characterName;
    public GameObject operatorManager;

    private Ray ra;
    private RaycastHit hit;
    public GameObject floorBlock; // the floor block hit by ray

    public GameObject character;

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    [Header("Deploy Related------------------------")]
    public int layerMask;
    public bool is_dragging;
    public bool is_directed;
    public bool is_deployed;
    public bool is_directing;
    public bool is_attacking;
    public bool is_selected;
    public enum direction
    {
        none,
        up,
        down,
        left,
        right
    }
    public Vector2 lastPosition;
    public direction dir;
    public float checkLength;

    [Header("Enemy Related-------------------------")]
    public int maxBlockNum;
    public int maxAttackNum;
    public List<GameObject> blockedEnemy = new List<GameObject>();
    public List<GameObject> attackedEnemy = new List<GameObject>();

    [Header("Attack Related------------------------")]
    public float attackSpeed;
    public float timeFromLastAttack = 1;

    // Start is called before the first frame update
    void Start()
    {
        characterName = "Lucia";
        operatorManager = GameObject.FindGameObjectWithTag("OperatorManager");

        character = this.gameObject;
        is_dragging = true;
        is_directed = false;
        is_deployed = false;
        is_directing = false;
        is_attacking = false;
        is_selected = false;

        layerMask = 1 << 10;
        skeletonAnimation = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.state;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_dragging)
        {
            if (Input.GetMouseButton(0))
            {
                ra = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ra, out hit, 10000, layerMask))
                {
                    floorBlock = hit.collider.gameObject;
                    character.transform.position = floorBlock.transform.position + new Vector3(0, 0.5f, 0);
                }
                else
                {

                }
            }
        }
        if (Input.GetMouseButtonUp(0) && hit.transform == null)
        {
            ResetToButton();
        }
        if (!is_directed)
        {
            ChooseDirection();
            switch (dir){
                case direction.right:
                    gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case direction.up:
                    gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case direction.left:
                    gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, -180, 0);
                    break;
                case direction.down:
                    gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, -270, 0);
                    break;
            }
            if (is_deployed && is_directed)
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
                hit.collider.gameObject.GetComponent<FloorBlockInfo>().character = character;
                operatorManager.GetComponent<OperatorManager>().deployed_operator.Add(this.gameObject);
             }
        }
        if (attackedEnemy.Count > 0)
        {
            is_attacking = true;
        }
        else
        {
            is_attacking = false;
        }
        if (is_attacking)
        {
            Attack();
        }
        else if (skeletonAnimation.state.ToString() != "idle_stand")
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            spineAnimationState.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, "idle_stand", true);
        }
        ClearDeadInList();

        gameObject.transform.GetChild(3).gameObject.SetActive(is_selected);
        gameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(is_selected);
    }

    public void ChooseDirection()
    {
        if (is_deployed && !is_directed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPosition = Input.mousePosition;
                is_directing = true;
            }
            if (Input.GetMouseButton(0) && is_directing)
            {
                Vector2 currPosition = Input.mousePosition;

                if (currPosition.x > lastPosition.x + checkLength && Mathf.Abs(currPosition.x - lastPosition.x) > Mathf.Abs(currPosition.y - lastPosition.y))
                {
                    dir = direction.right;
                    gameObject.transform.GetChild(0).transform.localScale = new Vector3(1,1,1);
                    gameObject.transform.GetChild(4).transform.position = gameObject.transform.position + new Vector3(-1, 0, 0);
                }
                else if (currPosition.x < lastPosition.x - checkLength && Mathf.Abs(currPosition.x - lastPosition.x) > Mathf.Abs(currPosition.y - lastPosition.y))
                {
                    dir = direction.left;
                    gameObject.transform.GetChild(0).transform.localScale = new Vector3(-1, 1, 1);
                    gameObject.transform.GetChild(4).transform.position = gameObject.transform.position + new Vector3(1, 0, 0);
                }
                else if (currPosition.y > lastPosition.y - checkLength && Mathf.Abs(currPosition.y - lastPosition.y) > Mathf.Abs(currPosition.x - lastPosition.x))
                {
                    dir = direction.up;
                    gameObject.transform.GetChild(4).transform.position = gameObject.transform.position + new Vector3(0, 0, -1);
                }
                else if (currPosition.y < lastPosition.y - checkLength && Mathf.Abs(currPosition.y - lastPosition.y) > Mathf.Abs(currPosition.x - lastPosition.x))
                {
                    dir = direction.down;
                    gameObject.transform.GetChild(4).transform.position = gameObject.transform.position + new Vector3(0, 0, 1);
                }
            }
            if (Input.GetMouseButtonUp(0) && is_directing)
            {
                dir = direction.none;
                is_directed = true;
                is_directing = false;
                gameObject.transform.GetChild(4).transform.position = gameObject.transform.position;
                gameObject.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        if (is_attacking)
        {
            if (timeFromLastAttack > 1)
            {
                skeletonAnimation.skeleton.SetToSetupPose();
                spineAnimationState.ClearTracks();
                skeletonAnimation.state.SetAnimation(0, "attack_routine_04", true);
                /*
                for (int enemyIndex = 0; enemyIndex < maxAttackNum; enemyIndex++)
                {
                    if (attackedEnemy[enemyIndex])
                    {
                        attackedEnemy[enemyIndex].GetComponent<Enemy>().currHealth -= 1;
                    }
                }
                */
                foreach (GameObject enemy in attackedEnemy)
                {
                    enemy.GetComponent<Enemy>().currHealth -= 1;
                }

                timeFromLastAttack = 0;
            }
            timeFromLastAttack += Time.deltaTime;
        }
    }

    public void ClearDeadInList()
    {
        if (blockedEnemy.Count > 0)
        {
            foreach (GameObject enemy in blockedEnemy)
            {
                if (enemy.GetComponent<Enemy>().dead)
                {
                    blockedEnemy.Remove(enemy);
                }
            }
        }

        if (attackedEnemy.Count > 0)
        { 
            foreach (GameObject enemy in attackedEnemy)
            {
                if (enemy.GetComponent<Enemy>().dead)
                {
                    attackedEnemy.Remove(enemy);
                }
            }
        }
    }

    public void ResetToButton()
    {
        gameObject.SetActive(false);
        character = this.gameObject;
        is_dragging = false;
        is_directed = false;
        is_deployed = false;
        is_directing = false;
        is_attacking = false;
        is_selected = false;
        blockedEnemy.Clear();
        attackedEnemy.Clear();
    }
}
