using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using CityExperiences.Models;

namespace CityExperiences.Tests
{
  [TestClass]
  public class ExperienceTests : IDisposable
  {
    public ExperienceTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=cityexperiences_test;";
    }
    public void Dispose()
    {
      Experience.DeleteAll();
    }

    [TestMethod]
       public void GetAll_ExperiencesEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Experience.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

   [TestMethod]
       public void Equals_OverrideTrueForSameTitle_Experience()
       {
         //Arrange, Act
         Experience firstExperience = new Experience(1, 2, "Sky Diving", "Blah Blah..", "/hjfsddjf.com", 150);
         Experience secondExperience = new Experience(1, 2, "Sky Diving", "Blah Blah..", "/hjfsddjf.com", 150);

         //Assert
         Assert.AreEqual(firstExperience, secondExperience);
       }

  }
}
