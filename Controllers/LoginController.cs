using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Controllers
{
    public class LoginController : Controller
    {
        private string project = "codebreakers-f72cc";
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"codebreakers-f72cc-firebase-adminsdk-mipsp-f44ddfe0b8.json";
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginProcessAsync(UserModel userModel)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);

            CollectionReference usersRef = db.Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                if (userModel.Email.Equals(documentDictionary["Email"]) && userModel.Password.Equals(documentDictionary["Password"]))
                {
                    switch ((string)documentDictionary["Role"])
                    {
                        case "student":
                            {
                                return RedirectToAction("Student", userModel);
                            }
                        case "admin":
                            {
                                return RedirectToAction("Admin", userModel);
                            }
                        default:
                            break;
                    }
                    break;
                }
            }
            return RedirectToAction("Error","Home");
        }
    }
}
