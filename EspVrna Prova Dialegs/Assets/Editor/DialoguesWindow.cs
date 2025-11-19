using UnityEditor;
using UnityEngine;

public class DialoguesWindow : EditorWindow
{
    /*string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;*/

    string csvFileDirectory = "/Files";
    string resourcesDirectoryPath = "Assets/Resources";

    string csvFileName = "Npc-Dialogues.csv";
    string npcName = "Npc";


    [MenuItem("Window/Dialogues")]
    public static void ShowWindow()
    {
        GetWindow<DialoguesWindow>("Dialogues");
    }

    void OnGUI()
    {
        GUILayout.Label("Generar Diàlegs", EditorStyles.boldLabel);

        EditorGUILayout.Space(20);

        csvFileDirectory = EditorGUILayout.TextField("Directory Arxiu .csv", csvFileDirectory);
        resourcesDirectoryPath = EditorGUILayout.TextField("Directory Resources", resourcesDirectoryPath);

        EditorGUILayout.Space(20);

        csvFileName = EditorGUILayout.TextField("Nom Arxiu .csv", csvFileName);
        npcName = EditorGUILayout.TextField("Nom NPC", npcName);

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Generar"))
        {
            CsvToDialogue.GenerateDialogues(npcName, csvFileName, csvFileDirectory, resourcesDirectoryPath);
        }

        /*GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();*/
    }
}
