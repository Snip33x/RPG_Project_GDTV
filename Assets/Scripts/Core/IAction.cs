using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//everything in Interface is public  i can put here methods and properties only, no variables

namespace RPG.Core
{
    public interface IAction
    {
        void Cancel();
    }
}