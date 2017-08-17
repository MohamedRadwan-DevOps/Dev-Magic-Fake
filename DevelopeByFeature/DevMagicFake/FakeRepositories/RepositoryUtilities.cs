// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryUtilities.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The repository utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace M.Radwan.DevMagicFake.FakeRepositories
{
    /// <summary>
    /// The repository utilities class, this is the main core of the functionality of DevMagicFake to apply DRY concept
    /// </summary>
    internal class RepositoryUtilities
    {
        #region public Methods

        /// <summary>
        /// Get object by id.
        /// </summary>
        /// <param name="memoryDb">
        /// The memory db, the place that will search in
        /// </param>
        /// <param name="id">
        /// The Id that we want to search by
        /// </param>
        /// <typeparam name="T">
        /// The type of the object that we search for
        /// </typeparam>
        /// <returns>
        /// The object that match the Id parameter
        /// </returns>
        public static T GetObjectById<T>(Dictionary<string, List<dynamic>> memoryDb, long id)
        {
            string typeName = typeof(T).FullName;
            if (typeName != null)
            {
                if (memoryDb.ContainsKey(typeName))
                {
                    var obj = memoryDb[typeName].Where(x => x.Id == id).FirstOrDefault();
                    return (T)obj;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Delete object by id.
        /// </summary>
        /// <param name="memoryDb">
        /// The memory db, the place that will search in
        /// </param>
        /// <param name="entity">
        /// The Id that we want to delete by
        /// </param>
        /// <typeparam name="T">
        /// The type of the object that we will delete from
        /// </typeparam>
        public static void DeleteObjectById<T>(Dictionary<string, List<dynamic>> memoryDb, T entity)
        {
            string objtypeName = typeof(T).FullName;
            PropertyInfo objectPropertyeInfo = typeof(T).GetProperty("Id");
            if (objectPropertyeInfo != null)
            {
                long objectId = (long)objectPropertyeInfo.GetValue(entity, null);

                if (objtypeName != null)
                {
                    if (memoryDb.ContainsKey(objtypeName))
                    {
                        /*
                         there is a big question here, shall I remove the object from anywhere?, so it will not be exist for any object reference it, 
                         * I can do this by assigning null to it, or just delete the item from the memeoryDb array so it can't be retrieval again but 
                         * it will be still exist for any other types that reference it, I think this could be configurable so the client of DevMagicFake 
                         * will choose a list of entities which need them to be delete for all reference and another list of entities 
                         * which will need them just delete the parent but not the child that reference by others
                         */
                        int objectIndex = memoryDb[objtypeName].FindIndex(x => x.Id == objectId);
                        memoryDb[objtypeName][objectIndex] = null;
                        memoryDb[objtypeName].RemoveAt(objectIndex); 
                    }
                }

                return;
            }

            throw new ArgumentException("There is no Id property");
        }

        /// <summary>
        /// The save object method, this is the actual method that responsible for saving or updating any object used by Dev Magic Fake
        /// </summary>
        /// <param name="memoryDb">
        /// The memory db, the place that will save in
        /// </param>
        /// <param name="obj">
        /// The Object that we want to save.
        /// </param>
        /// <param name="typeName">
        /// The type name of the object that we want to save.
        /// </param>
        /// <typeparam name="T">
        /// The type of the object that we want to save
        /// </typeparam>
        public static void SaveObject<T>(Dictionary<string, List<dynamic>> memoryDb, T obj, string typeName)
        {
            string objtypeName = typeof(T).FullName;
            PropertyInfo objectPropertyeInfo = typeof(T).GetProperty("Id");
            if (objectPropertyeInfo != null)
            {
                long objectId = (long)objectPropertyeInfo.GetValue(obj, null);

                if (memoryDb.ContainsKey(typeName))
                {
                    // Get id from the object by reflection
                    long oldId;
                    long newId;
                    List<dynamic> collection = memoryDb[typeName];
                    if (collection == null)
                    {
                        memoryDb[obj.ToString()] = new List<dynamic>();
                        newId = 1;
                    }
                    else
                    {
                        dynamic myObj = collection.OrderBy(x => x.Id).LastOrDefault();

                        oldId = myObj.Id;
                        newId = oldId + 1;
                    }

                    List<dynamic> table = memoryDb[typeName];
                    if (objectId != 0)
                    {
                        bool cantFind = true;
                        int objectIndex;
                        for (int i = 0; i < table.Count; i++)
                        {
                            if (GetIdFromObject<T>(table[i]) == objectId)
                            {
                                // find the object with the current id in the collection so I will update it
                                table[i] = obj;
                                cantFind = false;
                            }
                        }

                        if (cantFind)
                        {
                            // second and any save later of this type if the id not 0 and can't find the object with this id in the collection
                            objectPropertyeInfo.SetValue(obj, newId, null);
                            memoryDb[typeName].Add(obj);
                        }
                    }
                    else
                    {
                        // second and any save later of this type if the id=0
                        objectPropertyeInfo.SetValue(obj, newId, null);
                        memoryDb[typeName].Add(obj);
                    }
                }
                else
                {
                    // first save of this type
                    memoryDb.Add(typeName, new List<dynamic>());
                    objectPropertyeInfo.SetValue(obj, 1, null);
                    memoryDb[typeName].Add(obj);
                }
            }
        }
        
        #endregion 

        #region internal Methods
        /// <summary>
        /// The save method save an object to MemeoryDB
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        internal static void Save(dynamic instance, Assembly assembly)
        {
            PropertyInfo propertyInfo = instance.GetType().GetProperty("Id");

            if (propertyInfo == null)
            {
                return;
            }

            Type type = assembly.GetType(instance.ToString());
            var fakeRepositoryType = typeof(FakeRepository<>);
            Type[] typeArgs = { type };
            var genericFakeRepository = fakeRepositoryType.MakeGenericType(typeArgs);
            dynamic fakeRepository = Activator.CreateInstance(genericFakeRepository);
            fakeRepository.Add(instance);
        }
        /// <summary>
        /// The extract nested object from parent object.
        /// </summary>
        /// <param name="nestedTypeFullName">
        /// The nested type full name.
        /// </param>
        /// <param name="parentObject">
        /// The parent object.
        /// </param>
        /// <typeparam name="T">
        /// The type of the parent object
        /// </typeparam>
        /// <typeparam name="TX">
        /// The type of the nested object
        /// </typeparam>
        /// <returns>
        /// return nested object
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throw Exception in cased the nested type not exit in the parent object
        /// </exception>
        internal static TX ExtractNestedObjectFromParentObject<T, TX>(string nestedTypeFullName, T parentObject)
        {
            TX tembNestedObject = (TX)Activator.CreateInstance(typeof(TX));
            PropertyInfo nestedTypeInfo = typeof(T).GetProperty(String.Empty); // just initialize 
            var propertyInfos = typeof(T).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.IsInstanceOfType(tembNestedObject))
                {
                    nestedTypeInfo = propertyInfo;
                }
            }

            if (nestedTypeInfo == null)
            {
                throw new ArgumentException(String.Format("Your object didn't has nested Type of {0}, Please check your parent type for nested type", nestedTypeFullName));
            }

            return (TX)nestedTypeInfo.GetValue(parentObject, null);
        }

        /// <summary>
        /// Get all objects from collection (MemoryDB) by Type name
        /// </summary>
        /// <param name="memoryDb">
        /// The memory db.
        /// </param>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <param name="list">
        /// The list this will hold the return object.
        /// </param>
        /// <typeparam name="T">
        /// The type of the item in the collection
        /// </typeparam>
        /// <returns>
        /// The list that has all object in the collection (MemroyDB)
        /// </returns>
        internal static List<T> GetAllObjects<T>(Dictionary<string, List<dynamic>> memoryDb, string typeName, List<T> list)
        {
            if (memoryDb.ContainsKey(typeName))
            {
                if (memoryDb[typeName].Count > 0)
                {
                    list.AddRange(memoryDb[typeName].ToArray().Cast<T>().Select(obj => obj));
                }
            }

            return list;
        }

        /// <summary>
        /// Get id from object.
        /// </summary>
        /// <param name="obj">
        /// The object that we want to get it's Id.
        /// </param>
        /// <typeparam name="T">
        /// The type of the object
        /// </typeparam>
        /// <returns>
        /// The Id value from the object
        /// </returns>
        /// <exception cref="Exception">
        /// Object doesn't has Id property
        /// </exception>
        internal static long GetIdFromObject<T>(T obj)
        {
            PropertyInfo objectPropertyeInfo = typeof(T).GetProperty("Id");
            if (objectPropertyeInfo == null)
            {
                throw new ArgumentException("There is no Id property");
            }

            long objectId = (long)objectPropertyeInfo.GetValue(obj, null);
            return objectId;
        }

        /// <summary>
        /// Get object by code.
        /// </summary>
        /// <param name="memoryDb">
        /// The memory db.
        /// </param>
        /// <param name="code">
        /// The code that we want to search by.
        /// </param>
        /// <typeparam name="T">
        /// The type of the object that we search for
        /// </typeparam>
        /// <returns>
        /// The object that match the code parameter
        /// </returns>
        internal static T GetObjectByCode<T>(Dictionary<string, List<dynamic>> memoryDb, string code)
        {
            string typeName = typeof(T).FullName;
            if (typeName != null)
            {
                if (memoryDb.ContainsKey(typeName))
                {
                    dynamic obj = null;
                    try
                    {
                        obj = memoryDb[typeName].Where(x => x.Code == code).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException("Your type doesn't has a property called Code", e);
                    }
                    return (T)obj;
                }
            }

            return default(T);
        }

        /// <summary>
        /// The save collection, this Method like the save object it retrieve the nested object by Id and like it to the main object but this method retrieve the collection items and link collection , so this method
        /// assume that we only have the id and the name of  each item in the collection and we don't want to save new item in the MemroryDb we just need to retrieve this items and link to the main object
        /// </summary>
        /// <param name="nestedTypeFullName">
        /// The nested type full name.
        /// </param>
        /// <param name="obj">
        /// The object that has the collection.
        /// </param>
        /// <param name="memoryDb">
        /// The memory db.
        /// </param>
        /// <param name="typeFullName">
        /// The type full name.
        /// </param>
        /// <typeparam name="T">
        /// The type of the holder object 
        /// </typeparam>
        /// <typeparam name="TX">
        /// A collection of the other type List &lt;Order&gt; 
        /// </typeparam>
        /// <returns>
        /// The object that sent after retrieve it's item collection and create a new collection that hold them with all data not the Id and name only and link them to the main Object
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Throw Exception if the nested type doesn't exist in the main object
        /// </exception>
        internal static T SaveCollection<T, TX>(string nestedTypeFullName, T obj, Dictionary<string, List<dynamic>> memoryDb, string typeFullName)
        {
            dynamic collection;
            PropertyInfo collectionPropertyInfo;
            dynamic nestedObjectCollection;
            Type itemType = GetItemType<T, TX>(out collection, out collectionPropertyInfo, nestedTypeFullName, obj, out nestedObjectCollection);

            // I will not setValue in case of AddAll
            collectionPropertyInfo.SetValue(obj, collection, null);
            for (int i = 0; i < nestedObjectCollection.Count; i++)
            {
                // save it to the collection object created after retrieving the data from MemeryDB 
                // I just need to save in MemeoryDB in case of AddAll
                PropertyInfo idPropertyInfo = typeof(T).GetProperty(String.Empty); // just initialize 
                var itemPropertyInfos = nestedObjectCollection[i].GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in itemPropertyInfos)
                {
                    if (propertyInfo.Name == "Id")
                    {
                        idPropertyInfo = propertyInfo;
                    }
                }

                if (idPropertyInfo == null)
                {
                    throw new ArgumentException(String.Format("Your object didn't has nested Type of {0}, Please check your parent type for nested type", nestedTypeFullName));
                }

                long id = idPropertyInfo.GetValue(nestedObjectCollection[i], null);

                MethodInfo method = typeof(RepositoryUtilities).GetMethod("GetObjectById");
                MethodInfo generic = method.MakeGenericMethod(itemType);
                dynamic returnedObject = generic.Invoke(new RepositoryUtilities(), new object[] { memoryDb, id });
                if (returnedObject != null)
                {
                    collection.Add(returnedObject);
                }

            }

            SaveObject(memoryDb, obj, typeFullName);
            return obj;
        }

        /// <summary>
        /// The save collection and save nested, this method will save the main object and save all it's collection items, this method will not retrieve the item by Id, it save or update any item exist in the collection and link them to the main object
        /// </summary>
        /// <param name="nestedTypeFullName">
        /// The nested type full name.
        /// </param>
        /// <param name="obj">
        /// The object that has the collection.
        /// </param>
        /// <param name="memoryDb">
        /// The memory db.
        /// </param>
        /// <param name="typeFullName">
        /// The type full name.
        /// </param>
        /// <typeparam name="T">
        /// The type of the holder object
        /// </typeparam>
        /// <typeparam name="TX">
        /// A collection of the other type List &lt;Order&gt; 
        /// </typeparam>
        /// <returns>
        /// The object that sent after and create a new collection saved each item in the MemoryDB and link to the collection, each item will has an new Id in case of the Id is 0 or not exist
        /// </returns>
        internal static T SaveCollectionAndSaveNested<T, TX>(string nestedTypeFullName, T obj, Dictionary<string, List<dynamic>> memoryDb, string typeFullName)
        {
            dynamic collection;
            PropertyInfo collectionPropertyInfo;
            dynamic nestedObjectCollection;
            Type itemType = GetItemType<T, TX>(out collection, out collectionPropertyInfo, nestedTypeFullName, obj, out nestedObjectCollection);

            // I will not setValue in case of SaveWithNested
            for (int i = 0; i < nestedObjectCollection.Count; i++)
            {
                // I just need to save in MemeoryDB in case of SaveWithNested
                MethodInfo method = typeof(RepositoryUtilities).GetMethod("SaveObject");
                MethodInfo generic = method.MakeGenericMethod(itemType);
                generic.Invoke(new RepositoryUtilities(), new object[] { memoryDb, nestedObjectCollection[i], itemType.FullName });
            }

            SaveObject(memoryDb, obj, typeFullName);
            return obj;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get item type, this method get the type of an item of a generic collection
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="collectionPropertyInfo">
        /// The collection property info.
        /// </param>
        /// <param name="nestedTypeFullName">
        /// The nested type full name.
        /// </param>
        /// <param name="obj">
        /// The main object that has the collection so we can extract the collection from it.
        /// </param>
        /// <param name="nestedObjectCollection">
        /// The nested object collection, after we extract the collection from the object we will hold it's item in this collection
        /// </param>
        /// <typeparam name="T">
        /// The type of the main object that hold the collection
        /// </typeparam>
        /// <typeparam name="TX">
        /// The type of the collection itself
        /// </typeparam>
        /// <returns>
        /// The type of the item inside the collection
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private static Type GetItemType<T, TX>(out object collection, out PropertyInfo collectionPropertyInfo, string nestedTypeFullName, T obj, out object nestedObjectCollection)
        {
            Type itemType = null;
            if (typeof(TX).IsGenericType && (typeof(TX).GetGenericTypeDefinition() == typeof(IEnumerable<>) || (typeof(TX).GetGenericTypeDefinition() == typeof(List<>)) || (typeof(TX).GetGenericTypeDefinition() == typeof(IList<>))))
            {
                itemType = typeof(TX).GetGenericArguments()[0];
            }

            var collectionType = typeof(List<>);
            Type[] typeArgs = { itemType };
            var genericCollection = collectionType.MakeGenericType(typeArgs);
            collection = Activator.CreateInstance(genericCollection);

            collectionPropertyInfo = typeof(T).GetProperty(String.Empty);
            var declaringTypePropertyInfos = typeof(T).GetProperties();
            foreach (var propertyInfo in declaringTypePropertyInfos)
            {
                if (propertyInfo.PropertyType.IsInstanceOfType(collection))
                {
                    collectionPropertyInfo = propertyInfo;
                }
            }

            if (collectionPropertyInfo == null)
            {
                throw new ArgumentException(String.Format("Your object didn't has nested Type of {0}, Please check your parent type for nested type", nestedTypeFullName));
            }

            // review this part very good
            nestedObjectCollection = collectionPropertyInfo.GetValue(obj, null);
            return itemType;
        }

        #endregion
        
    }
}