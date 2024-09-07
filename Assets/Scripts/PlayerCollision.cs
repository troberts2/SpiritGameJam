using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D; 
    public SoulShards.SoulShardsType currentType;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "shard"){
            other.GetComponent<Collider2D>().enabled = false;
            CollectShard(other);
        }
        if(other.tag == "colorGate"){
            currentType = other.GetComponent<ColorGate>().changeToType;
            StartCoroutine(ChangeColor());
        }
    }

    IEnumerator ChangeColor(){
        float elapsedTime = 0;
        Color startColor = GetComponent<SpriteRenderer>().color;
        Color endColor = GetComponent<SpriteRenderer>().color;
        if(currentType == SoulShards.SoulShardsType.Angry){
            endColor = Color.red;
        }else if(currentType == SoulShards.SoulShardsType.Sad){
            endColor = Color.blue;
        }else if(currentType == SoulShards.SoulShardsType.Happy){
            endColor = Color.yellow;
        }
        while(elapsedTime < .2f){
            GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, endColor, elapsedTime/.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = endColor;
    }

    private void CollectShard(Collider2D other){
        if(other.transform.localScale.magnitude <= transform.localScale.magnitude && other.GetComponent<SoulShards>().type == currentType){
            if(growPlayer != null) StopCoroutine(growPlayer);
            growPlayer = StartCoroutine(GrowPlayerSizeLight(other));
        }else{
            if(growPlayer !=null ) StopCoroutine(growPlayer);
            StartCoroutine(PlayerFadeOut(other));
        }
        
    }

    Coroutine growPlayer;
    [SerializeField] private float playerGrowthMultiplier;
    IEnumerator GrowPlayerSizeLight(Collider2D other){
        GameManager.Instance.shardList.Remove(other.gameObject);
        //set start values
        float elapsedTime = 0f;
        float playerLightStartIntesity = PlayerManager.Instance.playerLight.intensity;
        Vector3 playerStartScale = transform.localScale;
        Vector3 shardStartScale = other.transform.localScale;
        Vector2 shardStartPos = other.transform.position;

        PlayerManager.Instance.playerFullPercentage += other.GetComponent<SoulShards>().soulPercentage;

        //set end values
        float endLightIntesity = PlayerManager.Instance.playerFullPercentage;
        Vector3 endScale = transform.localScale * playerGrowthMultiplier;
        Vector3 shardEndScale = Vector2.zero;
        //little delay
        yield return new WaitForSeconds(.15f);
        while(elapsedTime < .5f){
            PlayerManager.Instance.playerLight.intensity = Mathf.Lerp(playerLightStartIntesity, endLightIntesity, elapsedTime / .5f);
            transform.localScale = Vector3.Lerp(playerStartScale, endScale, elapsedTime/.5f);
            other.transform.localScale = Vector3.Lerp(shardStartScale, shardEndScale, elapsedTime/.5f);
            other.transform.position = Vector2.Lerp(shardStartPos, transform.position, elapsedTime/.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PlayerManager.Instance.playerLight.intensity = endLightIntesity;
        transform.localScale = endScale;
        Destroy(other.gameObject);
        if(GameManager.Instance.shardList.Count < 1){
            StartCoroutine(PlayerManager.Instance.EndGame());
        }
    }


    [SerializeField] private float shardGrowthMultiplier;
    IEnumerator PlayerFadeOut(Collider2D other){
        PlayerManager.Instance.playerConsumed = true;
        //set start values
        float elapsedTime = 0f;
        float playerLightStartIntesity = PlayerManager.Instance.playerLight.intensity;
        Vector3 shardStartScale = other.transform.localScale;
        Vector3 playerStartScale = transform.localScale;

        //set end values
        float endLightIntesity = 0;
        Vector3 shardEndScale = other.transform.localScale * shardGrowthMultiplier;
        Vector3 playerEndScale = Vector2.zero;
        Vector2 playerEndPos = other.transform.position;
        //little delay
        yield return new WaitForSeconds(.15f);
        Vector2 playerStartPos = transform.position;
        while(elapsedTime < .3f){
            PlayerManager.Instance.playerLight.intensity = Mathf.Lerp(playerLightStartIntesity, endLightIntesity, elapsedTime / .3f);
            other.transform.localScale = Vector3.Lerp(shardStartScale, shardEndScale, elapsedTime/.3f);
            transform.localScale = Vector3.Lerp(playerStartScale, playerEndScale, elapsedTime/.3f);
            transform.position = Vector2.Lerp(playerStartPos, playerEndPos, elapsedTime/.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PlayerManager.Instance.playerLight.intensity = endLightIntesity;
        other.transform.localScale = shardEndScale;
        transform.localScale = playerEndScale;
        //Destroy player after fading light
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        PlayerManager.Instance.EndGameForLoss();
    }
}
