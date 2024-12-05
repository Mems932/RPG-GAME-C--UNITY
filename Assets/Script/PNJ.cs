using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNJ : MonoBehaviour
{
    [SerializeField]
    string[] sentences; // Doit être un tableau pour stocker plusieurs phrases
    [SerializeField]
    string characterName;
    int index;
    bool isOndial, canDial;

    HUDManager manager => HUDManager.instance; // Correction de la faute de frappe

    public QuestSO quest;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && canDial)
        {
           if(quest != null && quest.statut == QuestSO.Statut.none)
            {
                StartDialogue(quest.sentences);
            }
            else if(quest != null && quest.statut == QuestSO.Statut.accepter)
            {
                StartDialogue(quest.InProgressSentence);
            }else if(quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount == quest.amounToFind)
            {
                StartDialogue(quest.completeSentence);
                quest.statut = QuestSO.Statut.complete;
            }
            else if(quest != null && quest.statut == QuestSO.Statut.complete)
            {
                StartDialogue(quest.afterQuestSentence);
            }
            else if(quest == null)
            {
                StartDialogue(sentences);
            }
        }
        
    }


    public void StartDialogue(string[] sentence)
    {
        manager.dialogueHolder.SetActive(true);
        PlayerController.instance.canMove = false;
        PlayerController.instance.canInteract = false; // Assurez-vous que cette ligne désactive correctement les interactions
        isOndial = true;
        TypingText(sentence);
        manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        manager.continueButton.GetComponent<Button>().onClick.AddListener(delegate { NextLine(sentence); });
    }


    void TypingText(string[] sentence)
    {
        manager.nameDisplay.text = "";
        manager.textDisplay.text = "";

        manager.nameDisplay.text = characterName;
        manager.textDisplay.text = sentence[index];

        if (manager.textDisplay.text == sentence[index])
        {
            manager.continueButton.SetActive(true);
        }
    }

    public void NextLine(string[] sentence)
    {
        manager.continueButton.SetActive(false);

        if (isOndial && index < sentence.Length - 1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentence);
        }
        else if (isOndial && index == sentence.Length - 1)
        {
            isOndial = false;
            index = 0;
            manager.textDisplay.text = "";
            manager.nameDisplay.text = "";
            manager.dialogueHolder.SetActive(false);

            PlayerController.instance.canMove = true; // Réactivation du mouvement
            PlayerController.instance.canAttack = true; // Réactivation de l'attaque
            PlayerController.instance.canInteract = true; // Réactivation de l'interaction
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canDial = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canDial = false;
        }
    }
}
