using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XevosWebApp.Data;
using XevosWebApp.Model;

namespace XevosWebApp.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly XevosWebApp.Data.AppDbContext _context;

        public IndexModel(XevosWebApp.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            ViewData["FirstNameSort"] = sortOrder == "FirstName" ? "FirstNameDesc" : "FirstName";
            ViewData["LastNameSort"] = sortOrder == "LastName" ? "LastNameDesc" : "LastName";
            ViewData["DateSort"] = sortOrder == "Date" ? "DateDesc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var people = from person
                         in _context.People
                         select person;

            if (!String.IsNullOrEmpty(searchString))
            {
                people = people.Where(p => p.Jmeno != null && p.Jmeno.Contains(searchString) || p.Prijmeni != null && p.Prijmeni.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "FirstName":
                    people = people.OrderBy(p => p.Jmeno);
                    break;
                case "FirstNameDesc":
                    people = people.OrderByDescending(p => p.Jmeno);
                    break;
                case "LastName":
                    people = people.OrderBy(p => p.Prijmeni);
                    break;
                case "LastNameDesc":
                    people = people.OrderByDescending(p => p.Prijmeni);
                    break;
                case "Date":
                    people = people.OrderBy(p => p.Date);
                    break;
                case "DateDesc":
                    people = people.OrderByDescending(p => p.Date);
                    break;
                default:
                    people = people.OrderBy(p => p.Id);
                    break;
            }

            Person = await people.AsNoTracking().ToListAsync();

        }

        public async Task<IActionResult> OnGetDataAsync()
        {
            string json;
            using (HttpClient client = new())
            {
                json = await client.GetStringAsync(@"https://xevos.store/wp-content/jmena.json");
            }

            var people = JsonConvert.DeserializeObject<List<Person>>(json);
            if (people is not null)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"TRUNCATE TABLE[People]");
                await _context.AddRangeAsync(people);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
