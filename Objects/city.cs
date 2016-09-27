using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;
using Xunit;

namespace AirlinePlanner
{
  public class city
  {
    private int _id;
    private string _name;

    public city(int id, string name)
    {
      _id = id;
      _name = name;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is city))
      {
        return false;
      }
      else
      {
        city newCity = (city) otherCity;
        bool idEquality = (this.GetId() == newCity.GetId());
        bool nameEquality = (this.GetName() == newCity.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<city> GetAll()
    {
      List<city> allcities = new List<city>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int citiesid = rdr.GetInt32(0);
        string citiesName = rdr.GetString(1);
        city newCities = new city(citiesid, citiesName);
        allcities.Add(newCities);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allcities;
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static city Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @cityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@cityId";
      cityIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCityId = 0;
      string foundCityName = null;
      while (rdr.Read() )
      {
        foundCityId = rdr.GetInt32(0);
        foundCityName = rdr.GetString(1);
      }
      city foundCity = new city(foundCityId, foundCityName);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCity;
    }
    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE cities SET name = @NewName OUTPUT INSERTED.name WHERE id = @cityId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@cityId";
      cityIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
      }
      

      if(rdr != null)
      {
        rdr.Close();
      }
    }
  }
}
