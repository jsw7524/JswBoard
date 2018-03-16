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

                var matches = people.Select(p => new { Match = new Matches() { A_ID = p.Id, B_ID = p.Id, A_OK = false, B_OK = false }, Gender = p.Gender }).ToList();

                int count = matches.Count();
                Random rnd = new Random(Guid.NewGuid().GetHashCode());

                foreach (var m in matches)
                {
                    int index = rnd.Next(1, count);
                    //for (int t = 0; t < 2; t++)
                    //{
                    //    if (m.Gender != matches[index].Gender)
                    //    {
                    //        index = rnd.Next(1, count);
                    //        break;
                    //    }
                    //}
                    int tmp = m.Match.B_ID;
                    m.Match.B_ID = matches[index].Match.B_ID;
                    matches[index].Match.B_ID = tmp;
                }


                matchesDB.AddRange(matches.Select(m=>m.Match));
                db.SaveChanges();
            }

        }
    }
}
