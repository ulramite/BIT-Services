using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIT_Services_Unit_Tests
{
	[TestClass]
	public class JobRequestTest
	{
		[TestMethod]
		public void TestConstructor()
		{
			try
			{
				BIT_Services.Model.JobRequest jobRequest = new BIT_Services.Model.JobRequest(
					0,
					1,
					null,
					"John Pancake",
					"Needs some more pancakes",
					new DateTime(),
					3,
					0,
					"23 Fake Street",
					new BIT_Services.Model.Suburb("Hornsby"),
					null
					);
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}
	}
}
