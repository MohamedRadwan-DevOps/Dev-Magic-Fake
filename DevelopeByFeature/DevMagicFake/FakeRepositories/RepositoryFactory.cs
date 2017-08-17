// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactory.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The repository factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;

#endregion

namespace M.Radwan.DevMagicFake.FakeRepositories
{
    /// <summary>
    /// The repository factory class, I will hold this feature because I don't need to use it till now, if there is sync problem because of threads, 
    /// I can use to generate we only have one instance of any repository or in other words it will be the singletons because the code below just make 
    /// sure that only one instance of each type can be exist.
    /// </summary>
    internal class RepositoryFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The repositories.
        /// </summary>
        private readonly Dictionary<string, BaseFakeRepository> repositories = new Dictionary<string, BaseFakeRepository>();

        #endregion

        #region Public Methods

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <returns>
        /// </returns>
        public BaseFakeRepository CreateRepository()
        {
            BaseFakeRepository repository;
            this.repositories.TryGetValue("Repository", out repository);

            if (repository != null)
            {
                return repository;
            }

            repository = new FakeRepository();
            this.repositories.Add("Repository", repository);
            return repository;
        }

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <param name="repositoryType">
        /// The repository type.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public BaseFakeRepository CreateRepository(Type repositoryType)
        {
            if (repositoryType == null)
            {
                throw new ArgumentException("Type parameter can't be null");
            }

            BaseFakeRepository repository;
            this.repositories.TryGetValue(repositoryType.FullName, out repository);

            if (repository != null)
            {
                return repository;
            }

            var fakeRepositoryType = typeof(FakeRepository<>).MakeGenericType(repositoryType);
            repository = (BaseFakeRepository)Activator.CreateInstance(fakeRepositoryType);
            this.repositories.Add(repositoryType.FullName, repository);
            return repository;
        }

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <param name="repositoryFirstType">
        /// The repository first type.
        /// </param>
        /// <param name="repositorySecondType">
        /// The repository second type.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public BaseFakeRepository CreateRepository(Type repositoryFirstType, Type repositorySecondType)
        {
            if (repositoryFirstType == null || repositorySecondType == null)
            {
                throw new ArgumentException("Type parameters can't be null");
            }


            BaseFakeRepository repository;
            this.repositories.TryGetValue(repositoryFirstType.FullName + "|" + repositorySecondType.FullName, out repository);

            if (repository != null)
            {
                return repository;
            }

            var fakeRepositoryType = typeof(FakeRepository<,>).MakeGenericType(repositoryFirstType, repositorySecondType);
            repository = (BaseFakeRepository)Activator.CreateInstance(fakeRepositoryType);
            this.repositories.Add(repositoryFirstType.FullName + "|" + repositorySecondType.FullName, repository);
            return repository;
        }

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <param name="repositoryFirstType">
        /// The repository first type.
        /// </param>
        /// <param name="repositorySecondType">
        /// The repository second type.
        /// </param>
        /// <param name="repositoryThirdType">
        /// The repository third type.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public BaseFakeRepository CreateRepository(Type repositoryFirstType, Type repositorySecondType, Type repositoryThirdType)
        {
            if (repositoryFirstType == null || repositorySecondType == null || repositoryThirdType == null)
            {
                throw new ArgumentException("Type parameters can't be null");
            }


            BaseFakeRepository repository;
            this.repositories.TryGetValue(repositoryFirstType.FullName + "|" + repositorySecondType.FullName + "|" + repositoryThirdType.FullName, out repository);

            if (repository != null)
            {
                return repository;
            }

            var fakeRepositoryType = typeof(FakeRepository<,,>).MakeGenericType(repositoryFirstType, repositorySecondType, repositoryThirdType);
            repository = (BaseFakeRepository)Activator.CreateInstance(fakeRepositoryType);
            this.repositories.Add(repositoryFirstType.FullName + "|" + repositorySecondType.FullName + "|" + repositoryThirdType.FullName, repository);
            return repository;
        }

        #endregion
    }
}
