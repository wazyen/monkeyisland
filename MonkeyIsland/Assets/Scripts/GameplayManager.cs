using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private bool GamePaused = false;
    
    private static Text DialogText;
    private GameObject AnswersButtons;
    private GameObject AnswerTextPrefab;

    private StoryNode currentNode;

    /* For deciding who speaks */
    private const int GUYBRUSH = 0;
    private const int PIRATE = 1;

    void Start()
    {
        Initialize();

        currentNode = StoryFiller.FillStory();
        UpdateUI();
    }

    void Initialize()
    {
        DialogText = GameObject.Find("dialog-text").GetComponent<Text>();
        AnswersButtons = GameObject.Find("answers-buttons");
        AnswerTextPrefab = Resources.Load<GameObject>("Prefabs/answers-text");

        DialogText.text = "";
    }
    
    void UpdateUI()
    {
        StoryNode node = currentNode;
        WriteDialog(node.History);
        EmptyAnswers();

        int i = 0;
        foreach (string answer in node.Answers)
        {
            GameObject NewButton = Instantiate(AnswerTextPrefab, AnswersButtons.transform, true);
            SetButtonText(NewButton, answer);
            PlaceButton(NewButton, i);
            SetButtonClickListener(NewButton, i, node.IsFinal);
            i++;
        }
        ResizeButtonsContainer(node.Answers.Length);
        ScrollerButton.scrollToTop();
    }
    
    public static void WriteDialog(string text)
    {
        DialogText.text = text;
    }
    
    void EmptyAnswers()
    {
        foreach (Transform answer in AnswersButtons.transform)
            Destroy(answer.gameObject);
    }
    
    void SetButtonText(GameObject Button, string text)
    {
        Button.GetComponent<Text>().text = text;
    }
    
    void PlaceButton(GameObject Button, int button_index)
    {
        RectTransform rt = Button.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0, -button_index * 18, 0);
        rt.offsetMin = new Vector2(0, rt.offsetMin.y);
        rt.offsetMax = new Vector2(0, rt.offsetMax.y);
    }
    
    void SetButtonClickListener(GameObject Button, int answer_index, bool isFinal)
    {
        if (isFinal)
        {
            if      (Button.GetComponent<Text>().text == "Menú principal")
                Button.GetComponent<Button>().onClick.AddListener(() => { gameObject.GetComponent<SceneController>().ReturnToMainMenu(); });
            else if (Button.GetComponent<Text>().text == "Jugar otra vez")
                Button.GetComponent<Button>().onClick.AddListener(() => { gameObject.GetComponent<SceneController>().StartNewGame(); });
            else if (Button.GetComponent<Text>().text == "Salir")
                Button.GetComponent<Button>().onClick.AddListener(() => { gameObject.GetComponent<SceneController>().ExitGame(); });
        }
        else
            Button.GetComponent<Button>().onClick.AddListener(() => { SelectAnswer(answer_index); });
    }

    void SelectAnswer(int answer_index)
    {
        if (GamePaused)
            return;
        Debug.Log("Selecting Answer...");
        currentNode = currentNode.NextNode[answer_index];
        if (currentNode.OnNodeVisited != null)
            currentNode.OnNodeVisited.Invoke();
        UpdateUI();
    }

    void ResizeButtonsContainer(int n_buttons)
    {
        RectTransform rt = AnswersButtons.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, n_buttons * 18);
    }

    public static void changeSpeaker(int speaker)
    {
        DialogText.color = ( speaker == GUYBRUSH ? Color.white : Color.green);
    }
}