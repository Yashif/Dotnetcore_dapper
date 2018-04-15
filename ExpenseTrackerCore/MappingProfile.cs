using ExpenseTrackerCoreBll;
using ExpenseTrackerCore.Models;
using AutoMapper;

namespace ExpenseTrackerCore
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Expense, ExpenseModel>();
            CreateMap<ExpenseModel, Expense>();
        }
    }
}
