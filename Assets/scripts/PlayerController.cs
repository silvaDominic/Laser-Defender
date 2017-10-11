using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float thrust = 5.0f;
    public float shipPadding = 1.0f;

    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start () {
        float distanceDiff = gameObject.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftViewPort = Camera.main.ViewportToWorldPoint(new Vector3(0 , 0, distanceDiff));
        Vector3 rightViewPort = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0, distanceDiff));

        xmin = leftViewPort.x + shipPadding;
        xmax = rightViewPort.x - shipPadding;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow)) {
            gameObject.transform.position += new Vector3 (0, thrust * Time.deltaTime, 0);
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            gameObject.transform.position += new Vector3(0, -thrust * Time.deltaTime, 0);
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            gameObject.transform.position += Vector3.left * thrust * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            gameObject.transform.position += Vector3.right * thrust * Time.deltaTime;
        }

        //Restricts players ship to gamespace
        float newX = Mathf.Clamp(gameObject.transform.position.x, xmin, xmax);

        gameObject.transform.position = new Vector3(newX, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
