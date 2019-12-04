using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIT_Services_Unit_Tests
{
	[TestClass]
	public class ContractorTest
	{
		[TestMethod]
		public void TestConstructor()
		{
			try
			{
				BIT_Services.Model.Contractor contractor = new BIT_Services.Model.Contractor(
					"John",
					"Pancake",
					"23 Fake Street",
					"NSW",
					new BIT_Services.Model.Suburb("Hornsby"),
					"0478777777",
					"johnPancake@gmail.com"
					);
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}
	}
}
