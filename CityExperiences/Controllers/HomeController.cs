using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using CityExperiences.Models;
using CityExperiences;

namespace CityExperiences.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost("/experiences/tag/search")]
    public ActionResult ViewTagExperiences()
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();


      Tag tagSearch = Tag.Find[Request.Form("tag-name")];
      List<Experience> allTagExperiences = Tag.GetTagExperiences();

      model.Add("tag", tagSearch);
      model.Add("experiences", allTagExperiences);

      return View("TagExperiences", model);


    }

    [HttpGet("/experience/{experienceId}/view")]
    public ActionResult ViewExperience(int userId, int experienceId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Experience thisExperience = Experience.Find(experienceId);

      model.Add("experience", thisExperience);

      return View("ViewExperience", model);
    }

    [HttpGet("/user/{userId}/experience/{experienceId}/view")]
    public ActionResult ViewExperience(int userId, int experienceId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person thisPerson = Person.Find(userId);
      Experience thisExperience = Experience.Find(experienceId);

      model.Add("user", thisPerson);
      model.Add("experience", thisExperience);

      return View("ViewExperience", model);
    }

    [HttpPost("/user/{userId}/experience/new")]
    public ActionResult AddExperience(int userId)
    {

      Person thisPerson = Person.Find(userId);

      Experience newExperience = new Experience(
      Int32.Parse(Request.Form["experience-location"]),
      userId, Request.Form["experience-title"], Request.Form["experience-description"], Request.Form["experience-photo"], Int32.Parse(Request.Form["experience-price"]));
      newExperience.Save();
      int TagValue = Int32.Parse(Request.Form["number-loop"]);
      for(var i=1;i<=TagValue;i++)
      {
        Tag newTag = new Tag(Request.Form["tag-name"+i]);
        if(newTag.IsNewTag() == true)
        {
          newTag.Save();
          newExperience.AddTag(newTag);
        }
        else
        {
          Tag repeatTag = newTag.FindTag();
          newExperience.AddTag(repeatTag);
        }
      }
      return View("IndexPerson", thisPerson);
    }

    [HttpPost("/user/{userId}/experience/{experienceId}/edit")]
    public ActionResult EditExperience(int userId, int experienceId)
    {
      Person thisPerson = Person.Find(userId);
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Experience thisExperience = Experience.Find(experienceId);

      thisExperience.UpdateDescription(Request.Form["experience-description"]);
      thisExperience.UpdatePhoto(Request.Form["experience-photo"]);
      thisExperience.UpdatePrice(Int32.Parse(Request.Form["experience-price"]));

      model.Add("user", thisPerson);
      model.Add("experience", thisExperience);


      return View("ViewExperience", model);


    }
  }
}
