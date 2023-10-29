[System.Serializable]
public class GoalChecker{
    public GoalType goalType;
    public string targetId;
    public int currentAmount;
    public int targetAmount;

    public GoalChecker(GoalType goalType, int targetAmount, string targetId){
        this.goalType = goalType;
        this.targetAmount = targetAmount;
        this.currentAmount = 0;
        this.targetId = targetId;
    }

    public bool isReached(){
        return currentAmount >= targetAmount;
    }

    public void EnemyKilled(string id){
        if(goalType == GoalType.Kill && targetId == id){
            currentAmount++;
        }
    }

    public void ItemCollected(string id){
        if((goalType == GoalType.Gathering || goalType == GoalType.Delivery) && targetId == id){
            currentAmount++;
        }
    }

    public override string ToString(){
        return $"{currentAmount}/{targetAmount}";
    }
}

public enum GoalType{
    Kill = 0,
    Gathering = 1,
    Delivery = 2
}