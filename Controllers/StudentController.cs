using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Controllers
{
    public class StudentController : Controller
    {
        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";
        public async Task<IActionResult> StudentAsync(UserModel userModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            CollectionReference usersRef = db.Collection("Student");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            StudentModel studentModel = new();

            foreach (var item in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = item.ToDictionary();
                if (userModel.Email.Equals(documentDictionary["Email"]))
                {
                    studentModel.Email = (string)documentDictionary["Email"];
                    studentModel.Name = (string)documentDictionary["Name"];
                    studentModel.PhoneNo = (string)documentDictionary["PhoneNo"];
                    studentModel.Department = (string)documentDictionary["Department"];
                    studentModel.Task = (string)documentDictionary["Task"];

                    return View(studentModel);
                }
            }

            return RedirectToAction("Error","Home");
        }
    }
}
