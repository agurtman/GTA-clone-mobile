using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject dialogue;
    public DialogueNode[] node;
    [SerializeField] Text npc;
    [SerializeField] Text[] textButtons;
    [SerializeField] GameObject[] buttons;
    [SerializeField] public List<GameObject> answerButtons = new List<GameObject>();
    [SerializeField] GameObject colorTarget;
    [System.NonSerialized] public GameObject target;
    bool isInstantiate;
    public int currentNode;

    public void Awake()
    {
        npc = GameObject.FindGameObjectWithTag("NPCtext").GetComponent<Text>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue");
        buttons = GameObject.FindGameObjectsWithTag("QuestButton");
        textButtons = new Text[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            textButtons[i] = buttons[i].transform.GetChild(0).GetComponent<Text>();
        }
    }

    private void Start()
    {
        dialogue.SetActive(false);
    }

    public void AnswerClicked(int button)
    {
        if (node[currentNode].PlayerAnswer[button].SpeakEnd)
        {
            dialogue.SetActive(false);
        }

        if (node[currentNode].PlayerAnswer[button].questValue > 0)
        {
            PlayerPrefs.SetInt(node[currentNode].PlayerAnswer[button].questName,
                    node[currentNode].PlayerAnswer[button].questValue);
        }

        if (node[currentNode].PlayerAnswer[button].getMoney > 0)
        {
            FindObjectOfType<PlayerController>().GetMoney(node[currentNode].PlayerAnswer[button].getMoney);
        }

        if (node[currentNode].PlayerAnswer[button].target != null)
        {
            if (!isInstantiate)
            {
                target = Instantiate(colorTarget);
                isInstantiate = true;
            }
            target.transform.position = node[currentNode].PlayerAnswer[button].target.transform.position;
        }
        if (node[currentNode].PlayerAnswer[button].destroyTarget)
        {
            Destroy(target);
        }

        currentNode = node[currentNode].PlayerAnswer[button].ToNode;
        Refresh();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(true);
            currentNode = 0;
            Refresh();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(false);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].SetActive(false);
        }
        answerButtons.Clear();
        npc.text = node[currentNode].NpcText;
        for (int i = 0; i < node[currentNode].PlayerAnswer.Length; i++)
        {
            buttons[i].SetActive(false);
            if (node[currentNode].PlayerAnswer[i].questName == "" ||
                    node[currentNode].PlayerAnswer[i].needQuestValue ==
                        PlayerPrefs.GetInt(node[currentNode].PlayerAnswer[i].questName))
            {
                answerButtons.Add(buttons[i]);
                textButtons[i].text = node[currentNode].PlayerAnswer[i].Text;
            }
        }
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].SetActive(true);
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    public string NpcText;
    public Answer[] PlayerAnswer;
}

[System.Serializable]
public class Answer
{
    public string Text;
    public int ToNode;
    public int questValue;
    public int needQuestValue;
    public string questName;
    public int getMoney;
    public bool SpeakEnd;
    public bool destroyTarget;
    public GameObject target;
}