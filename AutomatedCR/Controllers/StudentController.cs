using AutomatedCR.DbFile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}