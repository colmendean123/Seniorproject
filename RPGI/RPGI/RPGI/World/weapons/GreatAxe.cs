using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class GreatAxe : Weapon
    {
		public String attackDesc()
		{
			return "slashes with a greataxe at";
		}

		public Weapons getKeyword()
		{
			return Weapons.GreatAxe;
		}
	}
}
