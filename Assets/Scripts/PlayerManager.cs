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
        Debug.Log("end called");
        yield return new WaitForSeconds(1f);
        winPanel.SetActive(true);
        if(!playerConsumed){
            if(playerFullPercentage > .5f){
                //3 star win
                topText.text = "wow nice man!";
                StartCoroutine(LerpStarAlpha(star1));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star2));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star3));
            }else if(playerFullPercentage > .3f){
                //2 star win
                topText.text = "almost awesome";
                StartCoroutine(LerpStarAlpha(star1));
                yield return new WaitForSeconds(.5f);
                StartCoroutine(LerpStarAlpha(star2));
            }else if(playerFullPercentage > .1f){
                //1 star win
                topText.text = "one stars better than none";
                StartCoroutine(LerpStarAlpha(star1));
            }else if(playerFullPercentage > 0){
                topText.text = "at least you collected a soul";
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
