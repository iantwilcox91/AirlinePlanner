using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;
using Xunit;

namespace AirlinePlanner
{
  public class flight
  {
    private int _id;
    private string _name;

    public flight( string name, int id = 0)
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
    public override bool Equals(System.Object otherflight)
    {
      if (!(otherflight is flight))
      {
        return false;
      }
      else
      {
        flight newflight = (flight) otherflight;
        bool idEquality = (this.GetId() == newflight.GetId());
        bool nameEquality = (this.GetName() == newflight.GetName());
        return (idEquality && nameEquality);
      }
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("INSERT INTO flights (name) OUTPUT INSERTED.id VALUES (@flightName);",conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@flightName";
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

    public static List<flight> GetAll()
    {
      List<flight> allFlights = new List<flight>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightid = rdr.GetInt32(0);
        string flightName = rdr.GetString(1);
        flight newflight = new flight(flightName, flightid);
        allFlights.Add(newflight);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allFlights;
    }


    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
  }
}
