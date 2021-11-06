using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace CalendarQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        //static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string[] Scopes = { CalendarService.Scope.Calendar };    //SCOPES = ['https://www.googleapis.com/auth/calendar']
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.ReadWrite))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            string[] arg = Environment.GetCommandLineArgs();

            //Console.WriteLine(arg[1]);
            //Console.WriteLine(arg[2]);
            //Console.WriteLine(arg[3]);
            ////09:00-10:00//Int32.Parse(arg[4].Substring(0, 2))
            //Console.WriteLine(arg[4]);
            //Console.WriteLine(arg[5]);

            string command = arg[1];

            if(command == "insert")
            {
                var ev = new Event();
                EventDateTime start = new EventDateTime();
                start.DateTime = new DateTime(Int32.Parse(arg[2]), Int32.Parse(arg[3]), Int32.Parse(arg[4]), Int32.Parse(arg[5].Substring(0, 2)), Int32.Parse(arg[5].Substring(3, 2)), 0);

                EventDateTime end = new EventDateTime();
                end.DateTime = new DateTime(Int32.Parse(arg[2]), Int32.Parse(arg[3]), Int32.Parse(arg[4]), Int32.Parse(arg[5].Substring(6, 2)), Int32.Parse(arg[5].Substring(9, 2)), 0);

                ev.Start = start;
                ev.End = end;
                ev.Summary = arg[6];//title
                ev.Description = arg[7];//detail
                
                var calendarId = "primary";
                Event recurringEvent = service.Events.Insert(ev, calendarId).Execute();
                Console.WriteLine("Event created: %s\n", ev.HtmlLink);

            }
            else if(command == "delete")
            {
                var calendarId = "primary";
                string eventId = "";
                // List events.
                Events e = request.Execute();
                if (e.Items != null && e.Items.Count > 0)
                {
                    foreach (var eventItem in e.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (eventItem.Summary == arg[2])
                        {
                            Console.WriteLine("Event deleted:" + eventItem.Summary);
                            eventId = eventItem.Id;
                            break;
                        }
                    }
                }
                if(eventId != "")
                    service.Events.Delete(calendarId, eventId).Execute();
            }
            

            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
            Console.Read();
            Environment.Exit(0);
        }
    }
}