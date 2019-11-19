using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT_Services.Model
{
    class Skill
    {
		private string _skillName;
		private string _skillDescription;

		public string SkillName { get => _skillName; set => _skillName = value; }
		public string SkillDescription { get => _skillDescription; set => _skillDescription = value; }

		public Skill(string skillName, string skillDescription)
		{
			SkillName = skillName;
			SkillDescription = skillDescription;
		}
	}
}
