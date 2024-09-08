using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    [SerializeField]private LineRenderer shootLineUI;
    private AudioSource audioSource;
    [SerializeField] private AudioClip launchSound;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //when left click show the ui for the launch scaler
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            StartCoroutine(PullBackShooter());
        }
        
    }

    private Vector3 startPosition;
    [SerializeField] private float maxLineLength;
    [SerializeField] private float lineLengthMultiplier;
    [SerializeField] private TextMeshProUGUI shotCounter;
    private float clampedDistanceFromPlayer;

    IEnumerator PullBackShooter(){
        //buffer start time
        yield return new WaitForSeconds(.15f);
        //set up the line renderer
        shootLineUI.positionCount = 2;

        startPosition = transform.position;

        var positions = new[] { startPosition, startPosition };

        shootLineUI.enabled = true;
        //while holding down change the positions of the line
        while(Input.GetKey(KeyCode.Mouse0)){
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            worldMousePosition.z = 0;

            // Calculate the direction from the target to the mouse
            Vector3 direction = (worldMousePosition - transform.position).normalized;

            // Calculate the opposite direction and the length of the line
            float distanceFromPlayer = Vector3.Distance(worldMousePosition, transform.position);
            clampedDistanceFromPlayer = Mathf.Min(distanceFromPlayer * lineLengthMultiplier, maxLineLength);

            Vector3 oppositeEndPosition = transform.position - direction * clampedDistanceFromPlayer;
            shootLineUI.SetPosition(0, transform.position);
            shootLineUI.SetPosition(1, oppositeEndPosition);
            yield return null;
        }
        //disable line after letting go
        shootLineUI.enabled = false;
        StartCoroutine(LaunchBall());

    }
    [SerializeField] private float maxBallLaunchForce;
    IEnumerator LaunchBall(){
        //launch tiny delay for added effect
        yield return new WaitForSeconds(.15f);
        PlayerManager.Instance.currentStrokes++;
        shotCounter.text = "Current Shots: " +PlayerManager.Instance.currentStrokes;
        audioSource.clip = launchSound;
        if(clampedDistanceFromPlayer/maxLineLength > .1f){
            Vector2 shootDir = (shootLineUI.GetPosition(1) - transform.position).normalized;
            rb.AddForce(shootDir * (maxBallLaunchForce *(clampedDistanceFromPlayer/maxLineLength)), ForceMode2D.Impulse);
            audioSource.Play();
        }
        
        yield return null;
    }
}
