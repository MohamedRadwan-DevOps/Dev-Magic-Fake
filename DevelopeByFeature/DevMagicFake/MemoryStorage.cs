// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStorage.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The memory storage class, this class will provide the main storage for Dev Magic Fake, we can always find any saved instance here, we can easily query this class using LINQ
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System.Collections.Generic;

#endregion

namespace M.Radwan.DevMagicFake
{
    /// <summary>
    /// The memory storage.
    /// </summary>
    public class MemoryStorage
    {
        #region Constants and Fields

        /// <summary>
        /// The memory db property which hold all saved instances, the structure of the MemoryDb is a dictionary, the key is the name of the type like (Customer, Student, Employee) and the value is a collection of instance of the key type.
        /// </summary>
        private static Dictionary<string, List<dynamic>> memoryDb = new Dictionary<string, List<dynamic>>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets MemoryDb.
        /// </summary>
        public static Dictionary<string, List<dynamic>> MemoryDb
        {
            get
            {
                return memoryDb;
            }

            set
            {
                memoryDb = value;
            }
        }

        #endregion
    }
}