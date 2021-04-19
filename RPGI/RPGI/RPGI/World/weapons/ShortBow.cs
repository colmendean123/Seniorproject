using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
    class ShortBow : Weapon
    {
		public String attackDesc()
		{
			return "shoots the shortbow at";
		}

		public Weapons getKeyword()
		{
			return Weapons.ShortBow;
		}
	}
}
