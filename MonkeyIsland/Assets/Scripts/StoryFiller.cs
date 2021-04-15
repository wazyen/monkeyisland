using UnityEngine.UI;
using UnityEngine;

public static class StoryFiller
{
    [System.Serializable]
    public class Insult
    {
        public string pirate;
        public string guybrush;
    }

    [System.Serializable]
    public class Insults
    {
        public Insult[] insults;
    }

    private static Insults insults;
    private static int n_insults;

    private static int insult_index = -1;
    private static int answer_index = -1;

    private const int MAX_HEARTS = 3;
    private static GameObject player_hearts_container;
    private static GameObject computer_hearts_container;
    private static Image[] player_hearts = new Image[MAX_HEARTS];
    private static Image[] computer_hearts = new Image[MAX_HEARTS];
    private static int player_lives;
    private static int computer_lives;
    private static Sprite heart_empty;

    private static Animator guybrush_animator;
    private static Animator pirate_animator;

    /* For deciding who speaks */
    private const int GUYBRUSH = 0;
    private const int PIRATE = 1;

    public static StoryNode FillStory()
    {
        Initialize();

        StoryNode root = CreateNode(
            "¡Luchemos!",
            new[] {
            "¡Adelante!"});

        StoryNode player_insults = CreateNode(
            "",
            new string[n_insults]);
        player_insults.OnNodeVisited = () =>
        {
            guybrush_animator.SetTrigger("game_started");
            pirate_animator.SetTrigger("game_started");
        };

        StoryNode[] player_insulted = new StoryNode[n_insults];
        StoryNode[] player_answered = new StoryNode[n_insults];

        StoryNode[] computer_insults = new StoryNode[n_insults];
        StoryNode[] computer_answers = new StoryNode[n_insults];

        StoryNode player_wins = CreateNode(
            "¡Está bien, tú ganas! ¡La victoria es tuya!",
            new string[] { "Menú principal",
                           "Jugar otra vez",
                           "Salir" });
        player_wins.IsFinal = true;
        player_wins.OnNodeVisited = () =>
        {
            changeSpeaker(PIRATE);
        };

        StoryNode computer_wins = CreateNode(
            "¡Perdiste! ¡Tendrás que mejorar si quieres ser un digno rival para mí!",
            new string[] { "Menú principal",
                           "Jugar otra vez",
                           "Salir" });
        computer_wins.IsFinal = true;
        computer_wins.OnNodeVisited = () =>
        {
            changeSpeaker(PIRATE);
        };

        for (int i = 0; i < n_insults; i++)
        {
            Insult insult = insults.insults[i];
            // player_insulted
            player_insulted[i] = CreateNode(
                insult.pirate,
                new string[] { "Siguiente" });
            // player_insults
            player_insults.Answers[i] = insult.pirate;
            player_insults.NextNode[i] = player_insulted[i];
            // player_answered
            player_answered[i] = CreateNode(
                insult.guybrush,
                new[] { "Siguiente" });
            // computer_insults
            computer_insults[i] = CreateNode(
                insult.pirate,
                new string[n_insults]);
            // computer_answers
            computer_answers[i] = CreateNode(
                insult.guybrush,
                new[] { "Siguiente" });
        }

        for (int i = 0; i < n_insults; i++)
        {
            int index = i;
            // player_insulted
            player_insulted[i].OnNodeVisited = () =>
            {
                changeSpeaker(GUYBRUSH);
                int computer_answer = (Random.Range(0, 2) == 0 ? index : Random.Range(0, n_insults)); // Let's make the computer win the round the (50% + 50% * 1/16) of times
                player_insulted[index].NextNode[0] = computer_answers[computer_answer];
                insult_index = index;
            };
            // player_answered
            player_answered[i].OnNodeVisited = () =>
            {
                changeSpeaker(GUYBRUSH);
                answer_index = index;
                resolveRound(false, player_answered[index], player_insults, computer_insults, player_wins, computer_wins);
            };
            // computer_insults
            computer_insults[i].OnNodeVisited = () =>
            {
                guybrush_animator.SetTrigger("game_started");
                pirate_animator.SetTrigger("game_started");
                changeSpeaker(PIRATE);
                insult_index = index;
            };
            for (int j = 0; j < n_insults; j++)
            {
                computer_insults[i].Answers[j] = insults.insults[j].guybrush;
                computer_insults[i].NextNode[j] = player_answered[j];
            }
            // computer_answers
            computer_answers[i].OnNodeVisited = () =>
            {
                changeSpeaker(PIRATE);
                answer_index = index;
                GameplayManager.WriteDialog(insults.insults[answer_index].guybrush);
                resolveRound(true, computer_answers[index], player_insults, computer_insults, player_wins, computer_wins);
            };
        }

        root.NextNode[0] = newRound(-1, player_insults, computer_insults);

        return root;
    }

