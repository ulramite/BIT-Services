using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
	public class Suburb
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

		public override bool Equals(Object obj)
		{
			if (obj is Suburb)
			{

				return null != obj && ((Suburb)obj).SuburbName == SuburbName;
			}
			else if (obj is String)
			{
				return null != obj && (string)obj == SuburbName;
			}
			else
			{
			return false;
			}
		}

		public override int GetHashCode()
		{
			return SuburbName.GetHashCode();
		}
	}
}
