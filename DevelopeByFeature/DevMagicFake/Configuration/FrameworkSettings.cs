// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkSettings.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The framework settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Reflection;

using M.Radwan.DevMagicFake.Utilities;

#endregion

namespace M.Radwan.DevMagicFake.Configuration
{
    /// <summary>
    /// The framework settings.
    /// </summary>
    public class FrameworkSettings
    {
        #region Constants and Fields

        /// <summary>
        ///   The padlock.
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        ///   The framework settings instance.
        /// </summary>
        private static FrameworkSettings frameworkSettingsInstance;

        /// <summary>
        ///   The entities assembly.
        /// </summary>
        private string entitiesAssembly;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "FrameworkSettings" /> class.
        /// </summary>
        /// <exception cref = "Exception">
        /// </exception>
        static FrameworkSettings()
        {
            frameworkSettingsInstance = null;
            
        }

        /// <summary>
        ///   Prevents a default instance of the <see cref = "FrameworkSettings" /> class from being created.
        /// </summary>
        private FrameworkSettings()
        {
            
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets FrameworkSettingsInstance.
        /// </summary>
        public static FrameworkSettings FrameworkSettingsInstance
        {
            get
            {
                lock (Padlock)
                {
                    if (frameworkSettingsInstance == null)
                    {
                        frameworkSettingsInstance = new FrameworkSettings();
                        frameworkSettingsInstance.SetDefaultSettings();
                    }

                    return frameworkSettingsInstance;
                }
            }
        }

        /// <summary>
        ///   The assembly.
        /// </summary>
        internal Assembly Assembly { get; set; }

        /// <summary>
        ///   The current execution path.
        /// </summary>
        internal string CurrentExecutionPath { get; set; }

        /// <summary>
        ///   The CurrentRandom.
        /// </summary>
        internal Random CurrentRandom { get; set; }

        /// <summary>
        ///   The properties values.
        /// </summary>
        internal Dictionary<string, string> DataGenerationPrimaryRules { get; set; }

        /// <summary>
        ///   The entities assembly.
        /// </summary>
        internal string EntitiesAssembly
        {
            get
            {
                return this.entitiesAssembly;
            }

            set
            {
                this.entitiesAssembly = value;
                this.Assembly = Utilitie.TryLoadAssembly(this.CurrentExecutionPath + @"\" + this.EntitiesAssembly);
            }
        }

        /// <summary>
        ///   Gets or sets EntitiesNamespace.
        /// </summary>
        internal string EntitiesNamespace { get; set; }

        /// <summary>
        ///   Gets or sets MaximumObjectGraphLevel.
        /// </summary>
        internal int MaximumObjectGraphLevel { get; set; }

        /// <summary>
        ///   The RandomDynamicSeed.
        /// </summary>
        internal Random RandomDynamicSeed { get; set; }

        /// <summary>
        ///   The RandomFixedSeed.
        /// </summary>
        internal Random RandomFixedSeed { get; set; }

        /// <summary>
        ///   Gets or sets UseFakeableAttribute.
        /// </summary>
        internal bool UseFakeableAttribute { get; set; }

        /// <summary>
        ///   Gets or sets UseNotFakeableAttribute.
        /// </summary>
        internal bool UseNotFakeableAttribute { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The add rule.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        internal void AddRule(string key, string value)
        {
            bool keyIsExist = this.DataGenerationPrimaryRules.ContainsKey(key);
            if (keyIsExist)
            {
                this.DataGenerationPrimaryRules[key] = value;
            }
            else
            {
                this.DataGenerationPrimaryRules.Add(key, value);
            }
        }

        /// <summary>
        /// The set default settings.
        /// </summary>
        internal void SetDefaultSettings()
        {
            this.CurrentExecutionPath = Utilitie.GetCurrentExecutionPath();
            this.RandomDynamicSeed = new Random((int)DateTime.Now.Ticks); // thanks to McAden
            this.RandomFixedSeed = new Random(3);
            this.CurrentRandom = this.RandomFixedSeed;
            this.DataGenerationPrimaryRules = this.CreateDefaultDataGenerationPrimaryRules();
            this.EntitiesAssembly = "Domain.dll";
            this.MaximumObjectGraphLevel = 10000;
            this.UseFakeableAttribute = false;
            this.UseNotFakeableAttribute = false;
        }

        /// <summary>
        /// The set settings from config file.
        /// </summary>
        internal void SetSettingsFromConfigFile()
        {
            this.EntitiesNamespace = ConfigurationUtilities.GetEntitiesNamespaceFromConfig();
            this.DataGenerationPrimaryRules = ConfigurationUtilities.GetDataGenerationPrimaryRulesFromConfig();
            this.DataGenerationPrimaryRules = ConfigurationUtilities.GetDataGenerationSecondaryRulesFromConfig();
            this.EntitiesAssembly = ConfigurationUtilities.GetAssemblyNameFromConfig();
            this.MaximumObjectGraphLevel = ConfigurationUtilities.GetMaximumObjectGraphFromConfig();
            this.UseFakeableAttribute = ConfigurationUtilities.GetUseFakeableFromConfig();
            this.UseNotFakeableAttribute = ConfigurationUtilities.GetUseNotFakeableFromConfig();
        }

        /// <summary>
        /// The create default properties values.
        /// </summary>
        /// <returns>
        /// </returns>
        private Dictionary<string, string> CreateDefaultDataGenerationPrimaryRules()
        {
            var defaultDataGenerationPrimaryRules = new Dictionary<string, string>();
            defaultDataGenerationPrimaryRules.Add("Address", "Sawsan|Rania|Radwan|Heba|Null:0|GenerationType:List");
            defaultDataGenerationPrimaryRules.Add("Email", "M.Radwan@gmail.com|Rania@hotmail.com|Lara@live.com|Null:0|GenerationType:List");
            defaultDataGenerationPrimaryRules.Add("Name", "Seif|Lara|Rania|Radwan|Nadia|Null:0|GenerationType:List");
            return defaultDataGenerationPrimaryRules;
        }

        #endregion
    }
}