    private static void Initialize()
    {
        player_hearts_container = GameObject.Find("player-hearts");
        computer_hearts_container = GameObject.Find("computer-hearts");
        heart_empty = Resources.Load<Sprite>("Images/heart-empty");

        guybrush_animator = GameObject.Find("guybrush").GetComponent<Animator>();
        pirate_animator = GameObject.Find("pirate").GetComponent<Animator>();

        for (int i = 0; i < MAX_HEARTS; i++)
        {
            player_hearts[i] = player_hearts_container.transform.GetChild(i).gameObject.GetComponent<Image>();
            computer_hearts[i] = computer_hearts_container.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        player_lives = MAX_HEARTS;
        computer_lives = MAX_HEARTS;

        insults = JsonUtility.FromJson<Insults>(Resources.Load<TextAsset>("Text/insults").text);
        n_insults = insults.insults.Length;
    }

    private static StoryNode CreateNode(string history, string[] options)
    {
        var node = new StoryNode
        {
            History = history,
            Answers = options,
            NextNode = new StoryNode[options.Length],
            IsFinal = false
        };
        return node;
    }

    private static StoryNode newRound(int player_won_round, StoryNode player_insults, StoryNode[] computer_insults)
    {
        StoryNode new_round;
        if (player_won_round < 0) // First round, nobody won any round yet
        {
            if (Random.Range(0, 2) == 0)
                new_round = player_insults; // Player starts round
            else
                new_round = computer_insults[Random.Range(0, n_insults)]; // Computer starts round
        }
        else
            new_round = (player_won_round == 0 ? computer_insults[Random.Range(0, n_insults)] : player_insults);
        return new_round;
    }

    private static void resolveRound(bool player_started_round, StoryNode answered, StoryNode player_insults, StoryNode[] computer_insults, StoryNode player_wins, StoryNode computer_wins)
    {
        bool player_won_round = player_started_round ^ (insult_index == answer_index);
        if (player_won_round)
        {
            answered.NextNode[0] = (--computer_lives == 0 ? player_wins : newRound(1, player_insults, computer_insults));
            computer_hearts[computer_lives].sprite = heart_empty;
        }
        else
        {
            answered.NextNode[0] = (--player_lives == 0 ? computer_wins : newRound(0, player_insults, computer_insults));
            player_hearts[player_lives].sprite = heart_empty;
        }
        if (computer_lives == 0 || player_lives == 0)
        {
            guybrush_animator.SetBool("game_ended", true);
            pirate_animator.SetBool("game_ended", true);
        }
        guybrush_animator.SetTrigger("round_ended");
        pirate_animator.SetTrigger("round_ended");
        guybrush_animator.SetBool("won_round", player_won_round);
        pirate_animator.SetBool("won_round", !player_won_round);
    }

    private static void changeSpeaker(int new_speaker)
    {
        GameplayManager.changeSpeaker(new_speaker);
        guybrush_animator.SetTrigger(new_speaker == GUYBRUSH ? "speaks" : "listens");
        pirate_animator.SetTrigger(new_speaker == PIRATE ? "speaks" : "listens");
    }
}