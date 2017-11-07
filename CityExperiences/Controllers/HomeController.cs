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

    [HttpGet("/user/{userId}/experience/{experienceId}/view")]
    public ActionResult ViewExperience(int userId, int experienceId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      User thisUser = User.Find(userId);
      Experience thisExperience = Experience.Find(experienceId);

      model.Add("user", thisUser);
      model.Add("experience", thisExperience);

      return View("ViewExperience", model);
    }

    [HttpPost("/user/{userId}/experience/new")]
    public ActionResult AddExperience(int userId)
    {

      User thisUser = User.Find(userId);

      Experience newExperience = new Experience(
      Int32.Parse(Request.Form["experience-location"]),
      Int32.Parse(userId), Request.Form["experience-title"], Request.Form["experience-description"], Request.Form["experience-photo"], Int32.Parse(Request.Form["experience-price"]));
      newExperience.Save();
      int TagValue = Int32.Parse(Request.Form["number-loop"]);
      for(var i=1;i<=TagValue;i++)
      {
        Tag newTag = new Tag(Request.Form["tag-name"+i]);
        if(newTag.IsNewTag() == true)
        {
          newTag.Save();
          newTag.AddTag(newTag);
        }
        else
        {
          Tag repeatTag = newTag.FindTag();
          newTag.AddTag(repeatTag);
        }
      }
      return View("IndexUser", thisUser);
    }

    [HttpPost("/user/{userId}/experience/{experienceId}/edit")]
    public ActionResult EditExperience(int userId, int experienceId)
    {
      User thisUser = User.Find(userId);
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Experience thisExperience = Experience.Find(experienceId);

      findExperience.UpdateDescription(Request.Form["experience-description"];
      findExperience.UpdatePhoto(Request.Form["experience-photo"];
      findExperience.UpdatePrice(Int32.Parse(Request.Form["experience-price"]));

      model.Add("user", thisUser);
      model.Add("experience", thisExperience);


      return View("ViewExperience", model);

    }
  }
