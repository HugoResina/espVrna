using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewNpc : MonoBehaviour
{
    [Header("NPC Data")]
    [Tooltip("Scriptable Object containing NPC dialogues")]
    [SerializeField] private NpcSO npcSO;

    [Header("UI Elements")]
    [Tooltip("UI Text component to display dialogues")]
    [SerializeField] private TextMeshProUGUI UIText;

    [Tooltip("Parent transform for answer buttons")]
    [SerializeField] private Transform UIAnswersParent;

    [Tooltip("Button prefab for answers")]
    [SerializeField] private Button UIAnswerButton;

    [Tooltip("Button for proceeding to the next dialogue")]
    [SerializeField] private Button UINextButton;

    private List<TextSO> dialogues;
    private string currentId;
    private int currentIndex = 0;
    private bool isFinished = false;

    private void Awake()
    {
        dialogues = npcSO.Dialogues;

        currentId = dialogues.Count > 0 ? dialogues[0].Id : null;

        StartDialogue();
    }

    private void StartDialogue()
    {
        if (dialogues[0].Text != null)
        {
            UIText.text = dialogues[0].Text;

            UINextButton.gameObject.SetActive(true);
            UINextButton.onClick.AddListener(() =>
            {
                UINextButton.gameObject.SetActive(false);
                ShowText();
            });
        }
        else
        {
            UIText.text = "No dialogue available.";
            Debug.LogError("No dialogue available.");
        }
    }

    private void ShowText()
    {
        if (currentId != null)
        {
            SetCurrentIndexById();

            if (dialogues[currentIndex] is not AnswerSO)
            {
                UIText.text = dialogues[currentIndex].Text;

                if (dialogues[currentIndex] is SentenceSO)
                {
                    if (!isFinished)
                    {
                        UINextButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("Dialogue finished.");
                    }
                }
                else if (dialogues[currentIndex] is QuestionSO)
                {
                    int qIndex = currentIndex;

                    QuestionSO question = dialogues[qIndex] as QuestionSO;

                    foreach (string id in question.AnswerIds)
                    {
                        currentId = id;
                        SetCurrentIndexById();

                        int aIndex = currentIndex;

                        //AnswerSO answer = dialogues[aIndex] as AnswerSO;

                        Button button = Instantiate(UIAnswerButton, UIAnswersParent);
                        button.GetComponentInChildren<TextMeshProUGUI>().text = dialogues[aIndex].Text;

                        button.onClick.AddListener(() =>
                        {
                            currentId = ((AnswerSO)dialogues[aIndex]).NextId;

                            foreach (Transform child in UIAnswersParent)
                            {
                                if (child.gameObject != UIAnswerButton.gameObject)
                                {
                                    Destroy(child.gameObject);
                                }
                            }

                            ShowText();
                        });
                    }
                }
            }
        }
        else
        {
            Debug.Log("Current id is null");
        }
    }

    private void SetCurrentIndexById()
    {
        int i = currentIndex + 1;
        bool found = false;

        while (i < dialogues.Count && !found)
        {
            if (IsFinished(dialogues[i].Id) || currentId.Equals(dialogues[i].Id)/* || IsConvergent(currentId, dialogues[i].Id)*/)
            {
                found = true;
                currentIndex = i;
            }

            i++;
        }
    }

    private bool IsConvergent(string id1, string id2)
    {
        int Id1 = int.Parse(id1);
        int Id2 = int.Parse(id2);

        return (Id1 - Id2) == int.Parse(id1[id1.Length - 1].ToString());
    }

    private bool IsFinished(string id)
    {
        if (id.Length <= 2)
            return false;

        isFinished = id[^2..] == "00";
        return isFinished;
    }
}
