using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;

    [SerializeField] float chaseRadius = 6.0f;
    [SerializeField] float attackRadius = 4.0f;

    [SerializeField] float damagePerShot = 9.0f;
    [SerializeField] float secondsBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;
    [SerializeField] Vector3 aimOffset = new Vector3(0.0f, 1.0f, 0.0f);

    bool isAttacking = false;
    float currentHealthPoints = 100f;
    ThirdPersonCharacter thirdPersonCharacter = null;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    private void Start()
    {
        thirdPersonCharacter = this.GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = this.GetComponent<AICharacterControl>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealthPoints = maxHealthPoints;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        if (distanceToPlayer <= attackRadius && isAttacking == false)
        {
            isAttacking = true;
            //SpawnProjectile();
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); // TODO switch to coroutines
        }
        if(distanceToPlayer > attackRadius)
        {
            CancelInvoke("SpawnProjectile");
            isAttacking = false;
        }

        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(this.transform);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0.0f, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = (GameObject)Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);

        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;
    }

    private void OnDrawGizmos()
    {
        // Draw attack sphere
        Gizmos.color = new Color(255.0f, 0.0f, 0.0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw move sphere
        Gizmos.color = new Color(0.0f, 0.0f, 255.0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
