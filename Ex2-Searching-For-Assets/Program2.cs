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
using System.Collections.Generic;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2_Searching_For_Assets
{
    class Program2
    {
        static void Main(string[] args)
        {

            AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
            FindMetersByName(database, "Meter00*");
            FindMetersByTemplate(database, "MeterBasic");
            FindMetersBySubstation(database, "Edinburgh");
            FindMetersAboveUsage(database, 300);
            FindBuildingInfo(database, "MeterAdvanced");

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string servername, string databasename)
        {
            PISystems piafsystems = new PISystems();
            PISystem system = piafsystems[servername];
            if (system != null && system.Databases.Contains(databasename))
            {
                Console.WriteLine("Found '{0}' with '{1}' databases", system.Name, system.Databases.Count);
                return system.Databases[databasename];
            }
            else
                return null;
        }

        static void FindMetersByName(AFDatabase database, string elementNameFilter)
        {
            Console.WriteLine("Find Meters by Name: {0}", elementNameFilter);

            // Default search is as an element name string mask.
            string querystring = string.Format("{0}", elementNameFilter);
            AFElementSearch elementquery = new AFElementSearch(database, "ElementSearch", querystring);
            foreach (AFElement element in elementquery.FindElements())
            {
                Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
                    element.Name,
                    element.Template.Name,
                    element.CategoriesString);
            }

            Console.WriteLine();
        }

        static void FindMetersByTemplate(AFDatabase database, string templateName)
        {
            Console.WriteLine("Find Meters By Template: {0}", templateName);
            AFElementSearch elementQuery = new AFElementSearch(database, "TemplateSearch", string.Format("template:\"{0}\"", templateName));
            AFElementSearch templateFilter = new AFElementSearch(database, "DerivedTemplates", string.Format("templateName:\"MeterAdvanced\""));

            int count = 0;
            foreach (AFElement element in elementQuery.FindElements())
            {
                Console.WriteLine("Element: {0}, Template: {1}", element.Name, element.Template.Name);
                if (templateFilter.IsMatch(element))
                {
                    count++;
                }
            }
            Console.WriteLine("Found {0} derived templates", count);
            Console.WriteLine();

        }

        static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            // Your code here
        }

        static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            // Your code here
        }

        static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            Console.WriteLine("Find Building Info: {0}", templateName);

            AFElementTemplate elemTemp = database.ElementTemplates[templateName];
            AFCategory buildingInfoCat = database.AttributeCategories["Building Info"];

            AFSearchToken token = new AFSearchToken(AFSearchFilter.Template, AFSearchOperator.Equal, elemTemp.GetPath());
            ///AFSearchToken token2 = new AFSearchToken(AFSearchFilter.Value, AFSearchOperator.Equal, buildingInfoCat.GetPath());

            AFElementSearch search = new AFElementSearch(database, "search", new[] { token });

            IEnumerable<AFElement> foundElements = search.FindElements();
            AFNamedCollectionList<AFAttribute> foundAttributes = new AFNamedCollectionList<AFAttribute>();
            
            foreach (AFElement foundElem in foundElements)
            {
                foreach (AFAttribute attr in foundElem.Attributes)
                {
                    if (attr.Categories.Contains(buildingInfoCat))
                    {
                        foundAttributes.Add(attr);
                    }
                }
            }

            Console.WriteLine("Found {0} attributes.", foundAttributes.Count);
            Console.WriteLine();
        }
    }
}
