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
      Int32.Parse(Request.Form["experience-photo"],
      Int32.Parse(Request.Form["experience-price"]));
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

        return View("IndexUser", userId);
      }
