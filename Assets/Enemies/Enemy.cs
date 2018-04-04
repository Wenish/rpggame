using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField]
    float maxHealthPoints = 100f;

    [SerializeField]
    float attackRadius = 4f;

    float currentHealthPoints = 100f;

    AICharacterControl aICharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aICharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(distanceToPlayer <= attackRadius)
        {
            aICharacterControl.SetTarget(player.transform);
        } else
        {
            aICharacterControl.SetTarget(transform);
        }
    }
}
