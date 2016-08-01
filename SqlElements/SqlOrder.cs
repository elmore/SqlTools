namespace SqlElements
{
    public class SqlOrder
    {
        private readonly string _field;
        private readonly Order _order;

        public SqlOrder(string field, SqlOrder.Order order)
        {
            _field = field;
            _order = order;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _field, _order);
        }

        public enum Order
        {
            Asc,
            Desc
        }
    }
}