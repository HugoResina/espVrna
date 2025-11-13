using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextSO StartingText;
    private TextSO CurrentText;

    [SerializeField] private TextMeshProUGUI UIText;
    [SerializeField] private Transform UIAnswersParent;
    [SerializeField] private Button UIAnswerButton;
    [SerializeField] private Button UINextButton;

    private void Awake()
    {
        if (StartingText != null)
        {
            CurrentText = StartingText;
            NextDialogue();
        }
    }

    public void ShowCurrentText()
    {
        if (CurrentText is SentenceSO sentence)
        {
            //Debug.Log(sentence.text);  // Mostrem la frase
            UIText.text = sentence.text;

            CurrentText = sentence.nextText;

            UINextButton.gameObject.SetActive(true);

            UINextButton.onClick.AddListener(() =>
            {
                UINextButton.onClick.RemoveAllListeners();
                UINextButton.gameObject.SetActive(false);
                NextDialogue();
            });
        }
        else if (CurrentText is QuestionSO question)
        {
            //Debug.Log(question.text);  // Mostrem la pregunta
            UIText.text = question.text;

            for (int i = 0; i < question.answers.Count; i++)
            {
                //Debug.Log($"{i + 1}. {question.answers[i].text}");

                AnswerSO answer = question.answers[i];

                Button button = Instantiate(UIAnswerButton, UIAnswersParent);
                button.GetComponentInChildren<TextMeshProUGUI>().text = question.answers[i].text;

                button.onClick.AddListener(() =>
                {
                    CurrentText = answer.nextText;

                    NextDialogue();

                    foreach (Transform child in UIAnswersParent)
                    {
                        if (child.gameObject != UIAnswerButton.gameObject)
                        {
                            Destroy(child.gameObject);
                        }
                    }
                });
            }
        }
    }

    // Exemple simple per iniciar un diàleg
    public void NextDialogue()
    {
        ShowCurrentText();
    }
}
