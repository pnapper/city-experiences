using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace CityExperiences.Models;
{
  public class User
  {
    private int _id;
    private string _name;
    private string _dateOfBirth;
    private int _cityId;
    private string _email;

    public User(string name, string dateOfBirth, int cityId, string email, int id = 0)
    {
      _id = id;
      _name = name;
      _dateOfBirth = dateOfBirth;
      _cityId = cityId;
      _email = email;
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public string GetName()
    {
      return _name;
    }

    public string GetDateOfBirth()
    {
      return _dateOfBirth;
    }

    public int GetCityId()
    {
      return _cityId;
    }

    public string GetEmail()
    {
      return _email;
    }

    public int GetId()
    {
      return _id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users (name, dateOfBirth, cityId, email) VALUES (@name, @dateOfBirth, @cityId, @email);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter dateOfBirth = new MySqlParameter();
      dateOfBirth.ParameterName = "@dateOfBirth";
      dateOfBirth.Value = this._dateOfBirth;
      cmd.Parameters.Add(dateOfBirth);

      MySqlParameter cityId = new MySqlParameter();
      cityId.ParameterName = "@cityId";
      cityId.Value = this._cityId;
      cmd.Parameters.Add(cityId);

      MySqlParameter email = new MySqlParameter();
      email.ParameterName = "@email";
      email.Value = this._email;
      cmd.Parameters.Add(email);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId();
      conn.Close();

      if (conn != null)
      {
        conn.Dispose()
      }
    }

    public static List<User> GetAll()
    {
      List<User> allUsers = new List<User> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int userId = rdr.GetInt32(0);
        string userName = rdr.GetString(1);
        string userDateOfBirth = rdr.GetString(2);
        int userCityId = rdr.GetInt32(3);
        string userEmail = rdr.GetString(4);

        User newUser = new User(userName, userDateOfBirth, userCityId, userEmail, userId);
        allUsers.Add(newUser);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allUsers;
    }

    public static User Find(int Id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int userId = 0;
      string userName = "";
      string userDateOfBirth = "";
      int userCityId = 0;
      string userEmail = "";

      while(rdr.Read())
      {
        userId = rdr.GetInt32(0);
        userName = rdr.GetString(1);
        userDateOfBirth = rdr.GetString(2);
        userCityId = rdr.GetInt32(3);
        userEmail = rdr.GetString(4);
      }
      User newUser = new User(userName, userDateOfBirth, userCityId, userEmail, userId);
      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
      return newUser;
    }

    //Experiences and bookings

    public void DeleteUser()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand = new MySqlCommand("DELETE FROM users WHERE id = @userId; DELETE FROM experiences WHERE id = @userId; DELETE FROM bookings WHERE id = @userId;", conn);
      MySqlParameter UserIdParameter = new MySqlParameter();
      UserIdParameter.ParameterName = "@userId";
      UserIdParameter.Value = this.GetId();

      cmd.Parameters.Add(UserIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close(); 
      }
    }
  }
}
