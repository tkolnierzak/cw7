using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium.Models
{
    public class WyrobCukierniczy
    {
        [Key]
        public int IdWyrobuCukierniczego { get; set; }
        
        public string Nazwa { get; set; }
        
        public float CenaZaSzt { get; set; }
        
        public string Typ { get; set; }
        
        public virtual IEnumerable<ZamowienieWyrobCukierniczy> ZamowieniaWyrobCukiernicze { get; set; } 
    }
}