using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Club : Weapon
    {
		public String attackDesc()
		{
			return "swings a gnarled club at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Club;
		}
	}
}
