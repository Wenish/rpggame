using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float projectileSpeed;

    [SerializeField] GameObject shooter; // So can inspected when paused

    const float DESTORY_DELAY = 1f;

    float damageCaused;

    void Start()
    {
        Destroy(gameObject, DESTORY_DELAY);
    }

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }
    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != shooter.layer)
        {
            DamageIfDamageable(collision);
        }

    }

    private void DamageIfDamageable(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            Destroy(gameObject);
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }

    internal float GetDefaultLaunchSpeed()
    {
        return projectileSpeed;
    }
}
