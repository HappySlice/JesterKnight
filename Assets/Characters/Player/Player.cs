using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2.0f;

    GameObject currentTarget;
    float currentHealthPoints = 100f;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0.0f;

    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
    }

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if(layerHit == enemyLayer)
        {
            GameObject enemy = raycastHit.collider.gameObject;
            currentTarget = enemy;

            // Check enemy is in range
            if((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0.0f, maxHealthPoints);
        if(currentHealthPoints <= 0)
        {
            //Destroy(gameObject);
        }
    }
}
