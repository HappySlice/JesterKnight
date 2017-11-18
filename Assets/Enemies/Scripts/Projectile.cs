using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    float damageCaused;
    public float projectileSpeed; // Note other classes can set

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        //Debug.Log("Collided with damageableComponent: " + damageableComponent);
        if(damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }
}
