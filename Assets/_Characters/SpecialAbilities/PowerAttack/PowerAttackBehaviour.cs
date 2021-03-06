﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {

        PowerAttackConfig config;

        // Use this for initialization
        void Start()
        {
            print("PowerAttack behavior attached to " + gameObject.name);
        }

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }
        
        public void Use(AbilityUseParams useParams)
        {
            print("PowerAttack used by: " + gameObject.name);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    } 
}
