using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using CityExperiences.Models;
using CityExperiences;

namespace CityExperiences.Controllers
{
  public class HomeController : Controller
  {
    //GUEST LANDING PAGE
    [HttpGet("/")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      List<Experience> newestExperiences = Experience.GetAll();
      List<City> allCities = City.GetAll();

      model.Add("newest-experiences", newestExperiences);
      model.Add("all-cities", allCities);

      return View("Index", model);
    }

    //USER LANDING PAGE
    [HttpGet("/user/{userId}/home")]
    public ActionResult IndexUser(int userId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Person thisPerson = Person.Find(userId);
      List<Experience> newestExperiences = Experience.GetAll();
      List<City> allCities = City.GetAll();

      model.Add("user", thisPerson);
      model.Add("newest-experiences", newestExperiences);
      model.Add("all-cities", allCities);

      return View("Index", model);
    }

    //SEARCH EXPERIENCES BY TAG
    [HttpPost("/experiences/tag/search")]
    public ActionResult ViewTagExperiences()
    {
      Tag tagSearch = Tag.FindId(Request.Form["tag-name"]);
      List<Experience> allTagExperiences = tagSearch.GetTagExperiences();

      return View("TagExperiences", allTagExperiences);
    }

    //SEARCH EXPERIENCES BY CITY
    [HttpPost("/experiences/city/search")]
    public ActionResult ViewCityExperiences()
    {
      City citySearch = City.FindId(Request.Form["city-name"]);
      List<Experience> allCityExperiences = citySearch.GetCityExperiences();

      return View("CityExperiences", allCityExperiences);
    }

    //VIEW EXPERIENCES BY TAG & CITY WITHOUT USER LOGIN
    [HttpGet("/experience/{experienceId}/view")]
    public ActionResult ViewExperience(int userId, int experienceId)
    {
      Experience thisExperience = Experience.Find(experienceId);

      return View("ViewExperience", thisExperience);
    }

    //User Profile
    [HttpGet("/user/{userId}/profile")]
    public ActionResult UserProfile(int userId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person thisPerson = Person.Find(int userId);
      List<Experience> userListings = thisPerson.GetPersonListings();
      List<Booking> userBookings = thisPerson.GetPersonBookings();

      model.Add("user", thisPerson);
      model.Add("listings", userListings);
      model.Add("bookings", userBookings);

      return View(model);
    }



    [HttpGet("/user/{userId}/experience/{experienceId}/view")]
    public ActionResult ViewExperienceUser(int userId, int experienceId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person thisPerson = Person.Find(userId);
      Experience thisExperience = Experience.Find(experienceId);

      model.Add("user", thisPerson);
      model.Add("experience", thisExperience);

      return View("ViewExperienceUser", model);
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
