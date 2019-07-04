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
        public int ID { get; set; }
        public string ContractAddress { get; set; }
        public string Description { get; set; }
        public float Value { get; set; }
        public string Tag { get; set; }
        public GameObject CollectableObject { get; set; }
    }
}
