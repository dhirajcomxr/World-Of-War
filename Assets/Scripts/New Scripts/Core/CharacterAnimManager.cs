using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOT.Core
{
    public class CharacterAnimManager : MonoBehaviour
    {
        public Animator anim;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool(AnimHash.Interacting, isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
