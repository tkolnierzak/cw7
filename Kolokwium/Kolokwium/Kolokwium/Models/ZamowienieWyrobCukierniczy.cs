using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium.Models
{
    public class ZamowienieWyrobCukierniczy
    {
        public int Ilosc { get; set; }
        
        public string Uwagi { get; set; }

        [ForeignKey("WyrobCukierniczy")]
        public int IdWyrobCukierniczy { get; set; }

        [ForeignKey("Zamowienie")]
        public int IdZamowienie { get; set; }

        public WyrobCukierniczy WyrobCukierniczy { get; set; }
        
        public Zamowienie Zamowienie { get; set; }
    }
}