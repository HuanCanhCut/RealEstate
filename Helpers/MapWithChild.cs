using System.Data;

namespace RealEstate.Helpers
{
    public static class MapWithChild
    {
        public static class DataTableMapperRecursive
        {
            public static List<TParent> MapWithChildren<TParent, TChild>(
                DataTable table,
                string parentKeyColumn,
                Func<DataRow, TParent> parentFactory,
                Func<DataRow, TChild?> childFactory,
                Action<TParent, List<TChild>> childListSetter)
            {
                var grouped = table.AsEnumerable().GroupBy(row => row[parentKeyColumn]);
                var result = new List<TParent>();
                foreach (var g in grouped)
                {
                    var parent = parentFactory(g.First());
                    var children = g
                        .Select(childFactory)
                        .Where(c => c != null)
                        .ToList()!;
                    childListSetter(parent, children);
                    result.Add(parent);
                }
                return result;
            }
        }
    }
}
