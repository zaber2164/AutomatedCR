using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedCR.DbFile;
using Newtonsoft.Json;
//using NsExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.ComponentModel;

namespace AutomatedCR.Controllers
{
    public class CourseController : Controller
    {
        private readonly AutomatedCrConnection dbEntities = new AutomatedCrConnection();
        // GET: Course
        public ActionResult Course()
        {
            try
            {
                List<Course> CourseList =
                    dbEntities.Courses.ToList();
                if (CourseList.Count>0)
                {
                    return View(CourseList);
                }
                else
                {
                    return View(new List<Course>());
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetCourseById(long Id)
        {
            try
            {
                Course crs = dbEntities.Courses.Where(e => e.CourseId == Id).FirstOrDefault();

                String data = JsonConvert.SerializeObject(crs, Formatting.None);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Add(Course Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    dbEntities.Courses.Add(Data);
                    dbEntities.SaveChanges();
                    int id = Data.CourseId;

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
        public ActionResult Update(Course Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    var course = dbEntities.Courses.FirstOrDefault(e => e.CourseId == Data.CourseId);

                    course.Title = Data.Title;
                    course.TeacherId = Data.TeacherId;
                    course.Time = Data.Time;
                    course.Location = Data.Location;
                    course.Semester = Data.Semester;

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
        [HttpPost]
        public ActionResult UpdateTableData()
        {
            try
            {
                List<Course> courseList =
                    dbEntities.Courses.ToList();

                return Json(courseList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadDdlTeacher()
        {
            try
            {
                List<Teacher> TeacherList =
                    dbEntities.Teachers.ToList();
                if (TeacherList.Count > 0)
                {
                    return Json(TeacherList,JsonRequestBehavior.AllowGet);
                }
                else return Json(new List<Teacher>(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new List<Teacher>(), JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        public ActionResult ExportExcel()
        {
            //try
            //{
                List<Course> courseList =
                    dbEntities.Courses.ToList();
                //
                DataTable dtRawData = ConvertToDataTable(courseList);
                var grid = new GridView();
                grid.DataSource = dtRawData;
                grid.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                string fileName = "Course" + "_" + DateTime.Now.ToString("yyyyMMdd HH:mm");
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                return RedirectToAction("Course", "Course");
                //return View("Course");

                //return Json(courseList, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return Json(e.Message, JsonRequestBehavior.AllowGet);
            //}
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