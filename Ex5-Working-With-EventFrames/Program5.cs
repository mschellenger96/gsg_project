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

namespace Ex5_Working_With_EventFrames
{
    class Program5
    {
        static void Main(string[] args)
        {
            AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
            AFElementTemplate eventFrameTemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventFrameTemplate);
            CaptureValues(database, eventFrameTemplate);
            PrintReport(database, eventFrameTemplate);

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
            // Your code here
        }

        static public void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }

        static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }
    }
}
