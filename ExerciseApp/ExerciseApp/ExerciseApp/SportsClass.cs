using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseApp
{
    [Table("SportsClass")]
    class SportsClass
    {
        [PrimaryKey, AutoIncrement]
        public int Index { get; set; }
        public string SportsName { get; set; }
        public int SportsCaloriePerHour { get; set; }

    }
}
