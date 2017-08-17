// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceController.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The service controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using System.Web.Mvc;

using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.Domain.Entities;

#endregion

namespace M.Radwan.TryFakeMVC3.Controllers
{
    /// <summary>
    /// The service controller.
    /// </summary>
    public class ServiceController : Controller
    {
        // GET: /Service/
        #region Public Methods

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// return the create page
        /// </returns>
        public ActionResult Create()
        {
            var fakeRepository = new FakeRepository<VendorForm>();
            this.FillDropDown(fakeRepository);
            return this.View();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="serviceForm">
        /// The service form.
        /// </param>
        /// <returns>
        /// return the create page
        /// </returns>
        [HttpPost]
        public ActionResult Create(ServiceForm serviceForm)
        {
            var fakeRepository = new FakeRepository<ServiceForm, VendorForm, ProductTypeForm>();
            fakeRepository.Add(serviceForm);
            this.FillDropDown(new FakeRepository<VendorForm>());
            return View("Page", fakeRepository.GetAll());
        }

        /// <summary>
        /// The details.
        /// </summary>
        /// <param name="id">
        /// The id of the object.
        /// </param>
        /// <returns>
        /// return the Details page
        /// </returns>
        public ActionResult Details(int id)
        {
            var fakeRepository = new FakeRepository<ServiceForm>();
            ServiceForm serviceForm = fakeRepository.GetById(id);

            this.ViewData.Model = serviceForm;
            return this.View();

        }

        /// <summary>
        /// The edit page for the object.
        /// </summary>
        /// <param name="id">
        /// The id of the edit object.
        /// </param>
        /// <returns>
        /// return the edit page
        /// </returns>
        public ActionResult Edit(int id)
        {
            var fakeRepository = new FakeRepository<ServiceForm>();
            ServiceForm serviceForm = fakeRepository.GetById(id);
            this.ViewData.Model = serviceForm;
            this.ViewData["vendorDropList"] = new List<VendorForm> { serviceForm.VendorForm };
            this.ViewData["productTypeDropList"] = new List<ProductTypeForm> { serviceForm.ProductTypeForm };
            return this.View();
        }

        /// <summary>
        /// The edit page.
        /// </summary>
        /// <param name="serviceForm">
        /// The service form.
        /// </param>
        /// <returns>
        /// return the edit page
        /// </returns>
        [HttpPost]
        public ActionResult Edit(ServiceForm serviceForm)
        {
            var fakeRepository = new FakeRepository<ServiceForm, VendorForm, ProductTypeForm>();

            // if(this.ModelState.IsValid)
            // {
            // Update(); 
            fakeRepository.Add(serviceForm);

            // }
            this.FillDropDown(new FakeRepository<VendorForm>());
            return View(serviceForm);
        }

        /// <summary>
        /// The page.
        /// </summary>
        /// <returns>
        /// </returns>
        public ActionResult Page()
        {
            var fakeRepository = new FakeRepository<ServiceForm>();
            var list = fakeRepository.GetAll();
            return View(list);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The fill drop down.
        /// </summary>
        /// <param name="fakeRepository">
        /// The fake repository.
        /// </param>
        private void FillDropDown(FakeRepository<VendorForm> fakeRepository)
        {
            IEnumerable<VendorForm> vendorDropLists = fakeRepository.GetAll();
            this.ViewData["vendorDropList"] = vendorDropLists;
            List<ProductTypeForm> productTypeDropLists = new List<ProductTypeForm>(); 
            this.ViewData["productTypeDropList"] = productTypeDropLists;
        }

        #endregion
    }
}
