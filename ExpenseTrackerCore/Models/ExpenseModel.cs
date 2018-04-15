using ExpenseTrackerCore.Models;
using System;
using System.Collections.Generic;

namespace ExpenseTrackerCore.Models
{
    public class ExpenseModel
	{
			public string Name { get; set; }
			public string Id { get; set; }
			public string Description { get; set; }
			public double ExpenseAmount { get; set; }
			public DateTime ExpenseDate { get; set; }
			public string UserId { get; set; }
		}
}


