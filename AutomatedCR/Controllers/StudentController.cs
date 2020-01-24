using AutomatedCR.DbFile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AutomatedCR.Controllers
{
    public class StudentController : Controller
    {
        private readonly AutomatedCrConnection dbEntities = new AutomatedCrConnection();
        // GET: Student
        public ActionResult Index()
        {
            try
            {
                List<Student> studentList =
                    dbEntities.Students.ToList();
                if (studentList.Count==0 || studentList==null)
                {
                    studentList=new List<Student>();
                }

                return View(studentList);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetStudentById(long Id)
        {
            try
            {
                Student std = dbEntities.Students.Where(e => e.StudentId == Id).FirstOrDefault();

                String data = JsonConvert.SerializeObject(std, Formatting.None);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Add(Student Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    dbEntities.Students.Add(Data);
                    dbEntities.SaveChanges();
                    int id = Data.StudentId;

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Data not found.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult Update(Student Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    var student = dbEntities.Students.FirstOrDefault(e => e.StudentId == Data.StudentId);

                    student.Name = Data.Name;
                    student.Email = Data.Email;
                    student.PhoneNumber = Data.PhoneNumber;
                    student.UpdatedDate = Data.UpdatedDate;
                    student.UpdatedBy = Data.UpdatedBy;

                    dbEntities.SaveChanges();

                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Data not found.", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UpdateTableData()
        {
            try
            {
                List<Student> studentList =
                    dbEntities.Students.ToList();

                return Json(studentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportExcel()
        {
            List<Student> StudentList =
                dbEntities.Students.ToList();
            //
            DataTable dtRawData = ConvertToDataTable(StudentList);
            var grid = new GridView();
            grid.DataSource = dtRawData;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "Student" + "_" + DateTime.Now.ToString("yyyyMMdd HH:mm");
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Student");
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =

            TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)

            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)

                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;

        }
    }
}