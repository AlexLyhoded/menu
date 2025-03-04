using menu.Interface;
using menu.Model;

namespace menu.Repository
{
    public class ReceiptRepository : GenericRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(AppDbContext context) : base(context) { }
    }
}
