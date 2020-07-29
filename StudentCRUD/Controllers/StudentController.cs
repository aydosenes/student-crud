using Dapper;
using StudentCRUD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentCRUD.Controllers
{
    public class StudentController : Controller
    {
        #region DapperConnection
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public IDbConnection StudentDbContext
        {
            get
            {
                var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
                return conn;
            }
        }
        #endregion

        #region Read
        // GET: Student
        public ActionResult Index(string search)
        {
            using (var _db = StudentDbContext)
            {
                if(search != null)
                {
                    List<Student> _list = _db.Query<Student>("SELECT * FROM [dbo].[Student] WHERE [Name] LIKE '%"+search+"%'").ToList();
                    return View(_list);
                }
                List<Student> list = _db.Query<Student>("SELECT * FROM [dbo].[Student]").ToList();

                return View(list);
            }

        }
        #endregion

        #region Create
        // GET: Student/Create
        public ActionResult AddStudent()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult AddStudent(Student model)
        {
            try
            {
                // TODO: Add insert logic here
                using (var _db = StudentDbContext)
                {
                    Student student = new Student();
                    student.Name = model.Name;
                    student.Surname = model.Surname;
                    student.Age = model.Age;
                    student.ClassId = model.ClassId;

                    //Class item = _db.Query("SELECT * FROM [dbo].[Class] WHERE [ClassId] =" + model.ClassId);

                    //student.Class = item;
                    _db.Query<Student>(@"INSERT INTO[dbo].[Student]([Name],[Surname],[Age],[ClassId])
                                        VALUES(@Name, @Surname, @Age, @ClassId)", student);
                }


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        #endregion

        #region Update
        // GET: Student/Edit/5
        public ActionResult Edit(int id = 0)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(Student model)
        {
            try
            {
                // TODO: Add update logic here
                using (var _db = StudentDbContext)
                {
                    Student student = new Student();
                    student.Name = model.Name;
                    student.Surname = model.Surname;
                    student.Age = model.Age;
                    student.ClassId = model.ClassId;

                    _db.Query<Student>(@"UPDATE[dbo].[Student] 
                                SET [Name] = @Name,
                                    [Surname] = @Surname,
                                    [Age] = @Age,
                                    [ClassId] = @ClassId", student
                    );
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var _db = StudentDbContext)
            {//DELETE FROM [dbo].[Student] WHERE
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var student = _db.Query<Student>("SELECT * FROM [dbo].[Student] WHERE [StudentId] = " + id);
                if (student == null)
                {
                    return HttpNotFound();
                }

                return View();
            }

        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                // TODO: Add delete logic here
                using (var _db = StudentDbContext)
                {
                    var item = _db.Query<Student>(@"DELETE FROM [dbo].[Student] WHERE [StudentId] = " + id);

                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
