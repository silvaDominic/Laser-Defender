using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float thrust = 5.0f;
    public float shipPadding = 1.0f;
    public GameObject projectile;
    public float pulseLaserSpeed = 5f;
    public float fireRate = 0.2f;
    public float health = 100;

    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start () {
        float distanceDiff = gameObject.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0 , 0, distanceDiff));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0, distanceDiff));

        xmin = leftBoundary.x + shipPadding;
        xmax = rightBoundary.x - shipPadding;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            gameObject.transform.position += Vector3.left * thrust * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            gameObject.transform.position += Vector3.right * thrust * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Shoot", 0.0000001f, fireRate);
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("Shoot");
        }

        //Restricts players ship to gamespace
        float newX = Mathf.Clamp(gameObject.transform.position.x, xmin, xmax);
        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();
        if (projectile) {
            Debug.Log("Player Hit " + collider.gameObject);
            projectile.Hit();
            this.health -= projectile.GetDamage();

            if (this.health <= 0) {
                Debug.Log("Player Killed");
                Destroy(this.gameObject);
            }
        }
    }

    void Shoot() {
        Vector3 projectileStartingPos = gameObject.transform.position + new Vector3(0, 0.5f, 0);
        GameObject pulseLaser = Instantiate(projectile,  projectileStartingPos, Quaternion.identity) as GameObject;
        pulseLaser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, pulseLaserSpeed, 0);
    }
}
