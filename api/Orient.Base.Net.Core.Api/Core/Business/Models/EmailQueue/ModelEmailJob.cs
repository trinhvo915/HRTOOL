﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.EmailQueue
{
	public class ModelEmailJob
	{
		public ModelEmailJob()
		{

		}

		public string ToName { get; set; }

		public string JobName { get; set; }

		public string FromName { get; set; }
	}
}