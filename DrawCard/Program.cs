using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCard
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyCardEntities db = new MyCardEntities())
            {
                db.Database.ExecuteSqlCommand("truncate table Matches");

                var people = db.People.ToList();
                var matchesDB = db.Matches;

                var matches = people.Select(p => new { Match = new Matches() { A_ID = p.Id, B_ID = p.Id, A_OK = false, B_OK = false }, Gender = p.Gender }).OrderBy(m=>Guid.NewGuid()).OrderBy(m=>m.Gender).ToList();
                int count = matches.Count();
                int left = 0, right = count - 1;

                while (left < right)
                {
                    matches[left].Match.B_ID = matches[right].Match.A_ID;
                    matches[right].Match.B_ID = matches[left].Match.A_ID;
                    left++;
                    right--;
                }


                matchesDB.AddRange(matches.Select(m=>m.Match));
                db.SaveChanges();
            }

        }
    }
}
