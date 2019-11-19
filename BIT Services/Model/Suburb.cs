using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	class Suburb
	{
		private string _suburbName;

		public string SuburbName { get => _suburbName; }

		public Suburb(string suburbName)
		{
			_suburbName = suburbName;
		}

		public override string ToString()
		{
			return SuburbName;
		}
	}
}
