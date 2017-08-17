// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utilitie.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

using M.Radwan.DevMagicFake.Configuration;
using M.Radwan.DevMagicFake.FakeRepositories;

#endregion

namespace M.Radwan.DevMagicFake.Utilities
{
    /// <summary>
    /// The utilities.
    /// </summary>
    public class Utilitie
    {
        #region Constants and Fields

        /// <summary>
        ///   The Framework settings.
        /// </summary>
        private static readonly FrameworkSettings FrameworkSettings = FrameworkSettings.FrameworkSettingsInstance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "Utilitie" /> class.
        /// </summary>
        static Utilitie()
        {
            GetCurrentExecutionPath();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The binary deserialize method will get the object graph from the HDD and deserialize it with it's original state
        /// </summary>
        public static void BinaryDeserialize()
        {
            var db = new Dictionary<string, List<dynamic>>();
            try
            {
                FileStream fileStream = File.Open(FrameworkSettings.CurrentExecutionPath + @"\MemoryDB.binary", FileMode.Open);
                fileStream.Position = 0;
                var bf = new BinaryFormatter();
                db = (Dictionary<string, List<dynamic>>)bf.Deserialize(fileStream);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
            }

            MemoryStorage.MemoryDb = db;
        }

        /// <summary>
        /// The binary serialize method will serial all object graph with all it's references so we can deserialize it to the memory later
        /// </summary>
        public static void BinarySerialize()
        {
            Dictionary<string, List<object>> db = MemoryStorage.MemoryDb;
            FileStream fileStream = File.Open(FrameworkSettings.CurrentExecutionPath + @"\MemoryDB.binary", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fileStream, db);

            // fileStream.Flush();
            // fileStream.Close();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get current execution path.
        /// </summary>
        /// <returns>
        /// The current execution path.
        /// </returns>
        internal static string GetCurrentExecutionPath()
        {
            string currentExecutionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (currentExecutionPath != null)
            {
                if (currentExecutionPath.Substring(0, 6).ToLower() == @"file:\")
                {
                    currentExecutionPath = currentExecutionPath.Substring(6);
                }
            }

            return currentExecutionPath;
        }

        /// <summary>
        /// The try load assembly.
        /// </summary>
        /// <param name="path">
        /// The path of the assembly to be loaded.
        /// </param>
        /// <returns>
        /// The assembly if success otherwise it will return null
        /// </returns>
        internal static Assembly TryLoadAssembly(string path)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                return assembly;
            }
            catch (FileNotFoundException e)
            {
                return null;
            }
        }

        #endregion
    }
}
