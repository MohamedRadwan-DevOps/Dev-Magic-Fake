// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeRepository`1.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The fake repository class will has all the feature of other fake repositories classes but it's a default for a simple type, remember it inherit from BaseFakeRepostiroy which
//   give it all features for CRUD and fake any simple or complex type but it's default is one simple type
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
    /// The fake repository class will has all the feature of other fake repositories classes but it's a default for a simple type, remember it inherit from BaseFakeRepostiroy which
    ///   give it all features for CRUD and fake any simple or complex type but it's default is one simple type
    /// </summary>
    /// <typeparam name="T">
    /// The type that will be used throughout all methods of an instance of this class 
    /// </typeparam>
    public class FakeRepository<T> : BaseFakeRepository, IRepository<T> where T : class
    {
        #region Public Methods

        /// <summary>
        /// The same as the normal Add method but this method take a list for fast saving collection
        /// </summary>
        /// <param name="list">
        /// The list that we want to save to MemroyDb.
        /// </param>
        /// <returns>
        /// The list of the objects that saved, included it's Id property for each object with the new assigned Id from Dev Magic Fake in case of new object
        /// </returns>
        public IEnumerable<T> Add(List<T> list)
        {
            return this.Add<T>(list);
        }

        /// <summary>
        /// This method will create instance of type (T) and generate data based on the configuration file and return this instance
        /// </summary>
        /// <returns>
        /// The instance of the created type with generated data
        /// </returns>
        public T Create()
        {
            return DataGenerationManager.CreateObjectWithGeneratedData(typeof(T), this.frameworkSettings.Assembly, 1, false);
        }

        /// <summary>
        /// This method will create a list of type (T) and generate data based on the configuration file and return this list
        /// </summary>
        /// <param name="numberOfObject">
        /// The number of object.
        /// </param>
        /// <returns>
        /// Return a list of created instance of type (T) with generated data
        /// </returns>
        public IEnumerable<T> Create(int numberOfObject)
        {
            var list = new List<T>();
            for (int i = 0; i < numberOfObject; i++)
            {
                list.Add(DataGenerationManager.CreateObjectWithGeneratedData(typeof(T), this.frameworkSettings.Assembly, numberOfObject, false));
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
        public FakeRepository<T> RuleUsesClassProperty<M>(Expression<Func<T, M>> predicate, Expression<Func<DataGenerationOption, List<M>>> option)
        {
            return (FakeRepository<T>)base.RuleUsesClassProperty(predicate, option);
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
            return this.Add<T>(entity);
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
            string typeName = typeof(T).FullName;
            var list = new List<T>();
            RepositoryUtilities.GetAllObjects(this.MemoryDb, typeName, list);
            return list;
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
            return RepositoryUtilities.GetObjectByCode<T>(this.MemoryDb, code);
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
            return RepositoryUtilities.GetObjectById<T>(this.MemoryDb, id);
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
            this.Remove<T>(entity);
        }

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="where">
        /// The Expression that will be evaluated, the method will delete any object evaluate true for this expression
        /// </param>
        public void Remove(Expression<Func<T, bool>> where)
        {
            this.Remove<T>(where);
        }

        #endregion

        #endregion
    }
}