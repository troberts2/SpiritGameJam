using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public float playerFullPercentage;
    public Light2D playerLight;

    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;
    [SerializeField] TextMeshProUGUI topText;
    [SerializeField] GameObject nextLevelButton;
    public int levelParAmount;
    public int currentStrokes = 0;
    public bool playerConsumed = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EndGameForLoss(){
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame(){
        //lil delay
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        winPanel.SetActive(true);
        if(!playerConsumed){
            if(currentStrokes <= levelParAmount - 1){
                //3 star win
                topText.text = "Wow, above par!?";
                StartCoroutine(LerpStarAlpha(star1));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star2));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star3));
            }else if(currentStrokes == levelParAmount){
                //2 star win
                topText.text = "Just made par!";
                StartCoroutine(LerpStarAlpha(star1));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star2));
            }else if(currentStrokes == levelParAmount + 1){
                //1 star win
                topText.text = "Didn't make par!";
                StartCoroutine(LerpStarAlpha(star1));
            }else if(currentStrokes >= levelParAmount + 2){
                topText.text = "Not even close to par!";
                //0 star win
            }else{
            topText.text = "Big L";
            //loss
            nextLevelButton.SetActive(false);
            }
        }
        else{
            topText.text = "Big L";
            //loss
            nextLevelButton.SetActive(false);
        }
        yield return null;
    }

    IEnumerator LerpStarAlpha(GameObject star){
        float elapsedTime = 0f;
        Image starImage = star.GetComponent<Image>();
        var tempColor = starImage.color;

        while(elapsedTime < .2f){
            tempColor.a = Mathf.Lerp(80, 255, elapsedTime / .2f);
            starImage.color = tempColor;
            elapsedTime+= Time.deltaTime;
            yield return null;
        }
        tempColor.a = 255;
        starImage.color = tempColor;
    }
}
