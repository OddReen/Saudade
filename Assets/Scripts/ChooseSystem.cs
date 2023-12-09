using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChooseSystem : MonoBehaviour
{
    VideoPlayer firstVideo;
    public VideoPlayer parentVideoPlayer;
    public Button buttonPrefab;
    public Transform buttonsParent;

    public List<Button> choiceButtons;

    private void Start()
    {
        firstVideo = parentVideoPlayer;
        parentVideoPlayer.Play();
        parentVideoPlayer.loopPointReached += OnParentVideoEnd;
    }

    private void OnParentVideoEnd(VideoPlayer source)
    {
        EnableChoiceButtons();
    }

    private void EnableChoiceButtons()
    {
        //On Video End
        if (parentVideoPlayer.transform.childCount == 0)
        {
            Button choiceButton = Instantiate(buttonPrefab, buttonsParent);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = "Go Back";
            choiceButton.onClick.AddListener(() => GoBack(firstVideo));
            choiceButtons.Add(choiceButton);
        }
        for (int i = 0; i < parentVideoPlayer.transform.childCount; i++)
        {
            VideoPlayer childVideoPlayer = parentVideoPlayer.transform.GetChild(i).GetComponent<VideoPlayer>();
            if (childVideoPlayer != null)
            {
                Button choiceButton = Instantiate(buttonPrefab, buttonsParent);
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = childVideoPlayer.name;
                choiceButton.onClick.AddListener(() => NextVideo(childVideoPlayer));
                choiceButtons.Add(choiceButton);
            }
        }
    }

    private void NextVideo(VideoPlayer nextVideo)
    {
        parentVideoPlayer.Stop();
        parentVideoPlayer.enabled = false;
        parentVideoPlayer = nextVideo;
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            Destroy(choiceButtons[i].gameObject);
        }
        choiceButtons.Clear();
        parentVideoPlayer.loopPointReached += OnParentVideoEnd;
        parentVideoPlayer.enabled = true;
        parentVideoPlayer.Play();
    }
    private void GoBack(VideoPlayer nextVideo)
    {
        parentVideoPlayer.Stop();
        parentVideoPlayer.enabled = false;
        parentVideoPlayer = nextVideo;
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            Destroy(choiceButtons[i].gameObject);
        }
        choiceButtons.Clear();
        parentVideoPlayer.enabled = true;
        parentVideoPlayer.Play();
    }

}