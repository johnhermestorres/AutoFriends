﻿using Google.Apis.Auth.OAuth2;
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

namespace AutoFriends
{
    class CalendarController
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        static Events EventsList;

        public List<Friend> updateLastHangout(List<Friend> friends)
        {
            // have a list of Friends
            // iterate calendar events
            // look for friend's name in event
            // if that event was more recent, update the DB

            foreach (Friend friend in friends)
            {
                //foreach (Event event in )
                //{
                    
                //}
            }
            return friends;
        }

        public Events getCalendarEvents()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

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
            //EventsResource.ListRequest request = service.Events.List("primary");
            // TODO: retrieve this from the user
            String calendarID = "huqtgktdtsc2r4etdrnaq1gt9k@group.calendar.google.com";
            EventsResource.ListRequest request = service.Events.List(calendarID);
            //request.TimeMin = DateTime.Now;
            request.TimeMax = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            //request.MaxResults = 10;
            //request.OrderBy = EventsResource.ListRequest.;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

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
            EventsList = events;
            return events;
        }
    }
}