using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackDamage = 10;
    [SerializeField] float attackSpeed = 0.5f;
    [SerializeField] float maxAttackRange = 2f;
    [SerializeField] Transform spawnPoint = null;

    GameObject currentTarget;
    float currentHealthPoints;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0f;

    Animator animator;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    void Start()
    {
        currentHealthPoints = maxHealthPoints;
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        animator = GetComponent<Animator>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            //Check enemy is not in range
            if((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            currentTarget = enemy;
            var enemyComponent = enemy.GetComponent<Enemy>();
            if(Time.time - lastHitTime > attackSpeed)
            {
                transform.LookAt(enemy.transform);
                animator.SetTrigger("Attack");
                enemyComponent.TakeDamage(attackDamage);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if(currentHealthPoints <= 0)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            Transform transform = GetComponent<Transform>();
            transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            agent.enabled = true;
            agent.SetDestination(spawnPoint.position);
            currentHealthPoints = maxHealthPoints;
        }
    }
}
