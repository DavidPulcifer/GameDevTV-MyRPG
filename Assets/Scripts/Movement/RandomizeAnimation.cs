using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAnimation : MonoBehaviour
{
    Animator anim;
    float randomOffset;
    [SerializeField] string animationState = "Locomotion";

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        randomOffset = Random.Range(0f, 1f);

        anim.Play(animationState, 0, randomOffset);
    }    
}
