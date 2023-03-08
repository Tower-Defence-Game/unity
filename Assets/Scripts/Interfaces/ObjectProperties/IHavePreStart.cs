using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces.ObjectProperties
{
    public interface IHavePreStart
    {
        public bool IsLevelStarted { get; set; }
    }
}