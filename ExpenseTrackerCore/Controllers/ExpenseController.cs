using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerCoreBll;
using System.Threading.Tasks;
using System;
using ExpenseTrackerCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTrackerCore.Controllers
{
    [Route("[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IRepository<Expense> _expenseRepo;
        private readonly AutoMapper.IMapper _mapper;

        public ExpenseController(IRepository<Expense> expenseRepo, AutoMapper.IMapper mapper)
        {
            _expenseRepo = expenseRepo;
            _mapper = mapper;
        }

        // GET: api/Expense
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var expenses = await GetExpenses();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Expense/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try {
                var exp = await _expenseRepo.GetById(id);
                if (exp == null)
                {
                    return NotFound();
                }
                var expenseModel = _mapper.Map<ExpenseModel>(exp);
                return Ok(exp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            try
            {
                 var expenses = await _expenseRepo.GetByUserId(userId);
                if (expenses == null)
                {
                    return NotFound();
                }
                var expenseModels = _mapper.Map<IEnumerable<ExpenseModel>>(expenses);
                return Ok(expenseModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Expense
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ExpenseModel expenseModel)
        {
            var modelstate = ModelState.Distinct();
            if(!ModelState.IsValid)
            {
                return BadRequest(modelstate.FirstOrDefault().Value);
            }
            try
            {
                if (expenseModel == null)
                {
                    return BadRequest("invalid expense ");
                }    
                if(double.IsNaN(expenseModel.ExpenseAmount))
                {
                    return BadRequest("expense amount field should have number value ");
                }
                var expense = _mapper.Map<Expense>(expenseModel);

                await _expenseRepo.Insert(expense);

                var expenses = await GetExpenses();
                return Created("", expenses);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Expense/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]ExpenseModel expenseModel)
        {
            if (expenseModel == null ||id==Guid.Empty)
            {
                return BadRequest("invalid expense ");
            }
            try
            {
                var expense = _mapper.Map<Expense>(expenseModel);
                await _expenseRepo.Update(expense, id);
                var expenses = await GetExpenses();
                return Ok(expenses);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Expense/5
        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(Guid  id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest("please enter a valid id");
            }
            try
            {
				var expense =await _expenseRepo.Delete(id);
                var expenses = await GetExpenses();
                return Ok(expenses);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async  Task<IEnumerable<ExpenseModel>>GetExpenses()
        {
            var expenses = await _expenseRepo.GetAll();
            if (expenses == null)
            {
                throw new Exception( "no epxense record found.");
            }
            var expenseModels = _mapper.Map<IEnumerable<ExpenseModel>>(expenses);
            return expenseModels;
        }
    }
}
