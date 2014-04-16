using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {
	
	public int speed;
    public int power;
    public GameObject shotImpact;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float amtToMove = speed * Time.deltaTime;
		transform.Translate(Vector3.right * amtToMove);
		if (transform.position.x >= 14) {
			Destroy(gameObject);
		}
	}

    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "enemy")
        {
            Enemy enemy = (Enemy) otherObject.gameObject.GetComponent("Enemy");
            Player.score += 100;
            enemy.HitEnemy(power);
            Instantiate(shotImpact, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SuperShot() {
        this.power = this.power * 5;
        renderer.material.color = Color.red;
        transform.localScale = new Vector3(0.75f, 0.2f, 0.2f);
    }
}
