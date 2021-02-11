using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class RustyBlade : Weapon
    {
		public String attackDesc()
		{
			return "swings a rusty blade at";
		}

		public Weapons getKeyword()
		{
			return Weapons.RustyBlade;
		}
	}
}
