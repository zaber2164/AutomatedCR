using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedCR.DbFile;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AutomatedCR.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AutomatedCrConnection dbEntities = new AutomatedCrConnection();
        // GET: Teacher
        public ActionResult Index()
        {
            try
            {
                List<Teacher> teacherList =
                    dbEntities.Teachers.ToList();
                if (teacherList.Count == 0 || teacherList == null)
                {
                    teacherList = new List<Teacher>();
                }

                return View(teacherList);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetTeacherById(long Id)
        {
            try
            {
                Teacher std = dbEntities.Teachers.Where(e => e.TeacherId == Id).FirstOrDefault();

                String data = JsonConvert.SerializeObject(std, Formatting.None);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Add(Teacher Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    dbEntities.Teachers.Add(Data);
                    dbEntities.SaveChanges();
                    int id = Data.TeacherId;

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
        public ActionResult Update(Teacher Data)
        {
            try
            {
                //Int64 userid = Convert.ToInt32(Session["UserId"]);
                if (Data != null)
                {
                    var Teacher = dbEntities.Teachers.FirstOrDefault(e => e.TeacherId == Data.TeacherId);

                    Teacher.Name = Data.Name;
                    Teacher.Email = Data.Email;
                    Teacher.PhoneNumber = Data.PhoneNumber;
                    Teacher.UpdatedDate = Data.UpdatedDate;
                    Teacher.UpdatedBy = Data.UpdatedBy;

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
                List<Teacher> TeacherList =
                    dbEntities.Teachers.ToList();

                return Json(TeacherList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportExcel()
        {
            List<Teacher> TeacherList =
                dbEntities.Teachers.ToList();
            //
            DataTable dtRawData = ConvertToDataTable(TeacherList);
            var grid = new GridView();
            grid.DataSource = dtRawData;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "Teacher" + "_" + DateTime.Now.ToString("yyyyMMdd HH:mm");
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Teacher");
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
