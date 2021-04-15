using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    private Text dialog_text;
    private Transform answers_buttons;
    private AudioSource hit_sfx;

    void Start()
    {
        dialog_text = GameObject.Find("dialog-text").GetComponent<Text>();
        answers_buttons = GameObject.Find("answers-buttons").transform;
        hit_sfx = GameObject.Find("hit-sfx").GetComponent<AudioSource>();
    }

    void EnableAnswers()
    {
        for (int i = 0; i < answers_buttons.childCount; i++)
            answers_buttons.GetChild(i).GetComponent<Button>().interactable = true;
    }

    void DisableAnswers()
    {
        for (int i = 0; i < answers_buttons.childCount; i++)
            answers_buttons.GetChild(i).GetComponent<Button>().interactable = false;
    }

    void StartFight()
    {
        DisableAnswers();
        hit_sfx.Play();
    }

    void EnableDialog()
    {
        dialog_text.enabled = true;
    }

    void DisableDialog()
    {
        dialog_text.enabled = false;
    }
}