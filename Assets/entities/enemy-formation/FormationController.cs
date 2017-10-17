using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {

    public GameObject enemyPrefab;
    // 'Width' of enemy formation
    public float spread;
    // 'Height' of enemy formation
    public float depth;
    // Speeds of enemy formation
    public float horizontalSpeed;
    public float spawnDuration = 2;

    private bool isMovingRight = true;
    private float xmax;
    private float xmin;
    private int formationSize;

    // Use this for initialization
    void Start () {

        float distanceDiff = gameObject.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceDiff));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceDiff));

        xmin = leftBoundary.x;
        xmax = rightBoundary.x;

        ResetFormation();
	}

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(spread, depth, 0));
    }

    // Update is called once per frame
    void Update () {
        MoveFormation();

        if (AllEnemiesAreDead()) {
            Debug.Log("ALL ENEMIES KILLED.");
            SpawnUntilFull();
        }
    }

    // Resets formation
    void ResetFormation() {
        Debug.Log("RESETTING FORMATION");
        SpawnUntilFull();
    }

    // Creates an instance of an enemy at a designated position
    void SpawnNewEnemy(Transform positionGameObject) {
        if (positionGameObject != null) {
            GameObject enemy = Instantiate(enemyPrefab, positionGameObject.transform.position, Quaternion.identity) as GameObject;
            //enemy.GetComponent<EnemyBehavior>().SetPositionInFormation(positionInFormation);
            enemy.transform.parent = positionGameObject;
        }
    }

    // Returns the next available position available in the formation
    Transform NextFreePosition() {
        foreach (Transform positionGameObject in gameObject.transform) {
            if (positionGameObject.childCount == 0) {
                return positionGameObject;
            }
        }
        return null;
    }

    // Spawns enemies as long as there is an available position
    void SpawnUntilFull() {
        Transform positionTransform = NextFreePosition();
        if (positionTransform != null) {
            SpawnNewEnemy(positionTransform);
            Debug.Log("New Enemy Spawned");
            Invoke("SpawnUntilFull", spawnDuration);
        }
    }

    // Moves enemy formation left to right
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

    // Checks if all enemies are dead
    // Loops through all Position objects in formation and checks whether there are any child objects (Enemies) remaining
    bool AllEnemiesAreDead() {
        foreach (Transform positionGameObject in gameObject.transform) {
            if (positionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }
}
