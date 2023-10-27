[System.Serializable]
public class Quest{
    public bool isActive;

    public int id;
    public string title;
    public string description;
    public int  moneyReward;
    public int honorReward;

    public GoalChecker goalChecker;

    public Quest(string title, string description, int moneyReward, int honorReward){
        this.title = title;
        this.description = description;
        this.moneyReward = moneyReward;
        this.honorReward = honorReward;
    }
    public override string ToString() {
        return $"{title}\n  - {description} {goalChecker}\n";
    }
}