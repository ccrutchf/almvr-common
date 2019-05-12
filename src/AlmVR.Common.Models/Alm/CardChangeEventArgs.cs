using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Common.Models.Alm
{
    public class CardChangedEventArgs : EventArgs
    {
        public CardModel Card { get; private set; }

        public CardChangedEventArgs(CardModel card)
        {
            Card = card;
        }
    }
}
