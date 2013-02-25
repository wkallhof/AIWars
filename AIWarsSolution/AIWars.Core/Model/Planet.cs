﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWars.Core.Model
{
    public class Planet
    {
        private int _size;

        public Planet(int size)
        {
            _size = size;
        }

        public int Size
        {
            get
            {
                return _size;
            }

			set
			{
				_size = value;
			}
        }

        public Point Point { get; set; }
    }
}
