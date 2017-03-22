using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFriends
{
    class Friend
    {
        // Friends should have a:
        //first and last name
        //preferred frequency of hangout
        //and last time (in days) since last hangout
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // How often do you intend to see this friend?
        // Once every week: daysFrequency = 7
        // Once every two weeks: daysFrequency = 14
        // Once a month: daysFrequency = 30

        public int dayFrequency;
        public int lastHangout;

        public decimal urgency()
        {
            decimal frequency = dayFrequency/100m;
            decimal last = lastHangout/100m;
            decimal urgency = last/frequency;
            return urgency;
        }

        public string fullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
