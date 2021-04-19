using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class FireStaff : Weapon
    {
		public String attackDesc()
		{
			return "launches a fireball at";
		}

		public Weapons getKeyword()
		{
			return Weapons.FireStaff;
		}
	}
}
