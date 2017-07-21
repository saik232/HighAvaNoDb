using System;
using System.Diagnostics;
using System.Collections.Generic;
using HighAvaNoDb.Domain;

namespace HighAvaNoDb.Route
{

    
	public class HashBasedRouter : BaseRouter
	{
		public const string NAME = "HashBasedRouter";
        public HashBasedRouter()
        {
            HhashAlgorithm = new KetamaHashAlgorithm();
        }

        public override IHashAlgorithm HhashAlgorithm { set; get; }
    }
}