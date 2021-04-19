using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Trident : Weapon
    {
		public String attackDesc()
		{
			return "stabs forward with a trident at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Trident;
		}
	}
}
