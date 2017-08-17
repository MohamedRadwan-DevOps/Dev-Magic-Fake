// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VendorController.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The vendor controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Web.Mvc;

using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.Domain.Entities;

#endregion

namespace M.Radwan.TryFakeMVC3.Controllers
{
    /// <summary>
    /// The vendor controller.
    /// </summary>
    public class VendorController : Controller
    {
        #region Public Methods

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// return the create page
        /// </returns>
        public ActionResult Create()
        {
            var repoistory = new FakeRepository<VendorForm>();

            // repoistory.GenrateDataForAllAssemblyTypes(3);
            return this.View();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="vendorForm">
        /// The vendor form.
        /// </param>
        /// <returns>
        /// return the create page
        /// </returns>
        [HttpPost]
        public ActionResult Create(VendorForm vendorForm)
        {
            var repoistory = new FakeRepository<VendorForm>();
            repoistory.Add(vendorForm);
            return View("Page", repoistory.GetAll());
        }

        /// <summary>
        /// The page of the items.
        /// </summary>
        /// <returns>
        /// return the list page
        /// </returns>
        public ActionResult Page()
        {
            var fakeRepository = new FakeRepository<VendorForm>();
            var list = fakeRepository.GetAll();
            return View(list);
        }

        #endregion
    }
}
