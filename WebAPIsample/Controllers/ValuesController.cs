using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Http;

namespace WebAPIsample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        public HttpResponseMessage employee(int id)
        {
            HttpResponseMessage response = null;
            DataTable responseObj = new DataTable();
            responseObj.Columns.Add("employeeID");
            responseObj.Columns.Add("employeeName");
            responseObj.Columns.Add("employeeAge");
            DataRow dtRow = null;
            string json = string.Empty;

            Models.employeeModel model = new Models.employeeModel();
            using (SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["testDB"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from employee where employeeID = " + id, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.id = reader.GetInt32(0);
                            model.employeeName = reader.GetString(1);
                            model.employeeAge = reader.GetString(2);

                            dtRow = responseObj.NewRow();
                            dtRow["employeeID"] = model.id;
                            dtRow["employeeName"] = model.employeeName;
                            dtRow["employeeAge"] = model.employeeAge;
                            responseObj.Rows.Add(dtRow);
                        }
                    }
                }
            }

            json = JsonConvert.SerializeObject(responseObj);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;
        }
    }
}