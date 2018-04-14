using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;

        // Use this for initialization
        void Start()
        {
            print("AreaEffect behavior attached to " + gameObject.name);
        }

        public void SetConfig(AreaEffectConfig areaEffectConfig)
        {
            this.config = areaEffectConfig;
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Area Effect skill used");
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

            float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
            foreach(RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    } 
}
