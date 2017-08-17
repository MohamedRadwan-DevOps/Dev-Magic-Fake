// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using System.Web.Mvc;

using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.Domain.Entities;
using M.Radwan.DevMagicFake.Extensions;
#endregion

namespace M.Radwan.TryFakeMVC3.Controllers
{
    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        #region Public Methods

        /// <summary>
        /// The about.
        /// </summary>
        /// <returns>
        /// return the view of the about page
        /// </returns>
        public ActionResult About()
        {
            return this.View();
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// return view of the index page
        /// </returns>
        public ActionResult Index()
        {
            this.ViewBag.Message = "Welcome to ASP.NET MVC!";

            var list = new List<string>() { "Radwan", "Seif", "Lara", "Rania" };
            
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Name, o => o.GeneratFromList(list).NullPercentage(0.5));
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRange(100000, 100200).NullPercentage(0.5));
            repository.RuleUsesClassProperty(v => v.Address, o => o.GenerateFromValue("ABCDEF", 3).NullPercentage(0.2));


            repository.GenerateDataForAllAssemblyTypes(3);
            return this.View();
        }

        #endregion
    }
}
