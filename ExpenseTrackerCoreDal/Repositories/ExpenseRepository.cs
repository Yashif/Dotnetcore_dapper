using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using ExpenseTrackerCoreBll;
namespace ExpenseTrackerCoreDal
{
    public class ExpenseRepository : IRepository<Expense>
    {
        private readonly IDbConnection _conn;
        public ExpenseRepository(IDbConnection conn) => _conn = conn ?? throw new ArgumentNullException(nameof(conn));

        public async Task<int> Delete(Guid id)
        {
            try
            {
                var deleteSql = @"UPDATE  Expense SET Deleted = 1  WHERE Id = @id";

                var deleted = await _conn.ExecuteAsync(deleteSql, new { id = id.ToString() });
                return deleted;
            }
            catch(Exception ex)
            {
                throw new Exception("Delete query failed: ",ex);
            }
        }

        public async Task<IEnumerable<Expense>> GetAll()
        {
            var selectSql = @"SELECT * FROM Expense WHERE Deleted =0";
            var expenses = await _conn.QueryAsync<Expense>(selectSql);
            return expenses;
        }

        public async Task<Expense> GetById(Guid id)
        {
            var selectSql = @"SELECT * FROM Expense WHERE Deleted =0 AND Id =@id";
            var expenses = await _conn.QueryAsync<Expense>(selectSql, new {id = id.ToString() });
            return expenses.SingleOrDefault();
        }

        public async Task<Expense> GetByName(string name)
        {
            var selectSql = @"SELECT * FROM Expense WHERE Deleted =0 AND Name =@name";
            var expenses = await _conn.QueryAsync<Expense>(selectSql, new { name });
            return expenses.FirstOrDefault();
        }

        public async Task<IEnumerable<Expense>> GetByUserId(Guid userId)
        {
            var selectSql = @"SELECT * FROM Expense WHERE Deleted = 0 AND UserId = @userId";
            var expenses = await _conn.QueryAsync<Expense>(selectSql, new { userId });
            return expenses;
        }

        public async Task Insert(Expense expense)
        {
            expense.Id = Guid.NewGuid().ToString();
            expense.InsertDate = DateTime.UtcNow;
            expense.ExpenseDate = DateTime.UtcNow;
            var insertSql = @"INSERT INTO Expense
(Id,Name,Description,Amount,Deleted,InsertDate,ExpenseDate,UserId)
VALUES(@Id,@Name,@Description,@Amount,0,@InsertDate,@ExpenseDate,@UserId)";
            await _conn.ExecuteAsync(insertSql, expense);
        }

        public async Task Update(Expense expense, Guid id)
        {
            expense.UpdateDate = DateTime.UtcNow;
            var updateSql = @"UPDATE Expense SET
Name= @Name,
Description = @Description,
Amount = @Amount,
UpdateDate =@UpdateDate
WHERE Id = @Id";
            await _conn.ExecuteAsync(updateSql,  expense );
        }
    }
}