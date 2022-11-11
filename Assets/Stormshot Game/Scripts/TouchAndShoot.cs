using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndShoot : MonoBehaviour {

    public float speed = 1.5f;
    private Vector3 mouseClickPosition;

	Rigidbody2D rb;

	Touch touch;
	Vector3 touchPosition, whereToMove;

	float previousDistanceToTouchPos, currentDistanceToTouchPos;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {

		if (Input.touchCount > 0) {
            // The screen has been touched so store the touch
            Touch touch = Input.GetTouch (0);
 
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 10));            
                transform.position = touchPosition;
            }
 
            
        }

        if (Input.GetMouseButton(0))
        {
            mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseClickPosition.z = transform.position.z;
        }

        transform.position = Vector3.MoveTowards(transform.position, mouseClickPosition, speed);
    }
}
