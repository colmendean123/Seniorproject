using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Rapier : Weapon
    {
		public String attackDesc()
		{
			return "lunges with the rapier at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Rapier;
		}
	}
}
