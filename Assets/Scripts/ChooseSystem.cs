using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChooseSystem : MonoBehaviour
{
    public VideoPlayer parentVideoPlayer;
    public Button buttonPrefab;
    public Transform buttonsParent;

    public List<Button> choiceButtons;

    private void Start()
    {
        // Start playing the parent video
        parentVideoPlayer.Play();
        // Subscribe to the event when the parent video ends
        parentVideoPlayer.loopPointReached += OnParentVideoEnd;
    }

    private void OnParentVideoEnd(VideoPlayer source)
    {
        // Enable UI buttons based on the number of children videos
        EnableChoiceButtons();
    }

    private void EnableChoiceButtons()
    {
        // Enable UI buttons dynamically based on the number of children videos
        for (int i = 0; i < parentVideoPlayer.transform.childCount; i++)
        {
            VideoPlayer childVideoPlayer = parentVideoPlayer.transform.GetChild(i).GetComponent<VideoPlayer>();
            if (childVideoPlayer != null)
            {
                Button choiceButton = Instantiate(buttonPrefab, buttonsParent);
                // Set button label or other properties as needed
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = childVideoPlayer.name;
                int index = i; // Capture the index value to use in the lambda expression
                choiceButton.onClick.AddListener(() => OnChoiceButtonClicked(index, childVideoPlayer));
                choiceButtons.Add(choiceButton);
            }
        }
    }

    private void OnChoiceButtonClicked(int choiceIndex, VideoPlayer nextVideo)
    {
        // Stop the current child video
        parentVideoPlayer.Stop();
        parentVideoPlayer.enabled = false;
        parentVideoPlayer = nextVideo;
        // Disable UI buttons
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            Destroy(choiceButtons[i].gameObject);
        }
        choiceButtons.Clear();
        // Enable and play the chosen child video
        parentVideoPlayer.loopPointReached += OnParentVideoEnd;
        parentVideoPlayer.enabled = true;
        parentVideoPlayer.Play();
    }

}