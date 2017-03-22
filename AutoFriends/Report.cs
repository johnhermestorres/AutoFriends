using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFriends
{
    class Report
    {

        public Report(List<Friend> friends)
        {
            friendsList = friends;
        }

        public static List<Friend> friendsList; 
        public static string report;


        public string createReport()
        {
            // Make a nice looking report based on the ordered friends list
            report = "AutoFriends Report\n";
            foreach (Friend friend in friendsList)
            {
                decimal percentUrgency = Math.Round(friend.urgency() * 100, 2);
                report += String.Format("{0} {1} {2}%\n", friend.fullName(), friend.lastHangout, percentUrgency);

            }

            return report;
        }

        public static void sendEmail()
        {
            // Send email
        }

        public static void sendText()
        {
            // Send text message
        }
    }
}
