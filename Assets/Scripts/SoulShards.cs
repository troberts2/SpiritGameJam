using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulShards : MonoBehaviour
{
    public float soulPercentage;
    [SerializeField] float timeTillSoulDepletes;
    public enum SoulShardsType{
        none
    }
    public SoulShardsType type;

    private void Start() {
        StartCoroutine(DepleteShard(timeTillSoulDepletes));
    }

    IEnumerator DepleteShard(float time){
        float elapsedTime = 0;
        float soulPercentageStart = soulPercentage;
        while (elapsedTime < time) {
            soulPercentage = Mathf.Lerp(soulPercentageStart, 0, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
        GameManager.Instance.shardList.Remove(gameObject);
        if(GameManager.Instance.shardList.Count < 1) {
            PlayerManager.Instance.EndGameForLoss();
        }
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
