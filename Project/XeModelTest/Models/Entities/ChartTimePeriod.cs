using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Entities
{
    public class ChartTimePeriod
    {
        private DateTime _begin;
        private DateTime _end;

        public ChartTimePeriod()
        {
            _end = DateTime.Now;
            _begin = DateTime.Today;
            
        }

        public ChartTimePeriod(DateTime begin, DateTime end) : this()
        {
            Begin = begin;
            End = end;
        }

        /// <summary>
        /// Begin time.
        /// </summary>
        public DateTime Begin
        {
            get { return _begin; }
            set
            {
                if (value < _end)
                {
                    _begin = value;
                }
                else
                {
                    throw new ArgumentException("Begin time should be earlier than End time.", "Begin");
                }
            }
        }

        /// <summary>
        /// End time.
        /// </summary>
        public DateTime End
        {
            get { return _end; }
            set
            {
                if (value > _begin)
                {
                    _end = value;
                }
                else
                {
                    throw new ArgumentException("End time should be later than Begin time.", "End");
                }
            }
        }
    }
}
