using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider //this interface lets us implement it on for example in inventory on passive items that buffs our stats, buff debuff components also
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat); //IEnumerable let's us use foreach loop
    }
}