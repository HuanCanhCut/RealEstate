using System.Data;

namespace UserAPI.Helpers
{
    public static class MapWithChild
    {
        public static List<TParent> MapWithChildren<TParent, TChild>(
            this DataTable table,
            string parentKeyColumn,
            Func<DataRow, TParent> parentFactory,
            Func<DataRow, TChild?> childFactory,
            Action<TParent, List<TChild>> childListSetter)
        {
            IEnumerable<IGrouping<object, DataRow>> grouped = table.AsEnumerable().GroupBy(row => row[parentKeyColumn]);

            List<TParent> result = [];

            foreach (IGrouping<object, DataRow> g in grouped)
            {
                TParent parent = parentFactory(g.First());
                List<TChild> children = g
                    .Select(childFactory)
                    .Where(c => c != null)
                    .ToList()!;
                childListSetter(parent, children);
                result.Add(parent);
            }
            return result;
        }

        public static List<TParent> MapWithSingleChild<TParent, TChild>(
            this DataTable table,
            string parentKeyColumn,
            Func<DataRow, TParent> parentFactory,
            Func<DataRow, TChild?> childFactory,
            Action<TParent, TChild?> childSetter
        )
        {
            if (table == null) return new List<TParent>();

            IEnumerable<IGrouping<object, DataRow>> grouped = table.AsEnumerable().GroupBy(row => row[parentKeyColumn]);

            List<TParent> result = [];

            foreach (IGrouping<object, DataRow> g in grouped)
            {
                TParent parent = parentFactory(g.First());

                // Lấy child đầu tiên hợp lệ (non-null) trong group, nếu none -> null
                TChild? child = g.Select(childFactory).FirstOrDefault(c => c != null);

                childSetter(parent, child);
                result.Add(parent);
            }

            return result;
        }
    }
}
