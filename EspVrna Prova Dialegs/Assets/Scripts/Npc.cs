using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    /* Cada Npc t? un conjunt de frases, preguntes i respostes amb ids
     * 
     * Llegenda de ids:
     *      el id es string numerics, comen?a en 1
     *      el id de cada resposta es la concatenacio del id de la pregunta i un digit que indica la resposta concreta (1, 2, 3...)
     *      el id de una resposta a una frase o pregunta; o de una frase a una pregunta es el mateix
     *      si dues respostes duuen a la mateixa frase, aquesta es considera convergent i substitueix l'ultim digit del id per un 0
     *      si una frase es la ultima del di?leg, el seu id acaba en 00
     */

    [Header("NPC Data")]
    [Tooltip("Scriptable Object containing NPC dialogues")]
    [SerializeField] private NpcSO npcSO; // Contains all Npc data

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

    /// <summary>
    /// Initializes the dialogue system by setting up the initial dialogue state and starting the dialogue sequence.
    /// </summary>
    /// <remarks>This method retrieves the list of dialogues from the associated NPC scriptable object and
    /// sets the current dialogue ID to the first dialogue's ID, if available.  If no dialogues are present, the current
    /// dialogue ID is set to <see langword="null"/>. The dialogue sequence is then started.</remarks>
    private void Awake()
    {
        dialogues = npcSO.Dialogues;

        currentId = dialogues.Count > 0 ? dialogues[0].Id : null;

        StartDialogue();
    }

    /// <summary>
    /// Initializes the dialogue system and displays the first dialogue text, if available.
    /// </summary>
    /// <remarks>If the first dialogue text is not available, a default message is displayed, and an error is
    /// logged. This method also sets up the "Next" button to proceed to the next dialogue when clicked.</remarks>
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

    /// <summary>
    /// Displays the appropriate text or UI elements based on the current dialogue state.
    /// </summary>
    /// <remarks>This method updates the UI to show the text of the current dialogue element, which can be a
    /// sentence, question, or answer. - If the current dialogue is a sentence, the text is displayed, and the "Next"
    /// button is shown unless the dialogue is finished. - If the current dialogue is a question, answer buttons are
    /// dynamically created for each possible answer. Selecting an answer updates the dialogue state and recursively
    /// calls this method to display the next dialogue. - If the current dialogue ID is null, a debug message is
    /// logged.</remarks>
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

                        AnswerSO answer = dialogues[currentIndex] as AnswerSO;

                        Button button = Instantiate(UIAnswerButton, UIAnswersParent);
                        button.GetComponentInChildren<TextMeshProUGUI>().text = answer.Text;

                        button.onClick.AddListener(() =>
                        {
                            currentId = answer.Id;

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

    /// <summary>
    /// Updates the current index to the next dialogue entry that matches the specified criteria.
    /// </summary>
    /// <remarks>The method iterates through the dialogue collection starting from the next index after the
    /// current one. It updates the current index to the first dialogue entry that either has the same ID as the current
    /// one, is considered convergent with the current ID, or is marked as finished.</remarks>
    private void SetCurrentIndexById()
    {
        int i = currentIndex + 1;
        bool found = false;

        while ((i < dialogues.Count && !found))
        {
            if (currentId.Equals(dialogues[i].Id) || IsConvergent(currentId, dialogues[i].Id) || IsFinished(dialogues[i].Id))
            {
                found = true;
                currentIndex = i;
            }

            i++;
        }
    }

    /// <summary>
    /// Determines whether the two specified identifiers are convergent based on a specific numeric condition.
    /// </summary>
    /// <param name="id1">The first identifier, represented as a string. Must be a valid numeric string.</param>
    /// <param name="id2">The second identifier, represented as a string. Must be a valid numeric string.</param>
    /// <returns><see langword="true"/> if the difference between the numeric values of <paramref name="id1"/> and <paramref
    /// name="id2"/>  equals the last digit of <paramref name="id1"/>; otherwise, <see langword="false"/>.</returns>
    private bool IsConvergent(string id1, string id2)
    {
        int Id1 = int.Parse(id1);
        int Id2 = int.Parse(id2);

        return (Id1 - Id2) == int.Parse(id1[id1.Length - 1].ToString());
    }

    /// <summary>
    /// Determines whether the operation associated with the specified identifier is finished.
    /// </summary>
    /// <param name="id">The identifier representing the operation. Must be a non-null string with a length greater than 2.</param>
    /// <returns><see langword="true"/> if the operation is considered finished based on the identifier; otherwise, <see
    /// langword="false"/>.</returns>
    private bool IsFinished(string id)
    {
        if (id.Length <= 2)
            return false;
        else
            isFinished = true;
        return int.Parse(id[id.Length - 1].ToString()) + int.Parse(id[id.Length - 2].ToString()) == 0;
    }
}