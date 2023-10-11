public class GoalChecker{
    GoalType goalType;
    int currentAmount;
    int targetAmount;

    public GoalChecker(GoalType goalType, int targetAmount){
        this.goalType = goalType;
        this.targetAmount = targetAmount;
        this.currentAmount = 0;
    }

    public bool isReached(){
        return currentAmount >= targetAmount;
    }
}

public enum GoalType{
    Kill = 0,
    Gathering = 1
}