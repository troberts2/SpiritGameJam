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
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            PauseGame();
        }
    }
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    private void PauseGame(){
        PlayerManager.Instance.gameObject.GetComponent<PlayerMovement>().enabled = false;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }
    public void ResumeGame(){
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        Invoke(nameof(EnablePlayerMovement), .1f);
    }
    private void EnablePlayerMovement(){
        PlayerManager.Instance.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }
    public void Options(){
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
    }
    public void BackOptions(){
        optionsUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    void OnSceneLoaded(Scene sceneManager, LoadSceneMode loadSceneMode){
        List <SoulShards> shards = FindObjectsOfType<SoulShards>().ToList();
        foreach(SoulShards shard in shards){
            shardList.Add(shard.gameObject);
        }
        
    }
}
