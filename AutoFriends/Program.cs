using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFriends
{
    class Program
    {
        static void execute()
        {
            // Some while loop to run every day or to constantly be running?
            FriendLoader friendLoader = new FriendLoader();

            friendLoader.createDatabase();            
            friendLoader.updateFriendsList();
            List<Friend> friendsList = friendLoader.sortedFriends();
            Report report = new Report(friendsList);
            string reportString = report.createReport();

            // Report the results
            Console.WriteLine(reportString);
        }

        static void Main(string[] args)
        {
            Program.execute();
            Console.WriteLine("Press Enter to stop");
            Console.ReadLine();
        }
    }
}
