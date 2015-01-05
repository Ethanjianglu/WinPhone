using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseApp
{
    [Table("FoodClass")]
    public class FoodClass
    {
        [PrimaryKey, AutoIncrement]
        public int Index { get; set; }
        public string FoodName { get; set; }
        public int FoodCaloriePerKG { get; set; }
    }
}
