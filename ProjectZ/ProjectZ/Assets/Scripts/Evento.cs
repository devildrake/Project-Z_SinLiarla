using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Assets.Scripts
{
    [Serializable]
    public class Evento
    {
        public bool isHappening;
        public bool hasHappened;
        public int numInteracts;
        public int currInteract;
        public enum tipoEvento {NORMAL,ESPECIAL};
        public string[] messages;
        public tipoEvento tipo;
    }
}
