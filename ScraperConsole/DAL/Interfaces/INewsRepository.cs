using DTO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<NewsDTO>> GetAllNewsAsync();
        Task<NewsDTO> AddNews(NewsDTO news);
        Task<NewsDTO> GetNewsById(string externalId);
        Task<bool> UpdateNews(string id, NewsDTO news);
    }
}
