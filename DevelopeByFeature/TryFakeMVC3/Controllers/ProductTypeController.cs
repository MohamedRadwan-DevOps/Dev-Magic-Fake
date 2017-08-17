// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductTypeController.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The product type controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.Domain.Entities;

#endregion

namespace M.Radwan.TryFakeMVC3.Controllers
{
    /// <summary>
    /// The product type controller.
    /// </summary>
    public class ProductTypeController : Controller
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
            var fakeRepository = new FakeRepository<VendorForm>();
            this.FillDropDown(fakeRepository);
            return this.View();
        }

        // POST: /ProductType/Create

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="productTypeForm">
        /// The product type form.
        /// </param>
        /// <returns>
        /// return the create page
        /// </returns>
        [HttpPost]
        public ActionResult Create(ProductTypeForm productTypeForm)
        {
            var repoistory = new FakeRepository<ProductTypeForm, VendorForm>();
            repoistory.Add(productTypeForm);
            this.FillDropDown(new FakeRepository<VendorForm>());
            return View("Page", repoistory.GetAll());
        }

        // GET: /ProductType/Edit/5

        /// <summary>
        /// The edit of the object.
        /// </summary>
        /// <param name="id">
        /// The id of the object.
        /// </param>
        /// <returns>
        /// return the edit page
        /// </returns>
        public ActionResult Edit(int id)
        {
            var fakeRepository = new FakeRepository<ProductTypeForm, VendorForm>();
            var productTypeForm = fakeRepository.GetById<ProductTypeForm>(id);
            this.ViewData.Model = productTypeForm;
            this.FillDropDown(new FakeRepository<VendorForm>());
            return this.View();
        }

        // POST: /ProductType/Edit/5

        /// <summary>
        /// The edit of the product type.
        /// </summary>
        /// <param name="productTypeForm">
        /// The product type form.
        /// </param>
        /// <returns>
        /// The view page of the 
        /// </returns>
        [HttpPost]
        public ActionResult Edit(ProductTypeForm productTypeForm)
        {
            var fakeRepository = new FakeRepository<ProductTypeForm, VendorForm>();

            // if(this.ModelState.IsValid)
            // {
            // Update(); 
            fakeRepository.Add(productTypeForm);

            // }
            this.FillDropDown(new FakeRepository<VendorForm>());
            return View(productTypeForm);
        }

        /// <summary>
        /// The get all product type by vendor id.
        /// </summary>
        /// <param name="vendorId">
        /// The vendor id.
        /// </param>
        /// <returns>
        /// the json of the product types
        /// </returns>
        [HttpPost]
        public virtual JsonResult GetAllProductTypeByVendorId(int vendorId)
        {
            var fakeRepository = new FakeRepository<ProductTypeForm>();
            IEnumerable<ProductTypeForm> items = fakeRepository.GetAll();
            var objects = items.Where(x => x.VendorForm.Id == vendorId);
            return this.Json(new { success = true, message = string.Empty, objects });
        }

        /// <summary>
        /// The page that return
        /// </summary>
        /// <returns>
        /// return the page
        /// </returns>
        public ActionResult Page()
        {
            var fakeRepository = new FakeRepository<ProductTypeForm>();
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
            var vendorDropLists = fakeRepository.GetAll();
            this.ViewData["vendorDropList"] = vendorDropLists;
            var productTypeDropLists = new List<ProductTypeForm>();
            this.ViewData["productTypeDropList"] = productTypeDropLists;
        }

        #endregion
    }
}
