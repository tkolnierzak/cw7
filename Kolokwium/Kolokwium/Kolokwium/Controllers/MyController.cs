using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kolokwium.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium.Controllers
{
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MyController(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        [HttpGet("api/orders")]
        public async Task<IActionResult> Get([FromQuery] string nazwisko)
        {
            var orders = _context
                .Zamowienia
                .Include(o => o.ZamowieniaWyrobyCukiernicze)
                .ThenInclude(zwc => zwc.WyrobCukierniczy)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(nazwisko))
            {
                orders = orders.Where(o => o.Klient.Nazwisko == nazwisko);
            }

            if (!orders.Any())
            {
                return NotFound();
            }
            var finalOrdersResult = await orders.Select(o => new
            {
                o.DataPrzyjecia,
                o.DataRealizacji,
                o.IdZamowienia,
                o.Uwagi,
                Wyroby = o.ZamowieniaWyrobyCukiernicze.Select(zwc => new
                {
                    Cena = zwc.Ilosc * zwc.WyrobCukierniczy.CenaZaSzt,
                    zwc.WyrobCukierniczy.Nazwa,
                    zwc.WyrobCukierniczy.Typ,
                    zwc.Uwagi
                })
            }).ToListAsync();
            return Ok(finalOrdersResult);
        }


        [HttpPost("api/clients/{id}/orders")]
        public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateOrderDto dto)
        {
            var split = dto.DataPrzyjecia.Split('-');
            int day = int.Parse(split[2]);
            int month = int.Parse(split[1]);
            int year = int.Parse(split[0]);
            var date = new DateTime(year, month, day);

            var isProduct =_context
                    .WyrobyCukiernicze
                    .Any(w => dto.Wyroby.Select(d => d.Wyrob).Contains(w.Nazwa));
            if (!isProduct)
            {
                return BadRequest("Nie ma wszystkich produktów");
            }

            var customer = await _context.Klienci.SingleOrDefaultAsync(c => c.IdKlient == id);
            if (customer == null)
            {
                return BadRequest("Nie znaleziono takiego klienta");
            }

            var order = new Zamowienie()
            {
                DataPrzyjecia = date,
                Klient = customer,
                Uwagi = dto.Uwagi,
                ZamowieniaWyrobyCukiernicze = dto.Wyroby.Select(w => new ZamowienieWyrobCukierniczy()
                {
                    Ilosc = w.Ilosc,
                    Uwagi = w.Uwagi,
                    WyrobCukierniczy = _context.WyrobyCukiernicze.Single(wc => wc.Nazwa == w.Wyrob)
                })
            };
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok("Gratulacje! Zamówienie złożone pomyślnie!");
        }
    }
}