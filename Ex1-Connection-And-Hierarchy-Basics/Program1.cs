#region Copyright
//  Copyright 2016  OSIsoft, LLC
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
#endregion
using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1_Connection_And_Hierarchy_Basics
{
    class Program1
    {
        static void Main(string[] args)
        {

            AFDatabase database = GetDatabase("PISRV01", "Green Power Company");

            PrintRootElements(database);
            PrintElementTemplates(database);
            PrintAttributeTemplates(database, "MeterAdvanced");
            PrintEnergyUOMs(database.PISystem);
            PrintEnumerationSets(database);
            PrintCategories(database);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string server, string database)
        {
            PISystems assetServers = new PISystems();
            PISystem assetServer = assetServers[server];
            AFDatabase afDatabase = assetServer.Databases[database];
            return afDatabase;
        }

        static void PrintRootElements(AFDatabase database)
        {
            Console.WriteLine("Print Root Elements: {0}", database.Elements.Count);
            foreach (AFElement element in database.Elements)
            {
                Console.WriteLine("  {0}", element.Name);
            }

            Console.WriteLine();
        }

        static void PrintElementTemplates(AFDatabase database)
        {
            Console.WriteLine("Print Element Templates");
            AFNamedCollectionList<AFElementTemplate> elemTemplates = database.ElementTemplates.FilterBy(typeof(AFElement));
            foreach (AFElementTemplate elemTemp in elemTemplates)
            {
                string[] categories = new string[elemTemp.Categories.Count];
                int i = 0;
                foreach (AFCategory category in elemTemp.Categories)
                {
                    categories[i++] = category.Name;
                }


                string categoriesString = string.Join(",", categories);
                Console.WriteLine("Name: {0}, Categories: {1}", elemTemp.Name, categoriesString);

                // Note: An alternative approach is to use CategoriesString directly: "CategoriesString read only property returns the list of categories in a string separated by semicolons."
                //Console.WriteLine("Name: {0}, Categories: {1}", elemTemp.Name, elemTemp.CategoriesString);
            }

            Console.WriteLine();
        }

        static void PrintAttributeTemplates(AFDatabase database, string elemTempName)
        {
            Console.WriteLine("Print Attribute Templates for Element Template: {0}", elemTempName);
            AFElementTemplate elemTemp = database.ElementTemplates[elemTempName];
            foreach (AFAttributeTemplate attrTemp in elemTemp.AttributeTemplates)
            {
                string drName = attrTemp.DataReferencePlugIn == null ? "None" : attrTemp.DataReferencePlugIn.Name;
                Console.WriteLine("Name: {0}, DRPlugin: {1}", attrTemp.Name, drName);
            }

            Console.WriteLine();
        }

        static void PrintEnergyUOMs(PISystem system)
        {
            Console.WriteLine("Print Energy UOMs");
            UOMClass uomClass = system.UOMDatabase.UOMClasses["Energy"];
            foreach (UOM uom in uomClass.UOMs)
            {
                Console.WriteLine("UOM: {0}, Abbreviation: {1}", uom.Name, uom.Abbreviation);
            }

            Console.WriteLine();
        }

        static void PrintEnumerationSets(AFDatabase database)
        {
            Console.WriteLine("Print Enumeration Sets\n");
            AFEnumerationSets enumSets = database.EnumerationSets;
            foreach (AFEnumerationSet enumSet in enumSets)
            {
                Console.WriteLine(enumSet.Name);
                foreach (AFEnumerationValue state in enumSet)
                {
                    int stateValue = state.Value;
                    string stateName = state.Name;
                    Console.WriteLine("{0} - {1}", stateValue, stateName);
                }

                Console.WriteLine();
            }
        }

        static void PrintCategories(AFDatabase database)
        {
            Console.WriteLine("Print Categories\n");
            AFCategories elemCategories = database.ElementCategories;
            AFCategories attrCategories = database.AttributeCategories;

            Console.WriteLine("Element Categories");
            foreach (AFCategory category in elemCategories)
            {
                Console.WriteLine(category.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Attribute Categories");
            foreach (AFCategory category in attrCategories)
            {
                Console.WriteLine(category.Name);
            }

            Console.WriteLine();
        }
    }
}
