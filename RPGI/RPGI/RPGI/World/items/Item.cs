using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGI.World
{
	public interface Item
	{

		String trigger(Character character);

		String interact(Character character);

		String getAbbreviation();

		Items getKey();

	}
}
