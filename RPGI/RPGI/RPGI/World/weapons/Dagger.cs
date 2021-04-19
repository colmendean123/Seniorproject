using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Dagger : Weapon
    {
		public String attackDesc()
		{
			return "thrusts a dagger at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Dagger;
		}
	}
}
