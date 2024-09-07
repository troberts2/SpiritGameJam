using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<SoulShards> shardList = new List<SoulShards>();
    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        shardList = FindObjectsOfType<SoulShards>().ToList();
    }
}
