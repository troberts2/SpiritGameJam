using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D; 
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
            CollectShard(other);
        }
    }

    private void CollectShard(Collider2D other){
        PlayerManager.Instance.playerFullPercentage += other.GetComponent<SoulShards>().soulPercentage;
        PlayerManager.Instance.playerLight.intensity += other.GetComponent<SoulShards>().soulPercentage;
        Destroy(other.gameObject);
    }
}
