using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

[Serializable]
public class NavMeshControlBehavior : PlayableBehaviour
{
    public float maxSpeed;
    [Tooltip("A fraction of max speed")]
    public float speedFraction;
    public Transform navigationTarget;
    public bool teleport = false;
    public Animator animator;
    public AnimationParameter[] animationParameters;

    bool parametersSet = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var navMeshAgent = playerData as NavMeshAgent;

        if (navMeshAgent == null) return;

        if(!navMeshAgent.isOnNavMesh)
        {
            NavMeshHit closestHit;
            if(NavMesh.SamplePosition(navMeshAgent.gameObject.transform.position, out closestHit, 500, NavMesh.AllAreas))
            {
                navMeshAgent.gameObject.transform.position = closestHit.position;
            }
        }

        Debug.Log(animator);

        if(animationParameters.Length > 0 && !parametersSet)
        {
            foreach (AnimationParameter parameter in animationParameters)
            {
                if (parameter.animationParameterType == AnimationParameter.AnimationParameterType.Trigger) animator.SetTrigger(parameter.animationParameter);

                if (parameter.animationParameterType == AnimationParameter.AnimationParameterType.Bool) animator.SetBool(parameter.animationParameter, (bool)parameter.GetValue());

                if (parameter.animationParameterType == AnimationParameter.AnimationParameterType.Float) animator.SetFloat(parameter.animationParameter, (float)parameter.GetValue());

                if (parameter.animationParameterType == AnimationParameter.AnimationParameterType.Int) animator.SetInteger(parameter.animationParameter, (int)parameter.GetValue());
            }
            parametersSet = true;
        }

        if(navigationTarget && !teleport)
        {
            navMeshAgent.speed = maxSpeed * speedFraction;
            navMeshAgent.SetDestination(navigationTarget.position);
            navMeshAgent.isStopped = false;
        } else if (teleport)
        {
            navMeshAgent.Warp(navigationTarget.position);
        }

    }
}
