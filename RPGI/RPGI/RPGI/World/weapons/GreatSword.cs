using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class GreatSword : Weapon
    {
		public String attackDesc()
		{
			return "swings a mighty greatsword at";
		}

		public Weapons getKeyword()
		{
			return Weapons.GreatSword;
		}
	}
}
