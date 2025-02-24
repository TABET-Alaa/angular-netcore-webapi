using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http.HttpResults;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select DepartmentId, DepartmentName from
                            dbo.Department
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // read data from the SQL query.
            SqlDataReader myReader;

            //using ensures that the connection is properly disposed of after use.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                //Creates an instance of SqlCommand
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //Executes the query using ExecuteReader(),
                    //which returns a SqlDataReader containing the results.
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"
                        insert into dbo.Department 
                        values (@DepartmentName)
                         ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Creates a SQL parameter named @DepartmentName.
                    // Assigns dep.DepartmentName as its value.
                    // Ensures the value is treated as raw data, not SQL code.
                    //Sends the query separately from the value, preventing SQL injection.
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                return new JsonResult(table);
            }
        }


        [HttpPut]
        public JsonResult Put(Department dep) {
            string query = @"
                    update dbo.Department
                    set DepartmentName = @DepartmentName
                    where DepartmentId=@DepartmentId
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName ", dep.DepartmentName);
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                };
                return new JsonResult(table);
            }
        }


    }
}
