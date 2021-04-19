using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Flail : Weapon
    {
		public String attackDesc()
		{
			return "swings a wicked flail at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Flail;
		}
	}
}
