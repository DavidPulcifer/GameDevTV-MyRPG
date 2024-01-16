using RPG.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Abilities/Targeting/Directional", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1f;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit rayCastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out rayCastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(rayCastHit.point + ray.direction*groundOffset/ray.direction.y);
            }
            finished();
        }
    }
}
