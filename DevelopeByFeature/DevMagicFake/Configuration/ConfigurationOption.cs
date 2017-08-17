using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M.Radwan.DevMagicFake.Configuration
{
    public class ConfigurationOption
    {
        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetAssemblyNameThatContainClasses(string assemblyName)
        {
        }

        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetNamesapceThatContainClass(string namespaceName)
        {
        }

        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetUseFakeableAttribute(bool useIt)
        {
        }

        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetUseNotFakeableAttribute(bool useIt)
        {
        }

        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetMaximumObjectGraphLevel(int maximumLevel)
        {
        }

        // if I change this method I have to revise the code because the expression tree translation depend on using this method by name
        public void SetAssemblyPathThatContainClasses(string path)
        {
        }

        public void SetCurrentRandomToDynamic()
        {
        }

        public void SetCurrentRandomToFixed()
        {
        }

    }
}
