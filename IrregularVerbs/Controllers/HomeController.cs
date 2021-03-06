using IrregularVerbs.Models;
using IrregularVerbs.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IrregularVerbs.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IVerbRepository _verbRepository;
        private readonly IrregularVerbsContext _context;
        private static List<int> selectedVerbs = new List<int>();
        private static List<IncorrectForm> incorrectForms = new List<IncorrectForm>();
        private int verbsLeft;
        private static Random random = new Random();

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, IVerbRepository verbRepository, IrregularVerbsContext context)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _verbRepository = verbRepository;
            _context = context;
        }

        // Main Web Page
        [Route("~/Home")]
        [Route("~/")]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("Date", string.Empty);
            selectedVerbs.Clear();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Training/Test Web Page
        public IActionResult SessionPage()
        {
            if(HttpContext.Session.GetString("Date") == string.Empty)
            {
                HttpContext.Session.SetString("Date", DateTime.Now.ToString());
            }

            //Select a random verb from database
            Random random = new Random();
            int max = _context.Irregulars.Count(), selected;
             selected = random.Next(1, max + 1);

            verbsLeft = max - selectedVerbs.Count();

            if (verbsLeft > 0 && selectedVerbs.Count() > 0)
            {
                while (selectedVerbs.Contains(selected))
                {
                    selected = random.Next(1, max + 1);
                }
                selectedVerbs.Add(selected);
            }

            if (selectedVerbs.Count() == 0)
            {
                selectedVerbs.Add(selected);
            }

            verbsLeft = max - selectedVerbs.Count();
            var selectedVerb = _context.Irregulars.Find(selected);
            var verb = _verbRepository.GetVerb(selectedVerb, verbsLeft);

            return View(verb);
        }

        // Answer Checker
        [HttpPost]
        public IActionResult CheckSubmit(SubmitForm submit)
        {
            string getDate = HttpContext.Session.GetString("Date");
            DateTime TimeStamp = DateTime.Parse(getDate);

            if (submit.VerbsLeft <= 0)
            {
                _verbRepository.CheckSubmittedForm(TimeStamp, submit);
                return RedirectToAction("Index");
            }
            else
                _verbRepository.CheckSubmittedForm(TimeStamp, submit);

            return RedirectToAction("SessionPage");
        }

        // Get All Incorrect Results
        public IActionResult IncorrectPage()
        {
            var data = _verbRepository.GetIncorrectSubmissions();

            IncorrectForm formData = new IncorrectForm()
            {
                IncorrectFormList = data          
            };

            return View(formData);
        }

        // Get All Results
        public IActionResult ResultPage()
        {
            var data = _verbRepository.GetResults();

            ResultForm formData = new ResultForm()
            {
                ResultFormList = data
            };

            return View(formData);
        }

        [HttpPost]
        public JsonResult getSpecificIncorrects(DateTime TimeStamp)
        {
            var data = _verbRepository.GetSpecificResult(TimeStamp);
            incorrectForms = data;

            return Json("True");
        }

        //Get a Specific Result
        public IActionResult SpecificResultPage()
        {
            IncorrectForm formData = new IncorrectForm()
            {
                IncorrectFormList = incorrectForms
            };

            return View(formData);
        }
    }
}