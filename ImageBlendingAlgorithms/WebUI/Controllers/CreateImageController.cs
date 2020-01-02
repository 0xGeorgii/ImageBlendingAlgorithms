using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBALib.Interfaces;
using IBALib.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Utils;

namespace WebUI.Controllers
{
    public class CreateImageController : Controller
    {
        // GET: CreateImage
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateImage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateImage/Create
        public ActionResult Create()
        {
            ViewData["algorithms"] = new List<string>(10);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var algorithms = assembly
                    .GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(ImageBlendingAlgorithmAttribute), false).Length > 0)
                    .Select(t => t.Name.SplitCamelCase()).ToList();
                if (algorithms.Any())
                    (ViewData["algorithms"] as List<string>).AddRange(algorithms);
            }
            return View();
        }

        // POST: CreateImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateImage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateImage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateImage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}