using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int minSpeed;
    public int maxSpeed;
    public int minScale;
    public int maxScale;
    public GameObject explosion;
    public GameObject tokenPrefab;

    private float scale;
    private int shield;
    private int speed;
    private int rotSpeed;
    private int rotSpeedCoeficient;
    private Vector3 rotDirection;

    private const float scaleMultiplier = 0.5f;
    private const int shieldCoeficient = 10;

    void Start()
    {
        rotSpeedCoeficient = 10;
        ResetPosition();
    }

	// Update is called once per frame
	void Update () {
        float amtToMove = speed * Time.deltaTime;
        transform.Translate((Vector3.left * amtToMove), Space.World);
        transform.Rotate(this.rotDirection * this.rotSpeed * Time.deltaTime);

        if (transform.position.x <= -11) {
            Player.earthAttackRate++;
            ResetPosition();
        }
        
	}

    public void ResetPosition()
    {
        float posX = 11f;
        float posY = Random.Range(7.3f, -5.2f);
        float posZ = 0f;
        
        this.scale = Random.Range(minScale, maxScale + 1);
        this.shield = (int)(scale * scaleMultiplier * shieldCoeficient);
        gameObject.transform.position = new Vector3(posX, posY, posZ);
        gameObject.transform.localScale = new Vector3(scale * scaleMultiplier, scale * scaleMultiplier, scale * scaleMultiplier);

        if (this.scale > ((maxScale + minScale) / 2))
        {
            this.speed = Random.Range(minSpeed, ((minSpeed + maxSpeed) / 2));
        }
        else
        {
            this.speed = Random.Range(((minSpeed + maxSpeed) / 2), maxSpeed);
        }
        this.rotSpeed = Random.Range(minSpeed, maxSpeed) * rotSpeedCoeficient;
        this.rotDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }

    public void HitEnemy(int hits)
    {
        this.shield -= hits;
        if (this.shield <= 0) {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Player.score += 1000;
        int randomInt = Random.Range(0, 1000);
        if (randomInt >= Token.min) {
            GameObject tokenObj = (GameObject)Instantiate(tokenPrefab, transform.position, Quaternion.identity);
            Token token = (Token) tokenObj.GetComponent("Token");
            token.SetType(randomInt);
        }
        ResetPosition();
    }

    public int GetImpactPower()
    {
        return this.shield * 2;
    }

    void OnParticleCollision(GameObject otherObject)
    {
        if (otherObject.tag == "bomb") {
            this.HitEnemy(50);
        }
    }
}
