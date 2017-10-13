﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public float health = 100;
    public GameObject projectile;
    public float pulseLaserSpeed = 5f;
    public float fireRate = 0.2f;
    public float shotsPerSecond = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float probabilityOfFiring = shotsPerSecond * Time.deltaTime;
        if (Random.value < probabilityOfFiring) {
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();

        if (projectile) {
            Debug.Log("Enemy Hit " + collider.gameObject);
            projectile.Hit();
            this.health -= projectile.GetDamage();

            if (this.health <= 0) {
                Debug.Log("Enemy Killed");
                Destroy(this.gameObject);
            }
        }
    }

    void Shoot() {
        Vector3 projectileStartingPos = gameObject.transform.position + new Vector3(0, -0.5f, 0);
        GameObject pulseLaser = Instantiate(projectile, projectileStartingPos, Quaternion.identity) as GameObject;
        pulseLaser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -pulseLaserSpeed, 0);
    }
}
