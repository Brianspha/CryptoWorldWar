using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SmartContracts.Models
{
    public class Collectable
    {
        public Color Color { get; set; }
        public string Level { get; set; }
        public string ID { get; set; }
        public string Name {get;set;}
        public string ContractAddress { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Tag { get; set; }
        public GameObject CollectableObject { get; set; }
    }
}
