using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class CsvToDialogue : MonoBehaviour
{
    /*
     * This utility reads a CSV file containing dialogue data for a specific NPC
     * and generates corresponding ScriptableObjects for each dialogue entry.
     * The generated ScriptableObjects are saved in the Resources folder under the NPC's directory.
     * The CSV file is expected to have the following format:
     * ID|Type|Text|AnswerIds (for questions)
     * Type: 1 - Sentence, 2 - Question, 3 - Answer
     * 
     * Per a utilitzar s'ha de crear una carpeta dins Resources amb el nom de l'NPC i una carpeta "Dialogues" dins d'aquesta.
     * També s'ha de modificar la variable npcName per posar el nom de l'NPC que es vol processar.
     */

    /*private static readonly string npcName = "Npc1";

    private static readonly string csvFileName = npcName + "-Dialogues.csv";
    private static readonly string csvFilePath = "/Files/" + csvFileName;

    private static readonly string resourceFolderPath = "Assets/Resources/" + npcName + "/";

    private static readonly string dialoguesFolderPath = resourceFolderPath + "/Dialogues/";*/

    private static TextSO newDialogue;
    private static NpcSO newNpc;

    //[MenuItem("Utilities/Generate Dialogues")]
    public static void GenerateDialogues(string npcName, string csvFileName, string csvFileDirectory, string resourcesDirectoryPath)
    {
        //string csvFileName = npcName + "-Dialogues.csv";
        string csvFilePath = $"{csvFileDirectory}/{csvFileName}";
        string npcDirectoryPath = $"{resourcesDirectoryPath}/{npcName}";
        string dialoguesDirectoryPath = npcDirectoryPath + "/Dialogues";

        newNpc = ScriptableObject.CreateInstance<NpcSO>();
        newNpc.name = npcName;
        newNpc.Dialogues = new List<TextSO>();

        string[] allLines = File.ReadAllLines(Application.dataPath + csvFilePath);

        for (int i = 0; i < allLines.Length; i++)
        {
            string[] line = allLines[i].Split('|');

            int type = int.Parse(line[1]);

            string name = $"{i}_";

            if (type >= 1 && type <= 3)
            {
                Directory.CreateDirectory(npcDirectoryPath);
                Directory.CreateDirectory(dialoguesDirectoryPath);

                if (type == 1)
                {
                    newDialogue = ScriptableObject.CreateInstance<SentenceSO>();
                    name += "Sentence";
                }
                else if (type == 2)
                {
                    newDialogue = ScriptableObject.CreateInstance<QuestionSO>();
                    ((QuestionSO)newDialogue).AnswerIds = new List<string>(line[3].Split(':'));
                    name += "Question";
                }
                else if (type == 3)
                {
                    newDialogue = ScriptableObject.CreateInstance<AnswerSO>();
                    name += "Answer";
                }

                newDialogue.Id = line[0];
                newDialogue.Text = line[2];

                newNpc.Dialogues.Add(newDialogue);

                name += "_" + newDialogue.Id;

                AssetDatabase.CreateAsset(newDialogue, $"{dialoguesDirectoryPath}/{name}.asset");
            }
            else
            {
                Debug.LogError("Invalid type in CSV: " + type);
            }
        }
        AssetDatabase.CreateAsset(newNpc, $"{npcDirectoryPath}/{npcName}.asset");

        AssetDatabase.SaveAssets();
    }
}
