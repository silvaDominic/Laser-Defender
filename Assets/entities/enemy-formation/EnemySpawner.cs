using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    // 'Width' of enemy formation
    public float spread;
    // 'Height' of enemy formation
    public float depth;
    // Speeds of enemy formation
    public float horizontalSpeed;

    private bool isMovingRight = true;
    private float xmax;
    private float xmin;

    // Use this for initialization
    void Start () {

        float distanceDiff = gameObject.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceDiff));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceDiff));

        xmin = leftBoundary.x;
        xmax = rightBoundary.x;

        // Generates enemies on positioned game objects at start of game
        foreach (Transform child in gameObject.transform) {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
	}

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(spread, depth, 0));
    }

    // Update is called once per frame
    void Update () {
        MoveFormation();
	}

    void MoveFormation() {
        if (isMovingRight) {
            gameObject.transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;
        } else {
            gameObject.transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;
        }

        float leftEdgeOfFormation = gameObject.transform.position.x - (spread / 2);
        float rightEdgeOfFormation = gameObject.transform.position.x + (spread / 2);

        if (leftEdgeOfFormation <= xmin) {
            isMovingRight = true;
        } else if (rightEdgeOfFormation >= xmax) {
            isMovingRight = false;
        }
    }
}
