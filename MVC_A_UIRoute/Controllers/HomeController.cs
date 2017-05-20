using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MVC_A_UIRoute.Models;
using CrystalDecisions.CrystalReports.Engine;
using MVC_A_UIRoute.Report;
using System.IO;

namespace MVC_A_UIRoute.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]

        #region LoadData
        public JsonResult LoadData(BookModel Param)
        {
            List<BookModel> BookList = new List<BookModel>();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbSqlCon"].ConnectionString))
            {
                var cmd = new SqlCommand("BookMaster_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Mode", SqlDbType.VarChar)).Value = Param.Mode;
                cmd.Parameters.Add(new SqlParameter("@BookCode", SqlDbType.VarChar)).Value = Param.BookCode;
                try
                {
                   con.Open();
                    using (SqlDataReader DbReader = cmd.ExecuteReader())
                        if (DbReader.HasRows)
                        {
                            while (DbReader.Read())
                            {
                                BookModel Books = new BookModel();
                                Books.BookCode = DbReader.GetString(DbReader.GetOrdinal("BookCode"));
                                Books.BookName = DbReader.GetString(DbReader.GetOrdinal("BookName"));
                                Books.BookDesc = DbReader.GetString(DbReader.GetOrdinal("BookDesc"));
                                Books.BookAuthor = DbReader.GetString(DbReader.GetOrdinal("BookAuthor"));
                                BookList.Add(Books);
                            }
                        }
                    return Json(BookList, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                        con.Close();
                }
            }
        }
        #endregion

        [HttpPost]
        #region EditData
        public string EditData(BookModel Param)
        {
            if (Param != null)
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbSqlCon"].ConnectionString))
                {
                    var cmd = new SqlCommand("BookMaster_SP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mode", SqlDbType.VarChar)).Value = Param.Mode;
                    cmd.Parameters.Add(new SqlParameter("@BookCode", SqlDbType.VarChar)).Value = Param.BookCode;
                    cmd.Parameters.Add(new SqlParameter("@BookName", SqlDbType.VarChar)).Value = Param.BookName;
                    cmd.Parameters.Add(new SqlParameter("@BookDesc", SqlDbType.VarChar)).Value = Param.BookDesc;
                    cmd.Parameters.Add(new SqlParameter("@BookAutor", SqlDbType.VarChar)).Value = Param.BookAuthor;
                    
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return "Success";
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();

                    }

                }
            }

            else
            {
                return "Model Error";
            }
        }
        #endregion
        
        public ActionResult ExportExcel()
        {
            List<BookModel> BookList = new List<BookModel>();
            DataSetReport DsReport = new DataSetReport();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbSqlCon"].ConnectionString))
            {
                var cmd = new SqlCommand("BookMaster_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Mode", SqlDbType.VarChar)).Value = "GET";
                con.Open();
              
                (new SqlDataAdapter(cmd)).Fill(DsReport.Tables["BookList"]);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportBookList.rpt"));
            rd.SetDataSource(DsReport.Tables["BookList"]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ReportBookList.xlsx");
        }

        public ActionResult ExportPdf()
        {
            List<BookModel> BookList = new List<BookModel>();
            DataSetReport DsReport = new DataSetReport();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbSqlCon"].ConnectionString))
            {
                var cmd = new SqlCommand("BookMaster_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Mode", SqlDbType.VarChar)).Value = "GET";
                con.Open();

                (new SqlDataAdapter(cmd)).Fill(DsReport.Tables["BookList"]);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportBookList.rpt"));
            rd.SetDataSource(DsReport.Tables["BookList"]);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ReportBookList.Pdf");
        }



    }
}