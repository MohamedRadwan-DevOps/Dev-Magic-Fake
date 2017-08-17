// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The i repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace M.Radwan.DevMagicFake.Abstract
{
    /// <summary>
    /// The IRepository interface will be the interface for any repository, we can use this interface in our production during the testing or the faking time
    /// </summary>
    /// <typeparam name="T">
    /// The business class type that we will create this repository for
    /// </typeparam>
    public interface IRepository<T> where T : class
    {
        #region Public Methods

        /// <summary>
        /// The Add method one of the main feature of the Dev Magic Fake, it does many of the magic stuff so it's save a new instance if it's Id is 0 or if it's Id is not exist, it update the instance if it's Id existing in the MemoryDb
        /// </summary>
        /// <param name="entity">
        /// The object that we want to save or update.
        /// </param>
        /// <returns>
        /// return the object itself, if this is a new object DevMagicFake will assign a new Id incremental for each type,  if this is an existing object, it will just updated
        /// </returns>
        T Add(T entity);

        /// <summary>
        /// The Get method will get an instance based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <returns>
        /// Instance of type T
        /// </returns>
        T Get(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get All object of Type T from MemoryDb, even if we create Fake of T, we don't need this method we can direct query the MemoryStorge <see cref="MemoryStorage"/> using LINQ
        /// </summary>
        /// <returns>
        /// Return a list of all object from Type T in the MemoryDb
        /// </returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get an object by code, but remember the object must has a property it's name is code and of type string
        /// </summary>
        /// <param name="code">
        /// The code that we will use to search to get the object that has it
        /// </param>
        /// <returns>
        /// The object that match the code parameter
        /// </returns>
        T GetByCode(string code);

        /// <summary>
        /// Get an object by Id, but remember the object must has a property it's name is Id and of type long
        /// </summary>
        /// <param name="id">
        /// The id that we want to search by.
        /// </param>
        /// <returns>
        /// The object that match the Id and from Type T
        /// </returns>
        T GetById(long id);

        /// <summary>
        /// The GetMany method will get many of objects or list of objects based on Expression tree
        /// </summary>
        /// <param name="where">
        /// The where condition that filter the object from the repository as an Expression tree
        /// </param>
        /// <returns>
        /// List of objects from type T
        /// </returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="entity">
        /// The entity that we want to remove
        /// </param>
        void Remove(T entity);

        /// <summary>
        /// The remove method will delete an object from our repository
        /// </summary>
        /// <param name="where">
        /// The Expression that will be evaluated, the method will delete any object evaluate true for this expression
        /// </param>
        /// <typeparam name="T">
        /// The business class type that we want to remove from
        /// </typeparam>
        void Remove(Expression<Func<T, bool>> where);

        #endregion
    }
}
