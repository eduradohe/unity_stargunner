using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int speed;
	public int shotRate;
    public Collider shotPrefab;
    public GameObject enemyExplosion;
    public GameObject playerExplosion;
    public GameObject bombExplosion;

    public static int life;
	public static int score;
    public static int earthAttackRate;

    private const int initialLife = 3;
    private const int initialScore = 0;
    private const int initialEarthAttackRate = 1;
    private const int rotSpeed = 50;

    private int shield;
    private int bombs;
    private int earthStand;
    private int impactPower;

    private float shotController;
    private float blinkController;
    private float damageController;
    private float earthStandController;

    private bool blinking;
    private bool invulnerable;
    private int superShot;
    private bool unableToMove;

    private const int blinkRate = 10;
    private const int damageRate = 10;

    private const float superShotDuration = 7.5f;
    private const float invulnerabilityDuration = 7.5f;
    private const float blinkDuration = 1.5f;
    private const int maxEarthStand = 2000000;
    private const int earthAttackRateMultiplier = 23;

    public const int left = -1;
    public const int right = 1;
    public const int up = 1;
    public const int down = -1;
    public const bool horizontal = true;
    public const bool vertical = false;

    public static Vector3 playerInitialPos = new Vector3(-10, 1, 0);

	// Use this for initialization
	void Start () {
        InitPlayer();

        this.shield = 100;
        this.bombs = 2;
        this.earthStand = maxEarthStand;
        this.impactPower = 10;

        this.blinking = false;
        this.invulnerable = false;
        this.superShot = 0;
        this.unableToMove = false;

        this.shotController = 0;
        this.blinkController = 0;
        this.damageController = 0;

        StartCoroutine(TriggerRePresent());
	}

    void EndGame()
    {
        Application.LoadLevel(2);
    }

	// Update is called once per frame
	void Update () {

        if (earthStandController > 0)
        {
            this.earthStandController -= (earthAttackRate * Time.deltaTime);
        }
        if (shotController > 0)
        {
            this.shotController -= (shotRate * Time.deltaTime);
        }
        if (blinkController > 0)
        {
            this.blinkController -= (blinkRate * Time.deltaTime);
        }
        if (damageController > 0)
        {
            this.damageController -= (damageRate * Time.deltaTime);
        }

        if (earthStandController <= 0)
        {
            this.earthStand -= (earthAttackRate * earthAttackRateMultiplier);
            if (this.earthStand <= 0) {
                EndGame();
            }
            this.earthStandController = 1;
        }

        if ((blinking && blinkController <= 0) || (!blinking && !renderer.enabled))
        {
            renderer.enabled = !renderer.enabled;
            this.blinkController = 1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (this.bombs > 0) {
                this.bombs--;
                Instantiate(this.bombExplosion, transform.position, transform.rotation);
            }
        }

		if (Input.GetButton("Fire1")) {
			if (shotController <= 0) {
                Vector3 position = new Vector3(transform.position.x + (gameObject.transform.localScale.x / 2), transform.position.y);
                Collider shotObj = (Collider) Instantiate(this.shotPrefab, position, Quaternion.identity);
                if (this.superShot > 0) {
                    Shot shot = (Shot) shotObj.GetComponent("Shot");
                    shot.SuperShot();
                }
				this.shotController = 1;
			}
		}

        if (unableToMove)
        {
            if (transform.position.x < -8)
            {
                int rePresentSpeed = 2;
                transform.Translate((Vector3.right * rePresentSpeed * Time.deltaTime), Space.World);
            }
            else
            {
                unableToMove = false;
            }
        }
        else
        {
            float posX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float posY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            transform.Translate(posX, posY, 0, Space.World);
            transform.Rotate(new Vector3(0, Input.GetAxis("Vertical"), 0) * rotSpeed * Time.deltaTime);
        }
	}
	
	void OnGUI () {
		GUI.Label(new Rect(5, 5, 200, 20), "Score: " + score);
		GUI.Label(new Rect(5, 25, 200, 20), "Shield: " + shield);
        GUI.Label(new Rect(5, 45, 200, 20), "Life: " + life);
        GUI.Label(new Rect(5, 65, 200, 20), "Bombs: " + bombs);
        GUI.Label(new Rect(5, 85, 200, 20), "Earth Stand: " + earthStand);
	}

    void OnTriggerEnter(Collider otherObject)
    {
        if ((otherObject.gameObject.tag == "enemy") && (invulnerable || !blinking))
        {
            Enemy enemy = (Enemy) otherObject.gameObject.GetComponent("Enemy");
            enemy.HitEnemy(this.impactPower);
            if (!invulnerable)
            {
                TakeDamage(enemy.GetImpactPower());
            }
        }
        if (otherObject.gameObject.tag == "token")
        {
            Token token = (Token) otherObject.gameObject.GetComponent("Token");
            TokenType type = token.GetTokenType();
            switch (type)
            {
                case TokenType.Score:
                    score += 1000;
                    IncrementEarthStand(1000);
                    break;
                case TokenType.SpeedBoost:
                    if (this.speed < 15)
                    {
                        speed++;
                    }
                    break;
                case TokenType.EarthReinforcement:
                    IncrementEarthStand(10000);
                    if (earthAttackRate > 1)
                    {
                        earthAttackRate--;
                    }
                    score += 1000;
                    break;
                case TokenType.Shield:
                    this.shield = Mathf.Min(100, (this.shield + 50));
                    break;
                case TokenType.ShotUpgrade:
                    if (this.shotRate < 10)
                    {
                        this.shotRate++;
                    }
                    else
                    {
                        StartCoroutine(TriggerSuperShot());
                    }
                    break;
                case TokenType.Bomb:
                    this.bombs++;
                    break;
                case TokenType.Life:
                    life++;
                    break;
                case TokenType.Invulnerability:
                    StartCoroutine(TriggerInvulnerability());
                    break;
            }
            Destroy(otherObject.gameObject);
        }
    }

    void OnTriggerStay(Collider otherObject)
    {
        if ((otherObject.tag == "enemy") && (invulnerable || !blinking))
        {
            if (damageController <= 0)
            {
                Enemy enemy = (Enemy)otherObject.gameObject.GetComponent("Enemy");
                enemy.HitEnemy(this.impactPower);
                if (!invulnerable)
                {
                    TakeDamage(enemy.GetImpactPower());
                }
                this.damageController = 1;
            }
        }
    }

    void IncrementEarthStand(int increment)
    {
        this.earthStand = Mathf.Min(maxEarthStand, (this.earthStand + increment));
    }

    IEnumerator TriggerSuperShot()
    {
        this.superShot++;
        yield return new WaitForSeconds(superShotDuration);
        this.superShot--;
    }

    IEnumerator TriggerInvulnerability()
    {
        invulnerable = true;
        renderer.material.color = new Color(114f/255f, 0f, 61f/61f, 1f);
        yield return new WaitForSeconds(invulnerabilityDuration - blinkDuration);
        StartCoroutine(TriggerBlinking());
    }

    IEnumerator TriggerBlinking()
    {
        blinking = true;
        yield return new WaitForSeconds(blinkDuration);
        blinking = false;
        invulnerable = false;
        renderer.material.color = Color.blue;
    }

    IEnumerator TriggerRePresent()
    {
        unableToMove = true;
        return TriggerBlinking();
    }

    void TakeDamage(int damage)
    {
        this.shield -= damage;
        if (this.shield <= 0)
        {
            StartCoroutine(DestroyShip());
        }
    }

    IEnumerator DestroyShip()
    {
        life--;
        if (life <= 0) {
            EndGame();
        }
        if (shotRate > 2)
        {
            shotRate--;
        }
        if (speed > 5)
        {
            speed--;
        }
        
        Instantiate(playerExplosion, transform.position, transform.rotation);
        this.shield = 100;
        transform.position = playerInitialPos;
        return TriggerRePresent();
    }

    private static void InitPlayer()
    {
        life = initialLife;
        score = initialScore;
        earthAttackRate = initialEarthAttackRate;
    }
}
