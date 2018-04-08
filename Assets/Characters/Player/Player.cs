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
    [SerializeField] Weapon weaponInUse;
    [SerializeField] GameObject weaponSocket;

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
        RegisterForMouseClick();
        currentHealthPoints = maxHealthPoints;
        PutWeaponInHand();


        animator = GetComponent<Animator>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        var weapon = Instantiate(weaponPrefab, weaponSocket.transform, true);
        weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
        weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
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
