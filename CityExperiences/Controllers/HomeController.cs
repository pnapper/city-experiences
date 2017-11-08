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
      Console.WriteLine(tagSearch);
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



    //VIEW EXPERIENCES BY CITY WITHOUT USER LOGIN
    [HttpGet("/city/{cityId}/view")]
    public ActionResult ViewCityExperiences(int cityId)
    {
      City citySearch = City.Find(cityId);

      List<Experience> allCityExperiences = citySearch.GetCityExperiences();

      return View("CityExperiences", allCityExperiences);
    }


    //USER PROFILE
    [HttpGet("/user/{userId}/profile")]
    public ActionResult UserProfile(int userId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person thisPerson = Person.Find(userId);
      List<Experience> userListings = thisPerson.GetPersonListings();
      List<Experience> userBookings = thisPerson.GetPersonBookings();

      model.Add("user", thisPerson);
      model.Add("listings", userListings);
      model.Add("bookings", userBookings);

      return View(model);
    }

    //User Login PAGE
    [HttpGet("/login")]
    public ActionResult LoginPage()
    {
      return View("Login");
    }

    //User Login Logic
    [HttpPost("/user/home/login")]
    public ActionResult Login()
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      string username  = Request.Form["username"];
      string password = Request.Form["password"];

      List<Person> allUsers = Person.GetAll();


      foreach (var person in allUsers)
      {
        if(person.GetName() == username && person.GetPassword() == password)
        {
          List<Experience> newestExperiences = Experience.GetAll();
          List<City> allCities = City.GetAll();

          model.Add("user", person);
          model.Add("newest-experiences", newestExperiences);
          model.Add("all-cities", allCities);

          return View("IndexUser", model);
        }

      }
      return View("Login");
    }

    // User Signup Page
    [HttpGet("/signup")]
    public ActionResult SignUp()
    {
      return View();
    }

    //User Object Created bringing Users into User Enabled Index Page.
    [HttpPost("/user/home/signup")]
    public ActionResult SignUpPost()
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person newPerson = new Person(Request.Form["name"], Request.Form["dob"], Request.Form["country"], Request.Form["email"], Request.Form["phone"], Request.Form["password"]);
      List<Experience> allExperiences = Experience.GetAll();
      List<City> allCities = City.GetAll();

      newPerson.Save();
      model.Add("user", newPerson);
      model.Add("newest-experiences", allExperiences);
      model.Add("all-cities", allCities);

      return View("IndexUser", model);
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

    [HttpGet("/city/{cityId}/experience/{experienceId}/view")]
    public ActionResult ViewExperienceFromCity(int experienceId)
    {

      Experience thisExperience = Experience.Find(experienceId);

      return View("ViewExperience", thisExperience);
    }

    [HttpGet("/experience/{experienceId}/view")]
    public ActionResult ViewExperience(int experienceId)
    {

      Experience thisExperience = Experience.Find(experienceId);

      return View("ViewExperience", thisExperience);
    }

    [HttpGet("/user/{userId}/experience/new")]
    public ActionResult CreateExperience(int userId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();
      Person thisPerson = Person.Find(userId);
      List<City> allCities = City.GetAll();

      model.Add("user", thisPerson);
      model.Add("cities", allCities);

      return View(model);
    }

    [HttpPost("/user/{userId}/experience/add")]
    public ActionResult AddViewExperience(int userId)
    {
      Dictionary<string, object> model = new Dictionary<string, object> ();

      Person thisPerson = Person.Find(userId);
      int UserId = userId;
      // Console.WriteLine("location id"+Request.Form["experience-location"]);
      // Console.WriteLine("experience title"+Request.Form["experience-title"]);
      // Console.WriteLine("experience description"+Request.Form["experience-description"]);
      // Console.WriteLine("experience-photo"+Request.Form["experience-photo"]);
      // Console.WriteLine("experience-price"+Request.Form["experience-price"]);
      Experience newExperience = new Experience(Int32.Parse(Request.Form["experience-location"]),
      UserId, Request.Form["experience-title"], Request.Form["experience-description"], Request.Form["experience-photo"], Int32.Parse(Request.Form["experience-price"]));
      newExperience.Save();
      int TagValue = Int32.Parse(Request.Form["number-loop"]);
      for(var i=1;i<=TagValue;i++)
      {
        Console.WriteLine("tag"+Request.Form["tag-name"+i]);
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
      List<Experience> newestExperiences = Experience.GetAll();
      List<City> allCities = City.GetAll();

      model.Add("user", thisPerson);
      model.Add("newest-experiences", newestExperiences);
      model.Add("all-cities", allCities);


      return View("IndexUser", model);
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

      return View("ViewExperienceUser", model);
    }

    // [HttpPost("/user/{userId}/experience/{experienceId}/book")]
    // public ActionResult BookExperience()
    // {
    //
    // }
  }
}
