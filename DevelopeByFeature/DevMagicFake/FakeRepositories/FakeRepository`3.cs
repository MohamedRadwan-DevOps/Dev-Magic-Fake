﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeRepository`3.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The fake repository class will has all the feature of other fake repositories classes but it's a default for complex type that has two nested types, remember it
//   inherit from BaseFakeRepostiroy which give it all features for CRUD and fake any simple or complex type but it's default is complex type that has two nested types
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using M.Radwan.DevMagicFake.Abstract;
using M.Radwan.DevMagicFake.DataGeneration;

#endregion

namespace M.Radwan.DevMagicFake.FakeRepositories
{
    /// <summary>
    /// The fake repository class will has all the feature of other fake repositories classes but it's a default for complex type that has two nested types, remember it 
    ///   inherit from BaseFakeRepostiroy which give it all features for CRUD and fake any simple or complex type but it's default is complex type that has two nested types
    /// </summary>
    /// <typeparam name="T">
    /// The Main object that we will need to save or link any nested type to it
    /// </typeparam>
    /// <typeparam name="TX">
    /// The first nested type that we will need to link it to the Main type
    /// </typeparam>
    /// <typeparam name="TY">
    /// The second nested type that we will need to link it to the Main type
    /// </typeparam>
    public class FakeRepository<T, TX, TY> : BaseFakeRepository, IRepository<T> where T : class
    {
        #region Public Methods

        /// <summary>
        /// The save method, this method will save the list of the Main objects and retrieve the nested objects by the Id and link it to the main object, for example 
        ///   if we have scenario like we have Vendor that provide Products and services that installed on specific product, we entered all Vendors, then we enter all products 
        ///   for each Vendor and then we will enter the services and choose the appropriate product from dropdown list that has Id and name of the product, so we don't need 
        ///   to Add the Vendor or the product because they  already saved, we just want to save the service and link it to this vendor and the product, so the DevMagicFake 
        ///   will retrieve the Vendor by Id and retrieve the product by Id and save them to the properties Vendor and product in the service and then save the service into 
        ///   the MemroyDb
        /// </summary>
        /// <param name="list">
        /// The list of the Main objects we want to save.
        /// </param>
        /// <returns>
        /// return the list of the Main objects and each object in the list included it's new Id generated by DevMagicFake in cases of Add and link all nested objects
        /// </returns>
        public List<T> Add(List<T> list)
        {
            foreach (T obj in list)
            {
                this.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// The save all, this method will Add the main object and it's nested object, so it will not retrieve the nested objects by their Ids, instead it will save 
        ///   the nested objects with new Id if the Id is 0, or update the current objects in the MemoryDb in case the Id already exists
        /// </summary>
        /// <param name="entity">
        /// The Main object that has the nested objects that we want to save with nested.
        /// </param>
        /// <returns>
        /// return the Main object that include a new Id generated by DevMagicFake in case of new object and include the nested objects that include new Ids also that 
        ///   generated by DevMagicFake
        /// </returns>
        public T AddAll(T entity)
        {
            var fakeRepository1 = new FakeRepository<T, TX>();
            fakeRepository1.AddAll(entity);

            var fakeRepository2 = new FakeRepository<T, TY>();
            fakeRepository2.AddAll(entity);

            return entity;
        }

        /// <summary>
        /// The save all, this method will Add a list of the main objects and the their nested objects, so it will not retrieve the nested objects by their Ids instead 
        ///   it will save the nested objects with new Ids if the Id is 0, or update the current objects in the MemoryDb in case the Ids exist
        /// </summary>
        /// <param name="list">
        /// The list of main object that has the nested objects that we want to save with nested.
        /// </param>
        /// <returns>
        /// return a list of the main objects that include a new Id for each main object generated by DevMagicFake in case of new object and include the nested objects 
        ///   that include new Ids also for each nested in each main object
        /// </returns>
        public List<T> AddAll(List<T> list)
        {
            foreach (T obj in list)
            {
                this.AddAll(obj);
            }

            return list;
        }

        /// <summary>
        /// The rule uses class property.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <typeparam name="M">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public FakeRepository<T, TX, TY> RuleUsesClassProperty<M>(Expression<Func<T, M>> predicate, Expression<Func<DataGenerationOption, List<M>>> option)
        {
            return (FakeRepository<T, TX, TY>)base.RuleUsesClassProperty(predicate, option);
        }

        #endregion

        #region Implemented Interfaces

        #region IRepository<T>

        /// <summary>
        /// The Add method one of the main feature of the Dev Magic Fake, it does many of the magic stuff so it's save a new instance if it's Id is 0 or if it's Id is not exist, it update the instance if it's Id existing in the MemoryDb
        /// </summary>
        /// <param name="entity">
        /// The object that we want to save or update.
        /// </param>
        /// <returns>
        /// return the object itself, if this is a new object DevMagicFake will assign a new Id incremental for each type,  if this is an existing object, it will just updated
        /// </returns>
        public T Add(T entity)
        {
            var fakeRepository1 = new FakeRepository<T, TX>();
            fakeRepository1.Add(entity);

            var fakeRepository2 = new FakeRepository<T, TY>();
            fakeRepository2.Add(entity);

            return entity;
        }

        /// <summary>
        /// The Get method will get an instance based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <returns>
        /// Instance of type T
        /// </returns>
        public T Get(Expression<Func<T, bool>> where)
        {
            return this.Get<T>(where);
        }

        /// <summary>
        /// Get All object of Type T from MemoryDb, even if we create Fake of T, we don't need this method we can direct query the MemoryStorge <see cref="MemoryStorage"/> using LINQ
        /// </summary>
        /// <returns>
        /// Return a list of all object from Type T in the MemoryDb
        /// </returns>
        public IEnumerable<T> GetAll()
        {
            return this.GetAll<T>();
        }

        /// <summary>
        /// Get an object by code, but remember the object must has a property it's name is code and of type string
        /// </summary>
        /// <param name="code">
        /// The code that we will use to search to get the object that has it
        /// </param>
        /// <returns>
        /// The object that match the code parameter
        /// </returns>
        public T GetByCode(string code)
        {
            return this.GetByCode<T>(code);
        }

        /// <summary>
        /// Get an object by Id, but remember the object must has a property it's name is Id and of type long
        /// </summary>
        /// <param name="id">
        /// The id that we want to search by.
        /// </param>
        /// <returns>
        /// The object that match the Id and from Type T
        /// </returns>
        public T GetById(long id)
        {
            return this.GetById<T>(id);
        }

        /// <summary>
        /// The GetMany method will get many of objects or list of objects based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <returns>
        /// List of objects from type T
        /// </returns>
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return this.GetMany<T>(where);
        }

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="entity">
        /// The entity that we want to remove
        /// </param>
        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="where">
        /// The Expression that will be evaluated, the method will delete any object evaluate true for this expression
        /// </param>
        public void Remove(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}