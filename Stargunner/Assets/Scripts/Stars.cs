using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float amtToMove = speed * Time.deltaTime;
        transform.Translate(Vector3.left * amtToMove);

        if (transform.position.x <= -18.8f)
        {
            transform.position = new Vector3(18.8f, transform.position.y, transform.position.z);
        }
	}
}
