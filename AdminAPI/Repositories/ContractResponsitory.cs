using AdminAPI.DTO.Request;
using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;
using System.Text;

namespace AdminAPI.Repositories
{
    public class ContractResponsitory : IContractResponsitory
    {
        private readonly DbContext _dbContext;

        public ContractResponsitory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ================= API CŨ =================

        public List<ContractModel> GetContracts(int page, int perPage)
        {
            int offset = (page - 1) * perPage;

            string sql = $@"
                SELECT 
                    c.id,
                    c.amount,
                    c.commission,
                    c.status,
                    c.created_at,

                    JSON_OBJECT('full_name', cu.full_name) AS json_customer,
                    JSON_OBJECT('full_name', ag.full_name) AS json_agent,
                    JSON_OBJECT('title', p.title) AS json_post

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

        public int CountAll()
        {
            string sql = @"
                SELECT COUNT(id)
                FROM contracts
                WHERE is_deleted = 0
                  AND deleted_at IS NULL;
            ";

            return Convert.ToInt32(_dbContext.ExecuteScalar(sql));
        }

        public ContractModel GetContractById(int id)
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

            return _dbContext.ExecuteQuery(sql).ConvertTo<ContractModel>().FirstOrDefault();
        }

        public bool UpdateStatus(int contractId, string status)
        {
            string sql = $@"
                UPDATE contracts
                SET status = '{status}',
                    updated_at = NOW()
                WHERE id = {contractId}
                  AND is_deleted = 0
                  AND deleted_at IS NULL;
            ";

            return _dbContext.ExecuteNonQuery(sql) > 0;
        }

        public bool DeleteContract(int id)
        {
            string sql = $@"
                UPDATE contracts
                SET is_deleted = 1,
                    deleted_at = NOW()
                WHERE id = {id}
                  AND is_deleted = 0
                  AND deleted_at IS NULL;
            ";

            return _dbContext.ExecuteNonQuery(sql) > 0;
        }

        // ================= FILTER API MỚI =================

        private string BuildFilterWhere(ContractFilterRequest filter)
        {
            var where = new StringBuilder();

            where.Append(" AND c.is_deleted = 0 AND c.deleted_at IS NULL ");

            if (!string.IsNullOrEmpty(filter.status))
                where.Append($" AND c.status = '{filter.status}' ");

            if (filter.from_date.HasValue)
                where.Append($" AND c.created_at >= '{filter.from_date:yyyy-MM-dd}' ");

            if (filter.to_date.HasValue)
                where.Append($" AND c.created_at <= '{filter.to_date:yyyy-MM-dd} 23:59:59' ");

            if (!string.IsNullOrEmpty(filter.keyword))
                where.Append($@"
                    AND (
                        cu.full_name LIKE '%{filter.keyword}%'
                        OR ag.full_name LIKE '%{filter.keyword}%'
                        OR p.title LIKE '%{filter.keyword}%'
                    )
                ");

            return where.ToString();
        }

        public List<ContractModel> FilterContracts(ContractFilterRequest filter)
        {
            int offset = (filter.page - 1) * filter.per_page;

            string sql = $@"
                SELECT 
                    c.id,
                    c.amount,
                    c.commission,
                    c.status,
                    c.created_at,

                    JSON_OBJECT('full_name', cu.full_name) AS json_customer,
                    JSON_OBJECT('full_name', ag.full_name) AS json_agent,
                    JSON_OBJECT('title', p.title) AS json_post

                FROM contracts c
                JOIN users cu ON cu.id = c.customer_id
                JOIN users ag ON ag.id = c.agent_id
                JOIN posts p ON p.id = c.post_id

                WHERE 1=1
                {BuildFilterWhere(filter)}

                LIMIT {filter.per_page}
                OFFSET {offset};
            ";

            return _dbContext.ExecuteQuery(sql).ConvertTo<ContractModel>() ?? [];
        }

        public int CountFiltered(ContractFilterRequest filter)
        {
            string sql = $@"
                SELECT COUNT(c.id)
                FROM contracts c
                JOIN users cu ON cu.id = c.customer_id
                JOIN users ag ON ag.id = c.agent_id
                JOIN posts p ON p.id = c.post_id
                WHERE 1=1
                {BuildFilterWhere(filter)};
            ";

            return Convert.ToInt32(_dbContext.ExecuteScalar(sql));
        }
    }
}
