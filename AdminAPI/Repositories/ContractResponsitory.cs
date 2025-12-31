using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;

namespace AdminAPI.Repositories
{
    public class ContractResponsitory : IContractResponsitory
    {
        private readonly DbContext _dbContext;

        public ContractResponsitory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ContractModel> GetContracts(int page, int perPage)
        {
            try
            {
                // Chuẩn hoá page/perPage để tránh âm/0
                if (page <= 0) page = 1;
                if (perPage <= 0) perPage = 10;
                if (perPage > 100) perPage = 100; // chặn perPage quá lớn

                int offset = (page - 1) * perPage;

                string sql = $@"
                    SELECT *
                    FROM contracts
                    WHERE is_deleted = 0 AND deleted_at IS NULL
                    ORDER BY created_at DESC
                    LIMIT {perPage} OFFSET {offset};
                ";

                return _dbContext.ExecuteQuery(sql).ConvertTo<ContractModel>() ?? [];
            }
            catch
            {
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                // nhớ COUNT cũng phải lọc is_deleted giống list
                string sql = @"
                    SELECT COUNT(id)
                    FROM contracts
                    WHERE is_deleted = 0 AND deleted_at IS NULL;
                ";

                return Convert.ToInt32(_dbContext.ExecuteScalar(sql));
            }
            catch
            {
                throw;
            }
        }
    }
}
