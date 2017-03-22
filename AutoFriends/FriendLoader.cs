using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace AutoFriends
{
    class FriendLoader
    {
        //private SQLiteConnection friendConnection;
        private string connectionString = "Data Source=FriendsDatabase.sqlite;";
        private string databaseFile = "FriendsDatabase.sqlite";
        public List<Friend> friends; 

        public int createDatabase()
        {
            int queryResult = -1;

            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql =
                        "CREATE TABLE friends (" +
                        "lastName varchar(20) , " +
                        "firstName varchar(20), " +
                        "dayFrequency int, " +
                        "lastHangout int, " +
                        "urgency decimal(5,2)," +
                        "unique (lastName, firstName)" +
                        ");";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    try
                    {
                        queryResult = command.ExecuteNonQuery();
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine("FriendLoader.createDatabase error: " + e);
                    }
                    conn.Close();                
                }
                
            }
            return queryResult;
        }

        public List<Friend> sortedFriends()
        {
            // return list of friends sorted by urgency
            int queryResult = -1;
            string results = "";
            List<Friend> friends = new List<Friend>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM friends ORDER BY urgency DESC;";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                try
                {
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        Friend friend = new Friend
                        {
                            LastName = reader["lastName"].ToString(),
                            FirstName = reader["firstName"].ToString(),
                            dayFrequency = (int) reader["dayFrequency"],
                            lastHangout = (int) reader["lastHangout"],

                        };
                        friends.Add(friend);
                        results += reader["lastName"];
                    }

                }
                catch (SQLiteException e)
                {
                    Console.WriteLine("FriendLoader.sortedFriends() error: " + e);
                }
                conn.Close();
            }
            this.friends = friends;
            return friends;
        } 

        // Add friend
        public int addFriend(Friend friend)
        {
            int queryResult = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql ="INSERT INTO friends (lastName, firstName, dayFrequency, lastHangout, urgency)" +
                            String.Format("VALUES ('{0}', '{1}', {2}, {3}, {4});", friend.LastName, friend.FirstName, friend.dayFrequency, friend.lastHangout, friend.urgency());
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                try
                {
                    queryResult = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    // TODO: duplicate field probably?
                    Console.WriteLine("FriendLoader.addFriend() error: " + e);
                }
                conn.Close();
            }
            return queryResult;
        }

        // Update friend
        public int updateFriend(Friend friend)
        {
            int queryResult = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE friends " +
                             String.Format("SET dayFrequency = {0},", friend.dayFrequency) +
                             String.Format("SET lastHangout = {0},", friend.lastHangout) +
                             String.Format("SET urgency = {0}", friend.urgency()) +
                             String.Format("WHERE lastName = '{0}' AND firstName = '{1}';", friend.LastName,
                                 friend.FirstName);
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                try
                {
                    queryResult = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    Console.WriteLine("FriendLoader.updateFriend() error: " + e);
                }
                conn.Close();
            }
            return queryResult;
        }

        
        // Remove friend

        // Update Friends list
        public int updateFriendsList()
        {
            // query Google Calendar for updates to Friends List
            CalendarController calendar = new CalendarController();
            // TODO: this should be in the FriendLoader class init
            calendar.setup();
            friends = calendar.updateLastHangout(friends);
            return 0;
        }

        // Clear DB
        public int clearDatabase()
        {
            int queryResult = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM friends;";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                queryResult = command.ExecuteNonQuery();
                conn.Close();
            }

            return queryResult;
        }
    }
}
