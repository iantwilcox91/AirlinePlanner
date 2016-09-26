using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class flightTest // : IDisposable
  {
    public flightTest()
    {
      DBConfiguration.ConnectionString =  "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AirlinePlanner_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_emptyDataBaseCheck()
    {
      int result = flight.GetAll().Count;
      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_saveflights()
    {
      flight newflight = new flight("jfk");
      newflight.Save();

      List<flight> result = flight.GetAll();
      List<flight> testList = new List<flight>{newflight};

      Assert.Equal(testList, result);
    }
  }
}
