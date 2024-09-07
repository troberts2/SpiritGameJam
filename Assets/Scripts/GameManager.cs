using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> shardList = new List<GameObject>();
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        
    }
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnSceneLoaded(Scene sceneManager, LoadSceneMode loadSceneMode){
        List <SoulShards> shards = FindObjectsOfType<SoulShards>().ToList();
        foreach(SoulShards shard in shards){
            shardList.Add(shard.gameObject);
        }
        
    }
}
