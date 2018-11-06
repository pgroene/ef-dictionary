using System;
using System.Collections.Generic;
using System.Text;

namespace efdictionary.Entities
{
    public class Parent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public List<Child> Childs { get; private set; } = new List<Child>();
    }

}
