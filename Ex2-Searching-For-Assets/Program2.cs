#region Copyright
//  Copyright 2016, 2017  OSIsoft, LLC
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
            FindMetersBySubstation(database, "SSA*");
            FindMetersAboveUsage(database, 300);
            FindBuildingInfo(database, "MeterAdvanced");

            Console.WriteLine("Press ENTER key to close");
            Console.ReadLine();
        }

        static AFDatabase GetDatabase(string serverName, string databaseName)
        {
            PISystem assetServer = GetPISystem(null, serverName);
            if (!string.IsNullOrEmpty(databaseName))
                return assetServer.Databases[databaseName];
            else
                return assetServer.Databases.DefaultDatabase;
        }

        static PISystem GetPISystem(PISystems systems = null, string systemName = null)
        {
            systems = systems == null ? new PISystems() : systems;
            if (!string.IsNullOrEmpty(systemName))
                return systems[systemName];
            else
                return systems.DefaultPISystem;
        }

        static void FindMetersByName(AFDatabase database, string elementNameFilter)
        {
            Console.WriteLine("Find Meters by Name: {0}", elementNameFilter);

            // Default search is as an element name string mask.
            var queryString = $"\"{elementNameFilter}\"";
            using (AFElementSearch elementQuery = new AFElementSearch(database, "ElementSearch", queryString))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                foreach (AFElement element in elementQuery.FindElements())
                {
                    Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
                        element.Name,
                        element.Template.Name,
                        element.CategoriesString);
                }
            }
            Console.WriteLine();
        }

        static void FindMetersByTemplate(AFDatabase database, string templateName)
        {
            /// Your code here

        }

        static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            /// Your code here
        }

        static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            /// Your code here
        }

        static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            /// Your code here
        }
    }
}
