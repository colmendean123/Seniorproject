using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    public interface SpecialAbility
    {
        void special(WorldCharacter source, WorldCharacter target);

        String specialDesc();

        Abilities getKey();
    }
}
