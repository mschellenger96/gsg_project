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
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;

namespace Ex5_Working_With_EventFrames_Sln
{
    class Program5
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
            AFElementTemplate eventframetemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventframetemplate);
            CaptureValues(database, eventframetemplate);
            PrintReport(database, eventframetemplate);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static AFDatabase GetDatabase(string servername, string databasename)
        {
            PISystem system = GetPISystem(null, servername);
            if (!string.IsNullOrEmpty(databasename))
                return system.Databases[databasename];
            else
                return system.Databases.DefaultDatabase;
        }

        static PISystem GetPISystem(PISystems systems = null, string systemname = null)
        {
            systems = systems == null ? new PISystems() : systems;
            if (!string.IsNullOrEmpty(systemname))
                return systems[systemname];
            else
                return systems.DefaultPISystem;
        }

        static AFElementTemplate CreateEventFrameTemplate(AFDatabase database)
        {
            AFElementTemplate eventframetemplate = null;
            if (database.ElementTemplates.Contains("Daily Usage"))
            {
                return database.ElementTemplates["Daily Usage"];
            }

            eventframetemplate = database.ElementTemplates.Add("Daily Usage");
            eventframetemplate.InstanceType = typeof(AFEventFrame);
            eventframetemplate.NamingPattern = @"%TEMPLATE%-%ELEMENT%-%STARTTIME:yyyy-MM-dd%-EF*";

            AFAttributeTemplate usage = eventframetemplate.AttributeTemplates.Add("Average Energy Usage");
            usage.Type = typeof(double);
            usage.DataReferencePlugIn = AFDataReference.GetPIPointDataReference();
            usage.ConfigString = @".\Elements[.]|Energy Usage;TimeRangeMethod=Average";
            usage.DefaultUOM = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];

            database.CheckIn();
            return eventframetemplate;
        }

        static void CreateEventFrames(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            const int pageSize = 1000;
            int startIndex = 0;
            int totalCount;
            do
            {
                // This method returns the collection of AFBaseElement objects that were created with this template.
                AFNamedCollectionList<AFBaseElement> results = database.ElementTemplates["MeterBasic"].FindInstantiatedElements(
                    includeDerived: true,
                    sortField: AFSortField.Name,
                    sortOrder: AFSortOrder.Ascending,
                    startIndex: startIndex,
                    maxCount: pageSize,
                    totalCount: out totalCount
                    );

                IList<AFElement> meters = results.Select(elm => (AFElement)elm).ToList();

                DateTime timereference = DateTime.Now.AddDays(-7);
                //AFTime startTime = new AFTime(new DateTime(timereference.Year, timereference.Month, timereference.Day, 0, 0, 0, DateTimeKind.Local));
                foreach (AFElement meter in meters)
                {
                    foreach (int day in Enumerable.Range(1, 7))
                    {
                        AFTime startTime = new AFTime(timereference.AddDays(day - 1));
                        AFTime endTime = new AFTime(startTime.LocalTime.AddDays(1));
                        AFEventFrame ef = new AFEventFrame(database, "*", eventFrameTemplate);
                        ef.SetStartTime(startTime);
                        ef.SetEndTime(endTime);
                        ef.PrimaryReferencedElement = meter;
                    }
                }

                database.CheckIn();

                startIndex += pageSize;
            } while (startIndex < totalCount);

            database.CheckIn();
        }

        static public void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Formulate search constraints on time and template
            DateTime timereference = DateTime.Now.AddDays(-7);
            AFTime startTime = new AFTime(new DateTime(timereference.Year, timereference.Month, timereference.Day, 0, 0, 0, DateTimeKind.Local));
            string query = string.Format("template:\"{0}\"", eventFrameTemplate.Name);
            AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFEventFrameSearchMode.ForwardFromStartTime, startTime, query);

            int startIndex = 0;
            foreach (AFEventFrame item in eventFrameSearch.FindEventFrames())
            {
                item.CaptureValues();
                if ((startIndex++ % 512) == 0)
                    database.CheckIn();
            }

            database.CheckIn();
        }

        static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            DateTime timereference = DateTime.Now.AddDays(-7);
            AFTime startTime = new AFTime(new DateTime(timereference.Year, timereference.Month, timereference.Day, 0, 0, 0, DateTimeKind.Local));
            AFTime endTime = startTime.LocalTime.AddDays(+8);
            string query = string.Format("template:\"{0}\" ElementName:\"{1}\"", eventFrameTemplate.Name, "Meter003");
            AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFSearchMode.StartInclusive, startTime, endTime, query);

            foreach (AFEventFrame ef in eventFrameSearch.FindEventFrames())
            {
                Console.WriteLine("{0}, {1}, {2}",
                    ef.Name,
                    ef.PrimaryReferencedElement.Name,
                    ef.Attributes["Average Energy Usage"].GetValue().Value);
            }
        }
    }
}
