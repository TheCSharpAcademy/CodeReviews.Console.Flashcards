﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Model
{
	internal class StudySession
	{
        public int StudySessionId { get; set; }
		public int StackId { get; set; }
		public DateTime StudyDate { get; set; }
        public int Score { get; set; }
    }
}
