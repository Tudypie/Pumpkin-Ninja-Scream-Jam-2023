using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimation : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        float randomOffset = Random.Range(0f, 1f);
        string animationName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        anim.Play(animationName, 0, randomOffset);
    }
}
