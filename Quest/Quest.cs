[System.Serializable]
public class Quest{
    public bool isActive;
    public bool isFinished;

    public int id;
    public string title;
    public string description;
    public int  moneyReward;
    public int honorReward;
    public string targetNPC;
    public string completeDialog;

    public GoalChecker goalChecker;

    public Quest(int id, string title, string description, int moneyReward, int honorReward, string targetNPC, string completeDialog){
        this.id = id;
        this.title = title;
        this.description = description;
        this.moneyReward = moneyReward;
        this.honorReward = honorReward;
        this.targetNPC = targetNPC;
        this.completeDialog = completeDialog;
    }
    public string ToString(string color = "white") {
        return $"<color=\"{color}\">{title}\n</color> - "+ (goalChecker.isReached() && targetNPC != ""?$"Find {targetNPC}\n":$"{description} {goalChecker}\n");
    }
}