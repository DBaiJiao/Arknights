using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    [Header("Movement Related----------------------")]
    public Vector3 direction = new Vector3 (1,0,0);
    public float speed = 1;

    [Header("Attribute-----------------------------")]
    public int maxHealth;
    public int currHealth;
    public bool dead;

    public bool is_blocked;

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    // Start is called before the first frame update
    void Start()
    {
        is_blocked = false;
        skeletonAnimation = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.state;
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_blocked)
        {
            Move();
            if (skeletonAnimation.state.ToString() != "Run" && dead == false)
            {
                skeletonAnimation.skeleton.SetToSetupPose();
                spineAnimationState.ClearTracks();
                skeletonAnimation.state.SetAnimation(0, "Run", true);
            }
        }
        else if (skeletonAnimation.state.ToString() != "Idel" && dead == false)
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            spineAnimationState.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, "Idel", true);
        }
        CheckHealth();
    }

    void Move()
    {
        gameObject.transform.position += direction * speed * Time.deltaTime;
    }

    void CheckHealth()
    {
        if (currHealth <= 0)
        {
            dead = true;

            skeletonAnimation.skeleton.SetToSetupPose();
            spineAnimationState.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, "HIT", false);

            StartCoroutine(StartDie());
        }
    }

    IEnumerator StartDie()
    {
        yield return new WaitForSeconds(1);
        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
