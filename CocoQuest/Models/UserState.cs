using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocoQuest.Models
{
    public class UserState
    {
        public int XP { get; set; } = 0;
        public int Cocoonuts { get; set; } = 0;

        public double Water { get; set; } = 0;
        public double WaterMax { get; set; } = 2.0;

        public int CurrentLevel { get; set; } = 1;

        public bool ProteinDone { get; set; }
        public bool SleepDone { get; set; }
        public bool WalkDone { get; set; }
        public bool WorkoutDone { get; set; }

        public int TotalCoconutsEarned { get; set; }

    }
}
