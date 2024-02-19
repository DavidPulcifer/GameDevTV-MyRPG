using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class NavMeshControlClip : PlayableAsset, ITimelineClipAsset
{
    

    [SerializeField] ExposedReference<Transform> navigationTarget;
    [SerializeField] bool teleport = false;
    [SerializeField] ExposedReference<Animator> animator;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedFraction = 1;
    [SerializeField] AnimationParameter[] animationParameters;

    NavMeshControlBehavior template = new NavMeshControlBehavior();

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        template.navigationTarget = navigationTarget.Resolve(graph.GetResolver());
        template.teleport = teleport;
        template.speedFraction = speedFraction;
        template.maxSpeed = maxSpeed;
        template.animator = animator.Resolve(graph.GetResolver());
        template.animationParameters = animationParameters;
        return ScriptPlayable<NavMeshControlBehavior>.Create(graph, template);
    }
}
