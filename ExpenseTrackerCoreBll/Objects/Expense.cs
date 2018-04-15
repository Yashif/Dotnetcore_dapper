using System;

namespace ExpenseTrackerCoreBll
{
	public class Expense
	{

		public string Name { get; set; }
		public string Id { get; set; }
		public string Description { get; set; }
		public string Amount { get; set; }
        public double ExpenseAmount
        {
            get
            {
                double.TryParse(Amount, out double amount);
                return amount;
            }
            set => Amount = Convert.ToString(value);
        }
        public string UserId { get; set; }
		public bool Deleted { get; set; }
		public DateTime InsertDate { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}