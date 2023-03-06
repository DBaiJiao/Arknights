using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineTest : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    Spine.AnimationState spineAnimationState;
    Spine.Skeleton skeleton;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.state;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            spineAnimationState.ClearTracks();
            skeletonAnimation.AnimationName = "Run";
        }
        if (Input.GetKey(KeyCode.A))
        {
            skeletonAnimation.skeleton.SetToSetupPose();
            spineAnimationState.ClearTracks();
            skeletonAnimation.state.SetAnimation(0, "run", true);
        }
    }
}
