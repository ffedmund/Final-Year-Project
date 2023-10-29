using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoleDistributionManager : MonoBehaviour
{
    public PlayerRole playerRole;
    public Transform backgroundUI;
    public Transform skillUI;
    public Transform[] talentChoiceBoxesUI;

    public List<PlayerAbility> talentChoiceList = new List<PlayerAbility>();

    // Start is called before the first frame update
    async void Start()
    {
        await DataReader.ReadBackgroundDataBase();
        await DataReader.ReadAbilityDataBase();
        await DataReader.ReadTargetDataBase();
        AssignRandomRole();
    }

    public void AssignRandomRole(int mode = 0){
        switch(mode){
            case 0:
                int rndInt = Random.Range(0,DataReader.backgorundDictionary.Count)+1;
                playerRole.playerBackground = DataReader.backgorundDictionary[rndInt];
                playerRole.playerSkill = DataReader.skillDictionary[playerRole.playerBackground.id];
                do{
                    int tempRndInt = Random.Range(0,DataReader.talentDictionary.Count)+1;
                    if(!talentChoiceList.Contains(DataReader.talentDictionary[tempRndInt])){
                        talentChoiceList.Add(DataReader.talentDictionary[tempRndInt]);
                    }
                }while(talentChoiceList.Count<3);
                playerRole.playerTargets.Add(DataReader.roleTargetDictionary[rndInt]);
                int maximum = 100;
                do{
                    PlayerTarget rndPlayerTarget = DataReader.targetList[Random.Range(0,DataReader.targetList.Count)];
                    if(!playerRole.playerTargets.Contains(rndPlayerTarget)){
                        playerRole.playerTargets.Add(rndPlayerTarget);
                    }
                    maximum--;
                }while(playerRole.playerTargets.Count < 3 && maximum>0);
                break;
            default:
                break;
        }
        SetUpUI();
    }

    void SetUpUI(){

        backgroundUI.Find("RoleTitle").GetComponent<TextMeshProUGUI>().SetText(playerRole.playerBackground.role);
        backgroundUI.Find("BackgroundText").GetComponent<TextMeshProUGUI>().SetText("<size=120%>Background</size>\n"+playerRole.playerBackground.description);

        string targetString = "<size=120%>Target</size>\n";
        int targetCount = 1;
        foreach(PlayerTarget playerTarget in playerRole.playerTargets){
            targetString += String.Format("{2}.<indent=5%>"+playerTarget.title+" "+playerTarget.shortDescription+"</indent>\n",0,0,targetCount++);
        }
        backgroundUI.Find("TargetText").GetComponent<TextMeshProUGUI>().SetText(targetString);

        skillUI.Find("AbilityContent").Find("SkillText").GetChild(0).GetComponent<TextMeshProUGUI>().SetText(String.Format("<size=120%>Skill</size>\n"+"<align=\"left\"><i>{1}</i>\n"+playerRole.playerSkill.description+"</align>","30",playerRole.playerSkill.name));
        
        skillUI.Find("AbilityContent").Find("TalentText").GetChild(0).GetComponent<TextMeshProUGUI>().SetText(String.Format("<size=120%>Talent\nChoose One Talent</size>"));
        if(playerRole.playerTalent.description != null && playerRole.playerTalent.id != 0){
            skillUI.Find("AbilityContent").Find("TalentText").GetChild(0).GetComponent<TextMeshProUGUI>().SetText(String.Format("<size=120%>Talent</size>\n"+"<align=\"left\"><i>{1}</i>\n"+playerRole.playerTalent.description+"</align>","30",playerRole.playerTalent.name));
        }

        int i = 0;
        foreach(PlayerAbility talentChoice in talentChoiceList){
            talentChoiceBoxesUI[i].GetChild(0).GetComponent<TextMeshProUGUI>().SetText(talentChoice.name);
            talentChoiceBoxesUI[i].GetComponent<Button>().onClick.AddListener(() => {
                playerRole.playerTalent = talentChoice;
                SetUpUI();
            });
            i++;
        }
    }

    public void LoadScene(){
        SceneManager.LoadScene(1);
    }


}
