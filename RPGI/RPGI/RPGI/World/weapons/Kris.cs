using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class Kris : Weapon
    {
		public String attackDesc()
		{
			return "jabs its kris at";
		}

		public Weapons getKeyword()
		{
			return Weapons.Kris;
		}
	}
}
