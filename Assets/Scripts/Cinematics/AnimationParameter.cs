using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationParameter
{
    public enum AnimationParameterType
    {
        Trigger,
        Bool,
        Float,
        Int
    };
    public string animationParameter;
    public AnimationParameterType animationParameterType;
    [SerializeField] string animationParameterValue;

    public object GetValue()
    {
        if (animationParameterType == AnimationParameterType.Bool) return animationParameterValue == "true";

        if (animationParameterType == AnimationParameterType.Float) return float.Parse(animationParameterValue);

        if (animationParameterType == AnimationParameterType.Int) return int.Parse(animationParameterValue);

        return "trigger";
    }
}