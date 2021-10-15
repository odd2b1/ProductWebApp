using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Models
{
    public class TestModel
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public int Id { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Name { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime DateStamp { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Price { get; set; }
    }
}
