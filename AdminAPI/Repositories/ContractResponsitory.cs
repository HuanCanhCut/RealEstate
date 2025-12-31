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
            return NewMethod(page, perPage);
        }

        private List<ContractModel> NewMethod(int page, int perPage)
        {
            try
            {
                int offset = (page - 1) * perPage;

                string sql = $@"
                    SELECT 
                        c.id,
                        c.amount,
                        c.commission,
                        c.status,
                        c.created_at,

                        JSON_OBJECT(
                            'full_name', cu.full_name
                        ) AS json_customer,

                        JSON_OBJECT(
                            'full_name', ag.full_name
                        ) AS json_agent,

                        JSON_OBJECT(
                            'title', p.title
                        ) AS json_post

                    FROM contracts c
                    JOIN users cu ON cu.id = c.customer_id
                    JOIN users ag ON ag.id = c.agent_id
                    JOIN posts p ON p.id = c.post_id

                    WHERE c.is_deleted = 0
                    AND c.deleted_at IS NULL

                    LIMIT {perPage}
                    OFFSET {offset};
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


        public ContractModel GetContractById(int id)
        {
            try
            {
                string sql = $@"
                    SELECT 
                        c.id,
                        c.amount,
                        c.commission,
                        c.status,
                        c.duration,
                        c.clause,
                        c.created_at,

                        JSON_OBJECT(
                            'id', cu.id,
                            'full_name', cu.full_name,
                            'phone_number', cu.phone_number
                        ) AS json_customer,

                        JSON_OBJECT(
                            'id', ag.id,
                            'full_name', ag.full_name,
                            'phone_number', ag.phone_number
                        ) AS json_agent,

                        JSON_OBJECT(
                            'id', p.id,
                            'title', p.title
                        ) AS json_post

                    FROM contracts c
                    JOIN users cu ON cu.id = c.customer_id
                    JOIN users ag ON ag.id = c.agent_id
                    JOIN posts p ON p.id = c.post_id

                    WHERE c.id = {id}
                    AND c.is_deleted = 0
                    AND c.deleted_at IS NULL
                    LIMIT 1;
                    ";


                return _dbContext
                    .ExecuteQuery(sql)
                    .ConvertTo<ContractModel>()
                    .FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

    }
}
