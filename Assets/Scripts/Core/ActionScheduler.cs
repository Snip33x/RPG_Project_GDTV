using UnityEngine;

//main reason for this is substitution principle part of SOLID it stands for example: a an arrowor bullet can be usead as argument for missile function because they behave similar, and it should do fine

namespace RPG.Core
{
    class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }
    }
}
