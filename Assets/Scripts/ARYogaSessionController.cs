using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ARYogaSessionController : MonoBehaviour
{
    public GameObject yogaTutorPrefab;
    private GameObject spawnedObject;
    public GameObject endScreen;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public Button deployButton, startButton, pauseButton, resetButton;
    public Text timerText;

    //public WorkoutTimeDisplay workoutTime;
    public Storage storage;

    public AudioSource yogaInstructionsAudio;
    private Animator yogaTutorAnimator;
    private bool isPaused = false, sessionStarted = false, canPlaceObject = false, uiInteraction = false;

    private float sessionTime = 60f;
    public float exerciseTime = 0f;

    void Start()
    {
        storage = GameObject.FindObjectOfType<Storage>();

        raycastManager = GetComponent<ARRaycastManager>();

        deployButton.onClick.AddListener(OnDeployButtonClicked);
        startButton.onClick.AddListener(StartSession);
        pauseButton.onClick.AddListener(PauseSession);
        resetButton.onClick.AddListener(OnResetButtonClicked);

        resetButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        //endScreen = GameObject.Find("End Screen");
        endScreen.SetActive(false);
    }

    void Update()
    {
        HandleARObjectPlacement();

        if (sessionStarted && !isPaused)
        {
            if (sessionTime > 0)
            {
                sessionTime -= Time.deltaTime;
                exerciseTime += Time.deltaTime;
                timerText.text = "Time left: " + Mathf.FloorToInt(sessionTime).ToString();
            }
            else
            {
                EndSession();
            }
        }
    }

    void HandleARObjectPlacement()
    {
        if (Input.touchCount > 0 && canPlaceObject && !uiInteraction)
        {
            Touch touch = Input.GetTouch(0);

            if (IsTouchOverUI(touch)) return;

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(yogaTutorPrefab, hitPose.position, hitPose.rotation);
                    yogaTutorAnimator = spawnedObject.GetComponent<Animator>();
                    yogaTutorAnimator.speed = 0;

                    startButton.gameObject.SetActive(true);
                    resetButton.gameObject.SetActive(true);

                    canPlaceObject = false;
                }
            }
        }
    }

    void OnDeployButtonClicked()
    {
        canPlaceObject = true;
        deployButton.gameObject.SetActive(false);
        uiInteraction = true;
        StartCoroutine(ResetUIInteraction());
    }

    void OnResetButtonClicked()
    {
        if (spawnedObject != null) Destroy(spawnedObject);

        sessionStarted = false;
        sessionTime = 60f;
        deployButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        timerText.text = "";
    }

    void StartSession()
    {
        if (yogaTutorAnimator == null) return;

        exerciseTime = 0;
        yogaTutorAnimator.speed = 1;
        StartCoroutine(PlayAudioWithDelay(15f));
        startButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        sessionStarted = true;
        isPaused = false;
    }

    IEnumerator PlayAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yogaInstructionsAudio.Play();
    }

    void PauseSession()
    {
        isPaused = !isPaused;
        yogaTutorAnimator.speed = isPaused ? 0 : 1;

        if (isPaused) yogaInstructionsAudio.Pause();
        else yogaInstructionsAudio.UnPause();
    }
      
    void EndSession()
    {
        yogaTutorAnimator.speed = 0;
        yogaInstructionsAudio.Stop();

        storage.dailyTime += exerciseTime;
        storage.totalTime += exerciseTime;
        //workoutTime.UpdateDisplayTime();

        sessionStarted = false;
        sessionTime = 60f;

        endScreen.SetActive(true);
    }

    bool IsTouchOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    IEnumerator ResetUIInteraction()
    {
        yield return new WaitForSeconds(0.2f);
        uiInteraction = false;
    }
}