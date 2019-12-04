using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIT_Services_Unit_Tests
{
	[TestClass]
	public class ClientTest
	{
		[TestMethod]
		public void TestConstructor()
		{
			try
			{
				
				BIT_Services.Model.Client client = new BIT_Services.Model.Client(
					"John Pancake",
					"23 Fake Street",
					new BIT_Services.Model.Suburb("Hornsby"),
					"NSW",
					"0478777777",
					"johnPancake@gmail.com",
					"Likes his pancakes");
			}
			catch (Exception)
			{
				Assert.Fail();
			}
		}
	}
}
