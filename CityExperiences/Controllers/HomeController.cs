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
    [HttpPost("/user/{userId}/experience/new")]
    public ActionResult AddExperience(int userId)
    {
      Experience newExperience = new Experience(
      Int32.Parse(Request.Form["experience-location"],
      Int32.Parse(userId),
      Request.Form["experience-title"],
      Request.Form["experience-description"],
      Int32.Parse(Request.Form["experience-location"],
      Request.Form["experience-title"], Int32.Parse(Request.Form["experience-copies"]));
      newExperience.Save();
      int authorValue = Int32.Parse(Request.Form["number-loop"]);
      for(var i=1;i<=authorValue;i++)
      {
        Author newAuthor = new Author(Request.Form["author-name"+i]);
        if(newAuthor.IsNewAuthor() == true)
        {
          newAuthor.Save();
          newExperience.AddAuthor(newAuthor);
        }
        else
        {
          Author repeatAuthor = newAuthor.FindAuthor();
          newExperience.AddAuthor(repeatAuthor);
        }
      }
    }
  }
}
