using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Task1.Models;
using Google.Cloud.Firestore;

namespace Task1.Controllers
{
    public class HomeController : Controller
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"sparkproject-18ee9-firebase-adminsdk-1l558-283120a6d6.json";
        private string project = "sparkproject-18ee9";


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllAsync()
        {
            List<Consumer> consumer = new();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);
            CollectionReference usersRef = db.Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Consumer temp = new();
                temp.CurrentBalance = (string)documentDictionary["CurrentBalance"];
                temp.Email = (string)documentDictionary["Email"];
                temp.Name = (string)documentDictionary["Name"];
                consumer.Add(temp);
            }

            return View("ViewAll", consumer);
        }

        public IActionResult ViewOne()
        {
            return View();
        }
        public async Task<IActionResult> ViewOneProcessAsync(Consumer consumer)
        {
            List<Consumer> customer = new();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);
            CollectionReference usersRef = db.Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {

                Dictionary<string, object> documentDictionary = document.ToDictionary();
                if (documentDictionary["Name"].Equals(consumer.Name))
                {
                    consumer.CurrentBalance = (string)documentDictionary["CurrentBalance"];
                    consumer.Email = (string)documentDictionary["Email"];
                    consumer.Name = (string)documentDictionary["Name"];
                    customer.Add(consumer);
                }
            }
            return View("ViewAll", customer);
        }

        public IActionResult TransferMoney()
        {
            return View();
        }

        public async Task<IActionResult> TransferMoneyProcessAsync(Consumer consumer)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create(project);
            CollectionReference usersRef = db.Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                DocumentReference docRef = null;
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                if (documentDictionary["Name"].Equals(consumer.Name))
                {
                    docRef = db.Collection("Users").Document(document.Id);
                    consumer.Email = (string)documentDictionary["Email"];
                    consumer.Name = (string)documentDictionary["Name"];
                    consumer.CurrentBalance = (int.Parse(consumer.CurrentBalance) + int.Parse((string)documentDictionary["CurrentBalance"])).ToString();
                    Dictionary<string, object> update = new Dictionary<string, object>
                    {
                        {"CurrentBalance", consumer.CurrentBalance },
                    };
                    await docRef.SetAsync(update, SetOptions.MergeAll);
                }
            }
            return RedirectToAction("ViewAll");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
