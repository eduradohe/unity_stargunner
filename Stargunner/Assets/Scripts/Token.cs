using UnityEngine;
using System.Collections;

public class Token : MonoBehaviour {

    private int horizontalSpeed;
    private int verticalSpeed;
    private TokenType tokenType;

    private float blinkController;
    private const int blinkRate = 10;
    private bool finalSeconds;

    public const int min = 20;
    private const float lifeSpan = 7f;
    private const float blinkPeriod = 1.5f;

	// Use this for initialization
	void Start () {
        horizontalSpeed = Random.Range(-2, 3);
        verticalSpeed = Random.Range(-2, 3);
        finalSeconds = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (blinkController > 0)
        {
            this.blinkController -= (blinkRate * Time.deltaTime);
        }

        if ((finalSeconds && blinkController <= 0))
        {
            renderer.enabled = !renderer.enabled;
            this.blinkController = 1;
        }

        StartCoroutine(ControlLifeSpan());

        float posX = horizontalSpeed * Time.deltaTime;
        float posY = verticalSpeed * Time.deltaTime;

        transform.Translate(posX, posY, 0);
	}

    public TokenType GetTokenType()
    {
        return this.tokenType;
    }

    IEnumerator ControlLifeSpan()
    {
        yield return new WaitForSeconds(lifeSpan - blinkPeriod);
        StartCoroutine(MakeBlink());
    }

    IEnumerator MakeBlink()
    {
        finalSeconds = true;
        yield return new WaitForSeconds(blinkPeriod);
        Destroy(gameObject);
    }

    void UpdateMaterial()
    {
        switch (tokenType) {
            case TokenType.Score:
                renderer.material.color = Color.gray;
                break;
            case TokenType.SpeedBoost:
                renderer.material.color = Color.cyan;
                break;
            case TokenType.EarthReinforcement:
                renderer.material.color = Color.blue;
                break;
            case TokenType.Shield:
                renderer.material.color = Color.yellow;
                break;
            case TokenType.ShotUpgrade:
                renderer.material.color = Color.green;
                break;
            case TokenType.Bomb:
                renderer.material.color = Color.magenta;
                break;
            case TokenType.Life:
                renderer.material.color = Color.red;
                break;
            case TokenType.Invulnerability:
                renderer.material.color = new Color(114f / 255f, 0f, 61f / 61f, 1f);
                break;
        }
    }

    public void SetType(int randomInt)
    {
        if (randomInt >= 100 && randomInt < 200) {
            tokenType = TokenType.Score;
        }
        else if (randomInt >= 200 && randomInt < 450)
        {
            tokenType = TokenType.EarthReinforcement;
        }
        else if (randomInt >= 450 && randomInt < 650)
        {
            tokenType = TokenType.Shield;
        }
        else if (randomInt >= 650 && randomInt < 850)
        {
            tokenType = TokenType.SpeedBoost;
        }
        else if (randomInt >= 850 && randomInt < 950)
        {
            tokenType = TokenType.ShotUpgrade;
        }
        else if (randomInt >= 950 && randomInt < 980)
        {
            tokenType = TokenType.Bomb;
        }
        else if (randomInt >= 980 && randomInt < 995)
        {
            tokenType = TokenType.Life;
        }
        else if (randomInt >= 995)
        {
            tokenType = TokenType.Invulnerability;
        }
        UpdateMaterial();
    }
}
