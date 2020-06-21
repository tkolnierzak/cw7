using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium.Models
{
    public class Zamowienie
    {
        [Key]
        public int IdZamowienia { get; set; }
        
        public DateTime DataPrzyjecia { get; set; }
        
        public DateTime DataRealizacji { get; set; }
        
        public string Uwagi { get; set; }
        
        public virtual Klient Klient { get; set; }
        
        public virtual Pracownik Pracownik { get; set; }
        
        public virtual IEnumerable<ZamowienieWyrobCukierniczy> ZamowieniaWyrobyCukiernicze { get; set; }
    }
}