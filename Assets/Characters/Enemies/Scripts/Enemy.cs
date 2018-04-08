using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float chaseRadius = 9f;

    [SerializeField] float attackRadius = 4f;

    [SerializeField] float damagePerShot = 20f;

    [SerializeField] float attackSpeed = 0.5f;


    [SerializeField] GameObject projectileToUse = null;

    [SerializeField] GameObject projectileSocket;

    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

    float currentHealthPoints;

    AICharacterControl aICharacterControl = null;
    GameObject player = null;

    bool isAttacking = false;
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
        if(currentHealthPoints <= 0 )
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealthPoints = maxHealthPoints;
        player = GameObject.FindGameObjectWithTag("Player");
        aICharacterControl = GetComponent<AICharacterControl>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, attackSpeed); //TODO switch to coroutines
        }

        if(distanceToPlayer > attackRadius)
        {
            CancelInvoke();
            isAttacking = false;
        }

        if (distanceToPlayer <= chaseRadius)
        {
            aICharacterControl.SetTarget(player.transform);
        }
        else
        {
            aICharacterControl.SetTarget(transform);
        }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, projectileSocket.transform.rotation);

        Destroy(newProjectile, 1f);

        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

        
    }

    void OnDrawGizmos()
    {
        //Draw move sphere
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        //Draw attack sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
