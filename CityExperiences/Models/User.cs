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

    
  }
}
