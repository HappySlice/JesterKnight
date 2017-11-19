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

    private void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.collider.gameObject.GetComponent(typeof(IDamageable));
        //Debug.Log("Collided with damageableComponent: " + damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
        Destroy(this.gameObject, 0.01f);
    }
}
